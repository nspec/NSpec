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

        public virtual void Run(nspec nspec)
        {
            action();
        }

        public string FullName()
        {
            return Context.FullContext() + ". " + Spec + ".";
        }

        public bool Failed()
        {
            return Exception != null;
        }

        public void AssignProperException(Exception contextException)
        {
            if (contextException == null) return; //stick with whatever Exception may or may not be set on this Example

            if (Exception != null && Exception.GetType() != typeof(ExceptionNotThrown))
                Exception = new ExampleFailureException("Context Failure: " + contextException.Message + ", Example Failure: " + Exception.Message, contextException);

            if (Exception == null)
                Exception = new ExampleFailureException("Context Failure: " + contextException.Message, contextException);
        }

        public Example(Expression<Action> expr) : this(Parse(expr), null, expr.Compile()) { }

        public Example(string name, string tags)
        {
            Spec = name;

            Tags = Domain.Tags.ParseTags(tags);
        }

        public Example(string name, string tags, Action action, bool pending = false)
            : this(name, tags)
        {
            this.action = action;

            Pending = pending;
        }

        public bool Pending;
        public bool HasRun;
        public string Spec;
        public List<string> Tags;
        public Exception Exception;
        public Context Context;

        Action action;

        public bool ShouldSkip(Tags tagsFilter)
        {
            return tagsFilter.ShouldSkip(Tags) || ((HasRun = true) && Pending);
        }
    }
}