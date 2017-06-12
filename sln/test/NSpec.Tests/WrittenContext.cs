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
            BeforeAllException = context.BeforeAllChain.Exception;
            AnyBeforeAllException = context.BeforeAllChain.AnyException();
            BeforeException = context.BeforeChain.Exception;
            AnyBeforeException = context.BeforeChain.AnyException();
            ActException = context.ActChain.Exception;
            AnyActException = context.ActChain.AnyException();
            AfterException = context.AfterChain.Exception;
            AnyAfterException = context.AfterChain.AnyException();
            AfterAllException = context.AfterAllChain.Exception;
            AnyAfterAllException = context.AfterAllChain.AnyException();
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

        public Exception BeforeAllException { get; private set; }
        public Exception AnyBeforeAllException { get; private set; }

        public Exception BeforeException { get; private set; }
        public Exception AnyBeforeException { get; private set; }

        public Exception ActException { get; private set; }
        public Exception AnyActException { get; private set; }

        public Exception AfterException { get; private set; }
        public Exception AnyAfterException { get; private set; }

        public Exception AfterAllException { get; private set; }
        public Exception AnyAfterAllException { get; private set; }

        public bool ClearExpectedException { get; private set; }

        public string CapturedOutput { get; private set; }

        public bool IsPending { get; private set; }

        public string FullContext { get; private set; }

        public bool HasAnyFailures { get; private set; }

        public bool HasAnyExecutedExample { get; private set; }
    }
}
