using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSpec.Domain.Extensions;

namespace NSpec.Domain
{
    public class Context
    {
        public Type Type { get; set; }

        public void AddExample(Example example)
        {
            example.Context = this;
            Examples.Add(example);

            example.Pending |= example.Context.IsPending();
        }

        public void Afters()
        {
            if (After != null)
                After();
        }

        public void Befores()
        {
            if (Parent != null)
                Parent.Befores();

            if (Before != null)
                Before();
        }

        public void Acts()
        {
            if (Parent != null)
                Parent.Acts();

            if (Act != null)
                Act();
        }

        public IEnumerable<Example> AllExamples()
        {
            return Contexts.Examples().Union(Examples);
        }

        public Context(string name = "") : this(name, 0) { }

        public Context(string name, int level) : this(name, level, false)
        {
            
        }

        public Context(string name, int level, bool isPending)
        {
            Name = name.Replace("_", " ");
            Level = level;
            Examples = new List<Example>();
            Contexts = new ContextCollection();
            this.isPending = isPending;
        }

        protected MethodInfo Method { get; set; }

        public string Name { get; set; }
        public int Level { get; set; }
        public List<Example> Examples { get; set; }
        public ContextCollection Contexts { get; set; }
        public Action Before { get; set; }
        public Action Act { get; set; }
        public Action After { get; set; }
        public Context Parent { get; set; }
        public nspec NSpecInstance { get; set; }

        private bool isPending;
        public bool IsPending()
        {
            if(Parent != null)
            {
                return isPending || Parent.IsPending();
            };
            
            return isPending;
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

        public Action<nspec> BeforeInstance { get; set; }

        public Action<nspec> ActInstance { get; set; }

        public void SetInstanceContext(nspec instance)
        {
            NSpecInstance = instance;

            if (BeforeInstance != null) Before = () => BeforeInstance(instance);

            if (ActInstance != null) Act = () => ActInstance(instance);

            if (Parent != null) Parent.SetInstanceContext(instance);
        }

        public IEnumerable<Context> SelfAndDescendants()
        {
            return new[] { this }.Concat(Contexts.SelectMany(c => c.SelfAndDescendants()));
        }

        public void Run()
        {
            Contexts.Do(c => c.Run());

            if (Method != null)
            {
                nspec instance = CreateNSpecInstance();

                try
                {
                    Method.Invoke(instance, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception executing context: {0}".With(FullContext()));

                    throw e;
                }
            }

            if (IsClassLevelContext())
            {
                Examples.Do(
                e => 
                {
                    nspec instance = CreateNSpecInstance();
                    e.Run(this);
                });
            }
        }

        private nspec CreateNSpecInstance()
        {
            var instance = GetSpecType().Instance<nspec>();

            SetInstanceContext(instance);

            instance.Context = this;

            return instance;
        }

        private bool IsClassLevelContext()
        {
            return this is ClassContext;
        }

        private Type GetSpecType()
        {
            if (Type != null) return Type;

            return Parent.GetSpecType();
        }

        public string FullContext()
        {
            if (Parent != null)
                return Parent.FullContext() + ". " + Name;

            return Name;
        }
    }
}