using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Extensions;
using NSpec.Interpreter.Indexer;

namespace NSpec.Domain
{
    public class Context
    {
        public Type Type { get; set; }
        private readonly Action<spec> beforeInstance;

        public void AddExample(Example example)
        {
            Examples.Add(example);
        }

        public override string ToString()
        {
            if (AllExamples().Count() == 0) return "";

            var context = string.Format("\t".Times(Level) + "{0}", Name);

            Examples.Do(e => context += Environment.NewLine + "\t".Times(Level) + e );

            Contexts.Do(c => context += Environment.NewLine + c.ToString());

            return context;
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

            if (BeforeFrequency == "all")
                Before = null;
        }

        public void Acts()
        {
            if (Parent != null)
                Parent.Acts();

            if (Act != null)
                Act();

            if (ActFrequency == "all")
                Act = null;
        }

        public IEnumerable<Example> AllExamples()
        {
            return Contexts.SelectMany(c => c.AllExamples()).Union(Examples);
        }

        public Context(string name) :this(name,0) { }

        public Context(Type type) :this(type.Name,0)
        {
            Type = type;
            BeforeInstance = type.GetBefore();
        }

        public Context(string name, int level)
        {
            Name = name;
            Level = level;
            Examples = new List<Example>();
            Contexts = new List<Context>();
        }
        public string Name { get; set; }
        public int Level { get; set; }
        public List<Example> Examples { get; set; }
        public List<Context> Contexts { get; set; }
        public Action Before { get; set; }
        public Action Act { get; set; }
        public Action After { get; set; }
        public Context Parent { get; set; }
        public string AfterFrequency { get; set; }
        public string BeforeFrequency { get; set; }
        public string ActFrequency { get; set; }

        public IEnumerable<Example> Failures()
        {
            return AllExamples().Where(e => e.Exception != null);
        }

        public void AddContext(Context child)
        {
            child.Parent = this;
            Contexts.Add(child);
        }

        public Action<spec> BeforeInstance { get; set; }

        public void SetInstanceContext(spec instance)
        {
            if (BeforeInstance != null) Before = () => BeforeInstance(instance);
            Contexts.Do(c => c.SetInstanceContext(instance));
        }

        public IEnumerable<Context> SelfAndDescendants()
        {
            return new[] { this }.Concat(Contexts.SelectMany(c => c.SelfAndDescendants()));
        }
    }
}