using NSpec.Domain.Extensions;
using NSpec.Domain.Formatters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NSpec.Domain
{
    public class Context
    {
        public void RunBefores(nspec instance)
        {
            // parent chain

            RecurseAncestors(c => c.RunBefores(instance));

            // class (method-level)

            if (BeforeInstance != null && BeforeInstanceAsync != null)
            {
                throw new AsyncMismatchException(
                    "A spec class with all its ancestors cannot set both sync and async " +
                    "class-level 'before_each' hooks, they should either be all sync or all async");
            }

            BeforeInstance.SafeInvoke(instance);

            BeforeInstanceAsync.SafeInvoke(instance);

            // context-level

            if (Before != null && BeforeAsync != null)
            {
                throw new AsyncMismatchException(
                    "A single context cannot set both a 'before' and an 'beforeAsync', please pick one of the two");
            }

            if (Before != null && Before.IsAsync())
            {
                throw new AsyncMismatchException(
                    "'before' cannot be set to an async delegate, please use 'beforeAsync' instead");
            }

            Before.SafeInvoke();

            BeforeAsync.SafeInvoke();
        }

        void RunBeforeAll(nspec instance)
        {
            // context-level

            if (BeforeAll != null && BeforeAllAsync != null)
            {
                throw new AsyncMismatchException(
                    "A single context cannot set both a 'beforeAll' and an 'beforeAllAsync', please pick one of the two");
            }

            if (BeforeAll != null && BeforeAll.IsAsync())
            {
                throw new AsyncMismatchException(
                    "'beforeAll' cannot be set to an async delegate, please use 'beforeAllAsync' instead");
            }

            BeforeAll.SafeInvoke();

            BeforeAllAsync.SafeInvoke();

            // class (method-level)

            if (BeforeAllInstance != null && BeforeAllInstanceAsync != null)
            {
                throw new AsyncMismatchException(
                    "A spec class with all its ancestors cannot set both sync and async class-level 'before_all' hooks, they should either be all sync or all async");
            }

            BeforeAllInstance.SafeInvoke(instance);

            BeforeAllInstanceAsync.SafeInvoke(instance);
        }

        public void RunActs(nspec instance)
        {
            // parent chain

            RecurseAncestors(c => c.RunActs(instance));

            // class (method-level)

            if (ActInstance != null && ActInstanceAsync != null)
            {
                throw new AsyncMismatchException(
                    "A spec class with all its ancestors cannot set both sync and async class-level 'act_each' hooks, they should either be all sync or all async");
            }

            ActInstance.SafeInvoke(instance);

            ActInstanceAsync.SafeInvoke(instance);

            // context-level

            if (Act != null && ActAsync != null)
            {
                throw new AsyncMismatchException(
                    "A single context cannot set both an 'act' and an 'actAsync', please pick one of the two");
            }

            if (Act != null && Act.IsAsync())
            {
                throw new AsyncMismatchException(
                    "'act' cannot be set to an async delegate, please use 'actAsync' instead");
            }

            Act.SafeInvoke();

            ActAsync.SafeInvoke();
        }

        public void RunAfters(nspec instance)
        {
            // context-level

            if (After != null && AfterAsync != null)
            {
                throw new AsyncMismatchException(
                    "A single context cannot set both an 'after' and an 'afterAsync', please pick one of the two");
            }

            if (After != null && After.IsAsync())
            {
                throw new AsyncMismatchException(
                    "'after' cannot be set to an async delegate, please use 'afterAsync' instead");
            }

            After.SafeInvoke();

            AfterAsync.SafeInvoke();

            // class (method-level)

            if (AfterInstance != null && AfterInstanceAsync != null)
            {
                throw new AsyncMismatchException(
                    "A spec class with all its ancestors cannot set both sync and async class-level 'after_each' hooks, they should either be all sync or all async");
            }

            AfterInstance.SafeInvoke(instance);

            AfterInstanceAsync.SafeInvoke(instance);

            // parent chain

            RecurseAncestors(c => c.RunAfters(instance));
        }

        public void RunAfterAll(nspec instance)
        {
            // context-level

            if (AfterAll != null && AfterAllAsync != null)
            {
                throw new AsyncMismatchException(
                    "A single context cannot set both an 'afterAll' and an 'afterAllAsync', please pick one of the two");
            }

            if (AfterAll != null && AfterAll.IsAsync())
            {
                throw new AsyncMismatchException(
                    "'afterAll' cannot be set to an async delegate, please use 'afterAllAsync' instead");
            }

            AfterAll.SafeInvoke();

            AfterAllAsync.SafeInvoke();

            // class (method-level)

            if (AfterAllInstance != null && AfterAllInstanceAsync != null)
            {
                throw new AsyncMismatchException(
                    "A spec class with all its ancestors cannot set both sync and async class-level 'after_all' hooks, they should either be all sync or all async");
            }

            AfterAllInstance.SafeInvoke(instance);

            AfterAllInstanceAsync.SafeInvoke(instance);
        }

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
        /// Test execution happens in two phases: this is the first phase.
        /// </summary>
        /// <remarks>
        /// Here all contexts and all their examples are run, collecting distinct exceptions
        /// from context itself (befores/ acts/ it/ afters), beforeAll, afterAll.
        /// </remarks>
        public virtual void Run(ILiveFormatter formatter, bool failFast, nspec instance = null)
        {
            if (failFast && Parent.HasAnyFailures()) return;

            var nspec = savedInstance ?? instance;

            bool runBeforeAfterAll = AnyUnfilteredExampleInSubTree(nspec);

            if (runBeforeAfterAll) RunAndHandleException(RunBeforeAll, nspec, ref ExceptionBeforeAll);

            // intentionally using for loop to prevent collection was modified error in sample specs
            for (int i = 0; i < Examples.Count; i++)
            {
                var example = Examples[i];

                if (failFast && example.Context.HasAnyFailures()) return;

                Exercise(example, nspec);

                if (example.HasRun && !alreadyWritten)
                {
                    WriteAncestors(formatter);
                    alreadyWritten = true;
                }

                if (example.HasRun) formatter.Write(example, Level);
            }

            Contexts.Do(c => c.Run(formatter, failFast, nspec));

            if (runBeforeAfterAll) RunAndHandleException(RunAfterAll, nspec, ref ExceptionAfterAll);
        }

        /// <summary>
        /// Test execution happens in two phases: this is the second phase.
        /// </summary>
        /// <remarks>
        /// Here all contexts and all their examples are traversed again to set proper exception
        /// on examples, giving priority to exceptions from: inherithed beforeAll, beforeAll,
        /// context (befores/ acts/ it/ afters), afterAll, inherithed afterAll.
        /// </remarks>
        public virtual void AssignExceptions()
        {
            AssignExceptions(null, null);
        }

        protected virtual void AssignExceptions(Exception inheritedBeforeAllException, Exception inheritedAfterAllException)
        {
            inheritedBeforeAllException = inheritedBeforeAllException ?? ExceptionBeforeAll;
            inheritedAfterAllException = ExceptionAfterAll ?? inheritedAfterAllException;

            // if thrown exception was correctly expected, ignore this context Exception
            Exception unexpectedException = ClearExpectedException ? null : Exception;

            Exception contextException = (inheritedBeforeAllException ?? unexpectedException) ?? inheritedAfterAllException;

            for (int i = 0; i < Examples.Count; i++)
            {
                var example = Examples[i];

                if (!example.Pending)
                {
                    example.AssignProperException(contextException);
                }
            }

            Contexts.Do(c => c.AssignExceptions(inheritedBeforeAllException, inheritedAfterAllException));
        }

        public virtual void Build(nspec instance = null)
        {
            instance.Context = this;

            savedInstance = instance;

            Contexts.Do(c => c.Build(instance));
        }

        public string FullContext()
        {
            return Parent != null ? Parent.FullContext() + ". " + Name : Name;
        }

        public bool RunAndHandleException(Action<nspec> action, nspec nspec, ref Exception exceptionToSet)
        {
            bool hasThrown = false;

            try
            {
                action(nspec);
            }
            catch (TargetInvocationException invocationException)
            {
                if (exceptionToSet == null) exceptionToSet = nspec.ExceptionToReturn(invocationException.InnerException);

                hasThrown = true;
            }
            catch (Exception exception)
            {
                if (exceptionToSet == null) exceptionToSet = nspec.ExceptionToReturn(exception);

                hasThrown = true;
            }

            return hasThrown;
        }

        public void Exercise(ExampleBase example, nspec nspec)
        {
            if (example.ShouldSkip(nspec.tagsFilter))
            {
                Action<nspec> dummyExampleRun = _ => HandleSkipped(example);

                RunAndHandleException(dummyExampleRun, nspec, ref example.Exception);

                return;
            }

            var sw = new Stopwatch();
            sw.Start();

            RunAndHandleException(RunBefores, nspec, ref Exception);

            RunAndHandleException(RunActs, nspec, ref Exception);

            RunAndHandleException(example.Run, nspec, ref example.Exception);

            bool exceptionThrownInAfters = RunAndHandleException(RunAfters, nspec, ref Exception);

            sw.Stop();
            example.Duration = sw.Elapsed;

            // when an expected exception is thrown and is set to be cleared by 'expect<>',
            // a subsequent exception thrown in 'after' hooks would go unnoticed, so do not clear in this case

            if (exceptionThrownInAfters) ClearExpectedException = false;
        }

        public virtual bool IsSub(Type baseType)
        {
            return false;
        }

        public nspec GetInstance()
        {
            return savedInstance ?? Parent.GetInstance();
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

        bool AnyUnfilteredExampleInSubTree(nspec nspec)
        {
            Func<ExampleBase, bool> shouldNotSkip = e => e.ShouldNotSkip(nspec.tagsFilter);

            bool anyExampleOrSubExample = Examples.Any(shouldNotSkip) || Contexts.Examples().Any(shouldNotSkip);

            return anyExampleOrSubExample;
        }

        void HandleSkipped(ExampleBase example)
        {
            if (example != null && example.IsAsync)
            {
                throw new AsyncMismatchException(
                    "'xit' cannot be set to an async delegate, please use 'xitAsync' instead");
            }
        }

        public override string ToString()
        {
            string exceptionText = (Exception != null ? ", " + Exception.GetType().Name : String.Empty);

            return String.Format("{0}, L{1}, {2} exm, {3} ctx{4}", Name, Level, Examples.Count, Contexts.Count, exceptionText);
        }

        void RecurseAncestors(Action<Context> ancestorAction)
        {
            if (Parent != null) ancestorAction(Parent);
        }

        void WriteAncestors(ILiveFormatter formatter)
        {
            if (Parent == null) return;

            Parent.WriteAncestors(formatter);

            if (!alreadyWritten) formatter.Write(this);

            alreadyWritten = true;
        }

        public Context(string name = "", string tags = null, bool isPending = false)
        {
            Name = name.Replace("_", " ");
            Examples = new List<ExampleBase>();
            Contexts = new ContextCollection();
            Tags = Domain.Tags.ParseTags(tags);
            this.isPending = isPending;
        }

        public string Name;
        public int Level;
        public List<string> Tags;
        public List<ExampleBase> Examples;
        public ContextCollection Contexts;
        public Action Before, Act, After, BeforeAll, AfterAll;
        public Action<nspec> BeforeInstance, ActInstance, AfterInstance, BeforeAllInstance, AfterAllInstance;
        public Func<Task> BeforeAsync, ActAsync, AfterAsync, BeforeAllAsync, AfterAllAsync;
        public Action<nspec> BeforeInstanceAsync, ActInstanceAsync, AfterInstanceAsync, BeforeAllInstanceAsync, AfterAllInstanceAsync;
        public Context Parent;
        public Exception ExceptionBeforeAll, Exception, ExceptionAfterAll;
        public bool ClearExpectedException;

        nspec savedInstance;
        bool alreadyWritten, isPending;
    }
}