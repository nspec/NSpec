using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpec.Tests
{
    public class WrittenContext
    {
        public WrittenContext(Context context)
        {
            Name = context.Name;
            Level = context.Level;
            Tags = new List<string>(context.Tags);
            ExceptionBeforeAll = context.ExceptionBeforeAll;
            ExceptionBeforeAct = context.ExceptionBeforeAct;
            ExceptionAfter = context.ExceptionAfter;
            ExceptionAfterAll = context.ExceptionAfterAll;
            ClearExpectedException = context.ClearExpectedException;
            CapturedOutput = context.CapturedOutput;
            IsPending = context.IsPending();
            FullContext = context.FullContext();
            HasAnyFailures = context.HasAnyFailures();
            HasAnyExecutedExample = context.HasAnyExecutedExample();
        }

        public string Name { get; private set; }

        public int Level { get; private set; }

        public List<string> Tags { get; private set; }

        public Exception ExceptionBeforeAll { get; private set; }

        public Exception ExceptionBeforeAct { get; private set; }

        public Exception ExceptionAfter { get; private set; }

        public Exception ExceptionAfterAll { get; private set; }

        public bool ClearExpectedException { get; private set; }

        public string CapturedOutput { get; private set; }

        public bool IsPending { get; private set; }

        public string FullContext { get; private set; }

        public bool HasAnyFailures { get; private set; }

        public bool HasAnyExecutedExample { get; private set; }
    }
}
