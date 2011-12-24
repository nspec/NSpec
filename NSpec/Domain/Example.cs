using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NSpec.Domain
{
    public class Example
    {
        public static string Parse(Expression expressionBody)
        {
            var body = expressionBody.ToString();

            var cut = body.IndexOf(").");

            var sentence = body.Substring(cut + 1, body.Length - cut - 1).Replace(")", " ").Replace(".", " ").Replace("(", " ").Replace("  ", " ").Trim().Replace("_", " ").Replace("\"", " ");

            while (sentence.Contains("  ")) sentence = sentence.Replace("  ", " ");

            return sentence.Trim();
        }

        public static string Parse(Expression<Action> exp)
        {
            return Parse(exp.Body);
        }

        public void Run(nspec nspec)
        {
            if (MethodLevelExample != null) MethodLevelExample.Invoke(nspec, null);

            else action();
        }

        public string FullName()
        {
            return Context.FullContext() + ". " + Spec + ".";
        }

        public bool Passed
        {
            get { return (HasRun && ExampleLevelException == null); }
        }

        public Example(Expression<Action> expr) : this(Parse(expr), null, expr.Compile()) { }

        public Example(string name = "", string tags = null, Action action = null, bool pending = false)
        {
            this.action = action;

            Spec = name;

            Pending = pending;

            Tags = NSpec.Domain.Tags.ParseTags(tags);
        }

        public Example(MethodInfo methodLevelExample, string tags = null)
        {
            Spec = methodLevelExample.Name.Replace("_", " ");

            MethodLevelExample = methodLevelExample;

            Tags = NSpec.Domain.Tags.ParseTags(tags);
        }

        public bool Pending;

        public bool Failed()
        {
            return ExampleLevelException != null;
        }

        public bool HasRun;
        public string Spec;
        public List<string> Tags;
        public Exception ExampleLevelException;
        public Context Context;
        public MethodInfo MethodLevelExample;

        Action action;
    }
}