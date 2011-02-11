using System;
using System.Collections.Generic;
using System.Linq;

namespace NSpec
{
    public class Context
    {
        public string Name { get; set; }
        public int Level { get; set; }

        public Context(string name) :this(name,0, "given")
        {

        }

        public Context(string name, int level, string prefix)
        {
            Name = "{0} {1}".With(prefix,name);
            Level = level;
            Examples = new List<Example>();
            Contexts = new List<Context>();
        }

        public void AddExample(Example example)
        {
            Examples.Add(example);
        }

        public override string ToString()
        {
            if (Examples.Count == 0) return "";

            var context = string.Format("\t".Times(Level) + "{0}", Name);

            Examples.Do(e => context += Environment.NewLine + "\t".Times(Level) + e );

            Contexts.Do(c => context += Environment.NewLine + c.ToString());

            return context;
        }

        public List<Example> Examples { get; set; }
        public List<Context> Contexts { get; set; }

        public Action Before { get; set; }

        public Context Parent { get; set; }

        public void Befores()
        {
            if (Parent != null && Parent.Before != null)
                Parent.Before();

            if (Before != null)
                Before();
        }

        public IEnumerable<Example> AllExamples()
        {
            return Contexts.SelectMany(c => c.AllExamples()).Union(Examples);
        }
    }
}