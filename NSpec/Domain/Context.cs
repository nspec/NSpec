using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
        }

        public void Afters()
        {
            if (After != null)
                After();
        }

        public void Befores()
        {
            if (Parent != null )
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

        public Context(string name="") : this(name,0) { }

        public Context(Type type) : this(type.Name,0)
        {
            Type = type;
            BeforeInstance = type.GetBefore();
        }

        public Context(MethodInfo method) : this(method.Name,0)
        {
            Method = method;
        }

        public Context(string name, int level)
        {
            Name = MakeSentence(name);
            Level = level;
            Examples = new List<Example>();
            Contexts = new ContextCollection();
        }

        private string MakeSentence(string name)
        {
            name = name.Replace("_", " ");

            return Regex.Replace(name, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
            //supplied generously by Steven on stackoverflow
            //http://stackoverflow.com/questions/323314/best-way-to-convert-pascal-case-to-a-sentence/1211435#1211435
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

        public void SetInstanceContext(nspec instance)
        {
            if (BeforeInstance != null) Before = () => BeforeInstance(instance);

            if(Parent!=null) Parent.SetInstanceContext(instance);
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
                var instance = GetSpecType().Instance<nspec>();

                SetInstanceContext(instance);

                instance.Context = this;

                try
                {
                    Method.Invoke(instance, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception executing context: {0}".With(Method.Name));

                    throw e;
                }
            }
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