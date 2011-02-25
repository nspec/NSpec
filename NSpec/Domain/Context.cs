using System;
using System.Collections.Generic;
using System.Linq;
using NSpec.Extensions;

namespace NSpec.Domain
{
    public class Context
    {
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
            if (Parent != null && Parent.Before != null)
                Parent.Befores();

            if (Before != null)
                Before();

            if (BeforeFrequency == "all")
                Before = null;
        }

        public IEnumerable<Example> AllExamples()
        {
            return Contexts.SelectMany(c => c.AllExamples()).Union(Examples);
        }

        public Context(string name) :this(name,0, "given") { }

        public Context(string name, int level, string prefix)
        {
            Name = "{0} {1}".With(prefix,name);
            Level = level;
            Examples = new List<Example>();
            Contexts = new List<Context>();
        }
        public string Name { get; set; }
        public int Level { get; set; }
        public List<Example> Examples { get; set; }
        public List<Context> Contexts { get; set; }
        public Action Before { get; set; }
        public Action After { get; set; }
        public Context Parent { get; set; }
        public string AfterFrequency { get; set; }
        public string BeforeFrequency { get; set; }

        public IEnumerable<Example> Failures()
        {
            return Examples.Where(e => e.Exception != null);
        }
    }
}