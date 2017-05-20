using NSpec.Domain.Extensions;
using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NSpec.Domain
{
    public class Context
    {
        public void AddExample(ExampleBase example)
        {
            example.Context = this;

            example.Tags.AddRange(Tags);

            Examples.Add(example);

            example.Pending |= IsPending();
        }

        public IEnumerable<ExampleBase> AllExamples()
        {
            return Contexts.Examples().Union(Examples);
        }

        public bool IsPending()
        {
            return isPending || (Parent != null && Parent.IsPending());
        }

        public IEnumerable<ExampleBase> Failures()
        {
            return AllExamples().Where(e => e.Exception != null);
        }

        public void AddContext(Context child)
        {
            child.Level = Level + 1;

            child.Parent = this;

            child.Tags.AddRange(child.Parent.Tags);

            Contexts.Add(child);
        }

        /// <summary>
        /// Test execution happens in three phases: this is the first phase.
        /// </summary>
        /// <remarks>
        /// Here all contexts and all their examples are run, collecting distinct exceptions
        /// from context itself (befores/ acts/ it/ afters), beforeAll, afterAll.
        /// </remarks>
        public virtual void Run(bool failFast, nspec instance = null, bool recurse = true)
        {
            if (failFast && Parent.HasAnyFailures()) return;

            var nspec = builtInstance ?? instance;

            using (new ConsoleCatcher(output => this.CapturedOutput = output))
            {
                BeforeAllChain.Run(nspec);
            }

            // intentionally using for loop to prevent collection was modified error in sample specs
            for (int i = 0; i < Examples.Count; i++)
            {
                var example = Examples[i];

                if (failFast && example.Context.HasAnyFailures()) return;

                Exercise(example, nspec);
            }

            if (recurse)
            {
                Contexts.Do(c => c.Run(failFast, nspec, recurse));
            }

            // TODO wrap this as well in a ConsoleCatcher, not before adding tests about it
            AfterAllChain.Run(nspec);
        }

        /// <summary>
        /// Test execution happens in three phases: this is the second phase.
        /// </summary>
        /// <remarks>
        /// Here all contexts and all their examples are traversed again to set proper exception
        /// on examples, giving priority to exceptions from: inherithed beforeAll, beforeAll,
        /// context (befores/ acts/ it/ afters), afterAll, inherithed afterAll.
        /// </remarks>
        public virtual void AssignExceptions(bool recurse = true)
        {
            AssignExceptions(null, null, recurse);
        }

        protected virtual void AssignExceptions(Exception inheritedBeforeAllException, Exception inheritedAfterAllException, bool recurse)
        {
            inheritedBeforeAllException = inheritedBeforeAllException ?? BeforeAllChain.Exception;
            inheritedAfterAllException = AfterAllChain.Exception ?? inheritedAfterAllException;

            // if an exception was thrown before the example (either `before` or `act`) but was expected, ignore it
            Exception unexpectedException = ClearExpectedException ? null : BeforeChain.Exception ?? ActChain.Exception;

            Exception previousException = inheritedBeforeAllException ?? unexpectedException;
            Exception followingException = AfterChain.Exception ?? inheritedAfterAllException;

            for (int i = 0; i < Examples.Count; i++)
            {
                var example = Examples[i];

                if (!example.Pending)
                {
                    example.AssignProperException(previousException, followingException);
                }
            }

            if (recurse)
            {
                Contexts.Do(c => c.AssignExceptions(inheritedBeforeAllException, inheritedAfterAllException, recurse));
            }
        }

        /// <summary>
        /// Test execution happens in three phases: this is the third phase.
        /// </summary>
        /// <remarks>
        /// Here all examples are written out to formatter, together with their contexts,
        /// befores, acts, afters, beforeAll, afterAll.
        /// </remarks>
        public virtual void Write(ILiveFormatter formatter, bool recurse = true)
        {
            for (int i = 0; i < Examples.Count; i++)
            {
                var example = Examples[i];

                if (example.HasRun && !alreadyWritten)
                {
                    WriteAncestors(formatter);
                }

                if (example.HasRun) formatter.Write(example, Level);
            }

            if (recurse)
            {
                Contexts.Do(c => c.Write(formatter, recurse));
            }
        }

        public virtual void Build(nspec instance = null)
        {
            instance.Context = this;

            builtInstance = instance;

            Contexts.Do(c => c.Build(instance));
        }

        public string FullContext()
        {
            return Parent != null ? Parent.FullContext() + ". " + Name : Name;
        }

        public void Exercise(ExampleBase example, nspec nspec)
        {
            if (example.ShouldSkip(nspec.tagsFilter))
            {
                return;
            }

            example.HasRun = true;

            if (example.Pending)
            {
                HookChainBase.RunAndHandleException(example.RunPending, nspec, ref example.Exception);

                return;
            }

            var stopWatch = example.StartTiming();

            using (new ConsoleCatcher(output => example.CapturedOutput = output))
            {
                BeforeChain.Run(nspec);

                ActChain.Run(nspec);

                RunExample(example, nspec);

                AfterChain.Run(nspec);
            }

            // when an expected exception is thrown and is set to be cleared by 'expect<>',
            // a subsequent exception thrown in 'after' hooks would go unnoticed, so do not clear in this case

            if (AfterChain.Exception != null) ClearExpectedException = false;

            example.StopTiming(stopWatch);
        }

        void RunExample(ExampleBase example, nspec nspec)
        {
            if (BeforeAllChain.AnyBeforeAllsThrew()) return;

            if (BeforeChain.Exception != null) return;

            HookChainBase.RunAndHandleException(example.Run, nspec, ref example.Exception);
        }

        public virtual bool IsSub(Type baseType)
        {
            return false;
        }

        public nspec GetInstance()
        {
            return builtInstance ?? Parent.GetInstance();
        }

        public IEnumerable<Context> AllContexts()
        {
            return new[] { this }.Union(ChildContexts());
        }

        public IEnumerable<Context> ChildContexts()
        {
            return Contexts.SelectMany(c => new[] { c }.Union(c.ChildContexts()));
        }

        public bool HasAnyFailures()
        {
            return AllExamples().Any(e => e.Failed());
        }

        public bool HasAnyExecutedExample()
        {
            return AllExamples().Any(e => e.HasRun);
        }

        public void TrimSkippedDescendants()
        {
            Contexts.RemoveAll(c => !c.HasAnyExecutedExample());

            Examples.RemoveAll(e => !e.HasRun);

            Contexts.Do(c => c.TrimSkippedDescendants());
        }

        public bool AnyUnfilteredExampleInSubTree(nspec instance)
        {
            Func<ExampleBase, bool> shouldNotSkip = e => e.ShouldNotSkip(instance.tagsFilter);

            bool anyExampleOrSubExample =
                Examples.Any(shouldNotSkip) ||
                Contexts.Examples().Any(shouldNotSkip);

            return anyExampleOrSubExample;
        }

        public override string ToString()
        {
            string levelText = $"L{Level}";
            string exampleText = $"{Examples.Count} exm";
            string contextText = $"{Contexts.Count} exm";

            var exception = BeforeChain.Exception ?? ActChain.Exception ?? AfterChain.Exception;
            string exceptionText = exception?.GetType().Name ?? String.Empty;

            return String.Join(",", new []
            {
               Name, levelText, exampleText, contextText, exceptionText, 
            });
        }

        void WriteAncestors(ILiveFormatter formatter)
        {
            if (Parent == null)
            {
                alreadyWritten = true;
                return;
            }

            Parent.WriteAncestors(formatter);

            if (!alreadyWritten) formatter.Write(this);

            alreadyWritten = true;
        }

        // Context-level hook wrappers

        public Action BeforeAll
        {
            get { return BeforeAllChain.Hook; }
            set { BeforeAllChain.Hook = value; }
        }

        public Action Before
        {
            get { return BeforeChain.Hook; }
            set { BeforeChain.Hook = value; }
        }

        public Action Act
        {
            get { return ActChain.Hook; }
            set { ActChain.Hook = value; }
        }

        public Action After
        {
            get { return AfterChain.Hook; }
            set { AfterChain.Hook = value; }
        }

        public Action AfterAll
        {
            get { return AfterAllChain.Hook; }
            set { AfterAllChain.Hook = value; }
        }

        // Class/method-level hook wrappers

        public Action<nspec> BeforeAllInstance
        {
            get { return BeforeAllChain.ClassHook; }
        }

        public Action<nspec> BeforeInstance
        {
            get { return BeforeChain.ClassHook; }
        }

        public Action<nspec> ActInstance
        {
            get { return ActChain.ClassHook; }
        }

        public Action<nspec> AfterInstance
        {
            get { return AfterChain.ClassHook; }
        }

        public Action<nspec> AfterAllInstance
        {
            get { return AfterAllChain.ClassHook; }
        }

        // Context-level async hook wrappers

        public Func<Task> BeforeAllAsync
        {
            get { return BeforeAllChain.AsyncHook; }
            set { BeforeAllChain.AsyncHook = value; }
        }

        public Func<Task> BeforeAsync
        {
            get { return BeforeChain.AsyncHook; }
            set { BeforeChain.AsyncHook = value; }
        }

        public Func<Task> ActAsync
        {
            get { return ActChain.AsyncHook; }
            set { ActChain.AsyncHook = value; }
        }

        public Func<Task> AfterAsync
        {
            get { return AfterChain.AsyncHook; }
            set { AfterChain.AsyncHook = value; }
        }

        public Func<Task> AfterAllAsync
        {
            get { return AfterAllChain.AsyncHook; }
            set { AfterAllChain.AsyncHook = value; }
        }

        // Class/method-level async hook wrappers

        public Action<nspec> BeforeAllInstanceAsync
        {
            get { return BeforeAllChain.AsyncClassHook; }
        }

        public Action<nspec> BeforeInstanceAsync
        {
            get { return BeforeChain.AsyncClassHook; }
        }

        public Action<nspec> ActInstanceAsync
        {
            get { return ActChain.AsyncClassHook; }
        }

        public Action<nspec> AfterInstanceAsync
        {
            get { return AfterChain.AsyncClassHook; }
        }

        public Action<nspec> AfterAllInstanceAsync
        {
            get { return AfterAllChain.AsyncClassHook; }
        }

        public Context(string name = "", string tags = null, bool isPending = false)
        {
            Name = name.Replace("_", " ");
            Tags = Domain.Tags.ParseTags(tags);
            this.isPending = isPending;
            
            Examples = new List<ExampleBase>();
            Contexts = new ContextCollection();

            BeforeAllChain = new BeforeAllChain(this);
            BeforeChain = new BeforeChain(this);
            ActChain = new ActChain(this);
            AfterChain = new AfterChain(this);
            AfterAllChain = new AfterAllChain(this);
        }

        public string Name;
        public int Level;
        public List<string> Tags;
        public List<ExampleBase> Examples;
        public ContextCollection Contexts;
        public BeforeAllChain BeforeAllChain;
        public BeforeChain BeforeChain;
        public ActChain ActChain;
        public AfterChain AfterChain;
        public AfterAllChain AfterAllChain;
        public bool ClearExpectedException;
        public string CapturedOutput;
        public Context Parent;
        
        nspec builtInstance;
        bool alreadyWritten, isPending;
    }
}
