using System;

namespace NSpec.Api.Execution
{
    public class ExecutedExample
    {
        public string FullName { get; set; }

        public bool Pending { get; set; }

        public bool Failed { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionStackTrace { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
