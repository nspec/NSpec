using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NSpec.Domain
{
    public abstract class ExampleBase
    {
        public static string Parse(Expression expressionBody)
        {
            const string parensPattern = @"\((.*?)\)";
            const string otherSeparatorsPattern = @"[._""]";
            const string commmasPattern = @"\s+,";
            const string multiSpacesPattern = @"\s{2,}";

            string body = expressionBody.ToString();
            string sentence = body;

            // allow for 3 levels of nested parenthesis
            for (int i = 0; i < 3; i++)
            {
                sentence = Regex.Replace(sentence, parensPattern, @"$1");
            }

            sentence = Regex.Replace(sentence, otherSeparatorsPattern, " ");
            sentence = Regex.Replace(sentence, commmasPattern, ",");
            sentence = Regex.Replace(sentence, multiSpacesPattern, " ");

            return sentence.Trim();
        }

        public static string Parse(Expression<Action> exp)
        {
            return Parse(exp.Body);
        }

        public void AddTo(Context context)
        {
            Context = context;

            Tags.AddRange(context.Tags);

            Pending |= context.IsPending();
        }

        public abstract void Run(nspec nspec);
        public abstract void RunPending(nspec nspec);

        public abstract bool IsAsync { get; }
        public abstract MethodInfo BodyMethodInfo { get; }

        public string FullName()
        {
            return Context.FullContext() + ". " + Spec + ".";
        }

        public bool Failed()
        {
            return Exception != null;
        }

        public Stopwatch StartTiming()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            return stopWatch;
        }

        public void StopTiming(Stopwatch stopWatch)
        {
            stopWatch.Stop();

            Duration = stopWatch.Elapsed;
        }

        public bool ShouldSkip(Tags tagsFilter)
        {
            return tagsFilter.ShouldSkip(Tags);
        }

        public bool ShouldNotSkip(Tags tagsFilter)
        {
            return !tagsFilter.ShouldSkip(Tags);
        }

        public override string ToString()
        {
            string pendingPrefix = (Pending ? "(x)" : String.Empty);

            string exceptionText = (Exception != null ? ", " + Exception.GetType().Name : String.Empty);

            return String.Format("{0}{1}{2}", pendingPrefix, Spec, exceptionText);
        }

        public ExampleBase(string name = "", string tags = "", bool pending = false)
        {
            Spec = name;

            Tags = Domain.Tags.ParseTags(tags);

            Pending = pending;
        }

        public TimeSpan Duration { get; protected set; }
        public string CapturedOutput { get; set; }
        public bool Pending { get; protected set; }
        public bool HasRun;
        public string Spec;
        public List<string> Tags;
        public Exception Exception;
        public Context Context;
    }
}