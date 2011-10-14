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

            Contexts.Add(child);
        }

        public virtual void Run(nspec instance = null)
        {
            var nspec = savedInstance ?? instance;

            Contexts.Do(c => c.Run(nspec));

            for (int i = 0; i < Examples.Count; i++)
                Exercise(Examples[i], nspec);
        }

        public virtual void Build(nspec instance=null)
        {
            instance.Context = this;

            savedInstance = instance;

            Contexts.Do(c => c.Build(instance));
        }

        public string FullContext()
        {
            return Parent != null ? Parent.FullContext() + ". " + Name : Name;
        }

		public void RunAndHandleException( Action<nspec> action, nspec nspec, ref Exception exceptionRef )
		{
            try
            {
            	action( nspec );
            }
            catch( TargetInvocationException invocationException )
            {
				if( exceptionRef == null )
					exceptionRef = invocationException.InnerException;
            }
            catch( Exception exception )
            {
				if( exceptionRef == null )
					exceptionRef = exception;
            }
		}

    	public void Exercise(Example example, nspec nspec)
        {
            if (example.Pending) return;

            // run context-level steps (arrange and act)
            // note: exceptions that occur during 'before/act' should set the context-level exception
    		RunAndHandleException( RunBefores, nspec, ref contextLevelException );
    		RunAndHandleException( RunActs, nspec, ref contextLevelException );

            // run example step (the assert) for the current context
            // note: exceptions that occur during 'example' verification should set the example-level exception
			RunAndHandleException( example.Run, nspec, ref example.ExampleLevelException );

            // run context-level teardown step
            // note: exceptions that occur during 'after' should set the context-level exception
			RunAndHandleException( RunAfters, nspec, ref contextLevelException );

            // update example's exception status if there was a context-level failure
            if( example.ExampleLevelException == null && contextLevelException != null )
                example.ExampleLevelException = new ContextFailureException( "Exception was thrown during context setup or teardown",
                                                                             contextLevelException );
        }

        public virtual bool IsSub(Type baseType)
        {
            return false;
        }

        public Context(string name = "", int level = 0, bool isPending = false)
        {
            Name = name.Replace("_", " ");
            Level = level;
            Examples = new List<Example>();
            Contexts = new ContextCollection();
            this.isPending = isPending;
        }

        public string Name;
        public int Level;
        public List<Example> Examples;
        public ContextCollection Contexts;
        public Action Before, Act, After;
        public Action<nspec> BeforeInstance, ActInstance, AfterInstance;
        public Context Parent;
        public Exception contextLevelException;
        private bool isPending;
        nspec savedInstance;

        public nspec GetInstance()
        {
            return savedInstance ?? Parent.GetInstance();
        }

        public IEnumerable<Context> AllContexts()
        {
            return new[] {this}.Union(Contexts.SelectMany(c => c.AllContexts()));
        }
    }
}