using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    public class Context
    {
        public void RunBefores(nspec instance)
        {
            if (Parent != null) Parent.RunBefores(instance);

            RunBeforeAll();

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

        private void RunBeforeAll()
        {
            if (BeforeAll == null || BeforeAllHasBeenRun) return;

            BeforeAll();

            BeforeAllHasBeenRun = true;
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
            child.Level = Level + 1;

            child.Parent = this;

            child.Tags.AddRange(child.Parent.Tags);

            Contexts.Add(child);
        }

        public virtual void Run(ILiveFormatter formatter, bool failFast, nspec instance = null)
        {
            if (failFast && Parent.HasAnyFailures()) return;

            var nspec = savedInstance ?? instance;

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

                if(example.HasRun) formatter.Write(example, Level);
            }

            Contexts.Do(c => c.Run(formatter, failFast, nspec));
        }

        private void WriteAncestors(ILiveFormatter formatter)
        {
            if (Parent == null) return;

            Parent.WriteAncestors(formatter);

            if(!alreadyWritten) formatter.Write(this);

            alreadyWritten = true;
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
            if (nspec.tagsFilter.ShouldSkip(example.Tags)) return;

            example.HasRun = true;

            if (example.Pending) return;

            RunAndHandleException(RunBefores, nspec, ref contextLevelException);

            RunAndHandleException(RunActs, nspec, ref contextLevelException);

            RunAndHandleException(example.Run, nspec, ref example.ExampleLevelException);

            RunAndHandleException(RunAfters, nspec, ref contextLevelException);

            example.AssignProperException(contextLevelException);

        }

        public virtual bool IsSub(Type baseType)
        {
            return false;
        }

        public Context(string name = "", string tags = null, bool isPending = false)
        {
            Name = name.Replace("_", " ");
            Examples = new List<Example>();
            Contexts = new ContextCollection();
            Tags = NSpec.Domain.Tags.ParseTags(tags);
            this.isPending = isPending;
        }

        public bool BeforeAllHasBeenRun;
        public string Name;
        public int Level;
        public List<string> Tags;
        public List<Example> Examples;
        public ContextCollection Contexts;
        public Action Before, Act, After, BeforeAll;
        public Action<nspec> BeforeInstance, ActInstance, AfterInstance;
        public Context Parent;
        public Exception contextLevelException;
        public Exception contextLevelExpectedException;
        private bool isPending;
        nspec savedInstance;
        private bool alreadyWritten;

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
    }
}