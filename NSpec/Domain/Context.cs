using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSpec.Domain
{
    public class Context
    {
        public void RunBefores(nspec instance)
        {
            if (Parent != null) Parent.RunBefores(instance);

            if (BeforeInstance != null) BeforeInstance(instance);

            if (Before != null) Before();
        }

        public void RunActs(nspec instance)
        {
            if (Parent != null) Parent.RunActs(instance);

            if (ActInstance != null) ActInstance(instance);

            if (Act != null) Act();
        }

        public void RunAfters(nspec instance)
        {
            if (After != null) After();

            if (AfterInstance != null) AfterInstance(instance);

            if (Parent != null) Parent.RunAfters(instance);
        }

        public void AddExample(Example example)
        {
            example.Context = this;

            example.Tags.AddRange(Tags);

            Examples.Add(example);

            example.Pending |= IsPending();
        }

        public IEnumerable<Example> AllExamples()
        {
            return Contexts.Examples().Union(Examples);
        }

        public bool IsPending()
        {
            return isPending || (Parent != null && Parent.IsPending());
        }

        public IEnumerable<Example> Failures()
        {
            return AllExamples().Where(e => e.ExampleLevelException != null);
        }

        public void AddContext(Context child)
        {
            child.Parent = this;

            child.Tags.AddRange(child.Parent.Tags);

            Contexts.Add(child);
        }

        public virtual void Run(nspec instance = null)
        {
            var nspec = savedInstance ?? instance;

            Contexts.Do(c => c.Run(nspec));

            for (int i = 0; i < Examples.Count; i++) Exercise(Examples[i], nspec);
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

        public void RunAndHandleException(Action<nspec> action, nspec nspec, ref Exception exceptionToSet)
        {
            try
            {
                action(nspec);
            }
            catch (TargetInvocationException invocationException)
            {
                if (exceptionToSet == null) exceptionToSet = invocationException.InnerException;
            }
            catch (Exception exception)
            {
                if (exceptionToSet == null) exceptionToSet = exception;
            }
        }

        public void Exercise(Example example, nspec nspec)
        {
            if (example.Pending) return;

            if (nspec.tagsFilter.ShouldSkip(example.Tags)) return;

            RunAndHandleException(RunBefores, nspec, ref contextLevelException);

            RunAndHandleException(RunActs, nspec, ref contextLevelException);

            RunAndHandleException(example.Run, nspec, ref example.ExampleLevelException);

            RunAndHandleException(RunAfters, nspec, ref contextLevelException);

            example.HasRun = true;

            if (example.ExampleLevelException == null && contextLevelException != null)
                example.ExampleLevelException = new ContextFailureException("Exception thrown during context's befores, acts or afters", contextLevelException);
        }

        public virtual bool IsSub(Type baseType)
        {
            return false;
        }

        public Context(string name = "", string tags = null, int level = 0, bool isPending = false)
        {
            Name = name.Replace("_", " ");
            Level = level;
            Examples = new List<Example>();
            Contexts = new ContextCollection();
            Tags = NSpec.Domain.Tags.ParseTags(tags);
            this.isPending = isPending;
        }

        public string Name;
        public int Level;
        public List<string> Tags;
        public List<Example> Examples;
        public ContextCollection Contexts;
        public Action Before, Act, After;
        public Action<nspec> BeforeInstance, ActInstance, AfterInstance;
        public Context Parent;
        public Exception contextLevelException;
        public Exception contextLevelExpectedException;
        private bool isPending;
        nspec savedInstance;

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

        public bool HasAnyExecutedExample()
        {
            return AllExamples().Any(e => e.HasRun);
        }

        public void TrimSkippedDescendants()
        {
            Contexts.RemoveAll(c => !c.HasAnyExecutedExample());

            Contexts.Do(c => c.TrimSkippedDescendants());
        }
    }
}