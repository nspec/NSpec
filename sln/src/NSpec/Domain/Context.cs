using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSpec.Domain
{
    public class Context
    {
        public void AddExample(ExampleBase example)
        {
            example.AddTo(this);

            Examples.Add(example);

            runnables.Add(new RunnableExample(this, example));
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

            child.Tags.AddRange(Tags);

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

            var runningInstance = builtInstance ?? instance;

            using (new ConsoleCatcher(output => this.CapturedOutput = output))
            {
                BeforeAllChain.Run(runningInstance);
            }

            // intentionally using for loop to prevent collection was modified error in sample specs
            for (int i = 0; i < runnables.Count; i++)
            {
                if (failFast && HasAnyFailures()) return;

                runnables[i].Exercise(runningInstance);
            }

            if (recurse)
            {
                Contexts.Do(c => c.Run(failFast, runningInstance, recurse));
            }

            // TODO wrap this as well in a ConsoleCatcher, not before adding tests about it
            AfterAllChain.Run(runningInstance);
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
            var beforeAllException = BeforeAllChain.AnyException();
            var afterAllException = AfterAllChain.AnyException();

            for (int i = 0; i < runnables.Count; i++)
            {
                runnables[i].AssignException(beforeAllException, afterAllException);
            }

            if (recurse)
            {
                Contexts.Do(c => c.AssignExceptions(recurse));
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
                runnables[i].Write(formatter);
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
            string contextText = $"{Contexts.Count} ctx";

            var exception =
                BeforeAllChain.AnyException() ??
                BeforeChain.AnyException() ??
                ActChain.AnyException() ??
                AfterChain.AnyException() ??
                AfterAllChain.AnyException();

            string exceptionText = exception?.GetType().Name ?? String.Empty;

            return String.Join(",", new []
            {
               Name, levelText, exampleText, contextText, exceptionText,
            });
        }

        public void WriteAncestors(ILiveFormatter formatter)
        {
            if (alreadyWritten) return;

            // when hitting root `nspec` context, skip it
            if (Parent == null)
            {
                alreadyWritten = true;
                return;
            }

            Parent.WriteAncestors(formatter);

            formatter.Write(this);

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

        public Context(string name = "", string tags = null, bool isPending = false, Conventions conventions = null)
        {
            Name = name.Replace("_", " ");
            Tags = Domain.Tags.ParseTags(tags);
            this.isPending = isPending;

            Examples = new List<ExampleBase>();
            Contexts = new ContextCollection();

            if (conventions == null) conventions = new DefaultConventions().Initialize();

            runnables = new List<RunnableExample>();

            BeforeAllChain = new BeforeAllChain(this, conventions);
            BeforeChain = new BeforeChain(this, conventions);
            ActChain = new ActChain(this, conventions);
            AfterChain = new AfterChain(this, conventions);
            AfterAllChain = new AfterAllChain(this, conventions);
        }

        public string Name { get; protected set; }
        public int Level { get; protected set; }
        public List<string> Tags { get; protected set; }
        public List<ExampleBase> Examples { get; protected set; }
        public ContextCollection Contexts { get; protected set; }
        public BeforeAllChain BeforeAllChain { get; protected set; }
        public BeforeChain BeforeChain { get; protected set; }
        public ActChain ActChain { get; protected set; }
        public AfterChain AfterChain { get; protected set; }
        public AfterAllChain AfterAllChain { get; protected set; }
        public bool ClearExpectedException;
        public string CapturedOutput { get; protected set; }
        public Context Parent { get; protected set; }

        protected List<RunnableExample> runnables;
        protected nspec builtInstance;
        protected bool alreadyWritten;
        protected bool isPending;
    }
}
