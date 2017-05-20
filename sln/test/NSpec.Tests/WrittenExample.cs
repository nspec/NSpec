using NSpec.Domain;
using System;
using System.Collections.Generic;

namespace NSpec.Tests
{
    public class WrittenExample
    {
        public WrittenExample(ExampleBase example)
        {
            Pending = example.Pending;
            HasRun = example.HasRun;
            Spec = example.Spec;
            Tags = new List<string>(example.Tags);
            Exception = example.Exception;
            Failed = example.Failed();
            FullName = example.FullName();
            IsAsync = example.IsAsync;
            Duration = example.Duration;
            CapturedOutput = example.CapturedOutput;
        }

        public bool Pending { get; private set; }

        public bool HasRun { get; private set; }

        public string Spec { get; private set; }

        public List<string> Tags { get; private set; }

        public Exception Exception { get; private set; }

        public bool Failed { get; private set; }

        public string FullName { get; private set; }

        public bool IsAsync { get; private set; }

        public TimeSpan Duration { get; private set; }

        public string CapturedOutput { get; private set; }
    }
}
