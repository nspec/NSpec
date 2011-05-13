using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class Context
    {
        public void Befores()
        {
            if (Parent != null) Parent.Befores();

            if (Before != null) Before();
        }

        public void Acts()
        {
            if (Parent != null) Parent.Acts();

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

        public virtual void Run()
        {
            Contexts.Do(c => c.Run());
        }

        protected nspec CreateNSpecInstance()
        {
            NSpecInstance = GetSpecType().Instance<nspec>();

            SetInstanceContext(NSpecInstance);

            NSpecInstance.Context = this;

            return NSpecInstance;
        }

        private Type GetSpecType()
        {
            return Type ?? Parent.GetSpecType();
        }

        public void SetInstanceContext(nspec instance)
        {
            if (BeforeInstance != null) Before = () => BeforeInstance(instance);

            if (ActInstance != null) Act = () => ActInstance(instance);

            if (Parent != null) Parent.SetInstanceContext(instance);
        }

        public string FullContext()
        {
            return Parent != null ? Parent.FullContext() + ". " + Name : Name;
        }

        public Context(string name = "") : this(name, 0) { }

        public Context(string name, int level) : this(name, level, false){ }

        public Context(string name, int level, bool isPending)
        {
            Name = name.Replace("_", " ");
            Level = level;
            Examples = new List<Example>();
            Contexts = new ContextCollection();
            this.isPending = isPending;
        }

        protected MethodInfo Method;

        public Type Type;
        public string Name;
        public int Level;
        public List<Example> Examples;
        public ContextCollection Contexts;
        public Action Before, Act, After;
        public Action<nspec> BeforeInstance, ActInstance;
        public Context Parent;
        public nspec NSpecInstance;

        private bool isPending;
    }
}