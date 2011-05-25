using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Afters()
        {
            if (After != null) After();
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
            return AllExamples().Where(e => e.Exception != null);
        }

        public void AddContext(Context child)
        {
            child.Parent = this;

            Contexts.Add(child);
        }

        public virtual void Run(nspec instance=null)
        {
            var nspec = savedInstance ?? instance;

            Contexts.Do(c => c.Run(nspec));

            for (int i = 0; i < Examples.Count; i++)
                Build(Examples[i], nspec);
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

        public void Build(Example example, nspec nspec)
        {
            if (example.Pending) return;

            try
            {
                RunBefores(nspec);

                RunActs(nspec);

                example.Run(nspec);

                Afters();
            }
            catch (Exception e)
            {
                example.Exception = e;
            }
        }

        public virtual bool IsSub(Type baseType)
        {
            return false;
        }

        public Context(string name="", int level=0, bool isPending=false)
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
        public Action<nspec> BeforeInstance, ActInstance;
        public Context Parent;

        private bool isPending;
        nspec savedInstance;
    }
}