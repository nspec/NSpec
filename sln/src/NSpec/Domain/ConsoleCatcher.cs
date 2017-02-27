using System;
using System.IO;

namespace NSpec.Domain
{
    public class ConsoleCatcher : IDisposable
    {
        public ConsoleCatcher(Action<string> setOutput)
        {
            this.setOutput = setOutput;

            stringWriter = new StringWriter();

            lock (padlock)
            {
                stdout = Console.Out;
                stderr = Console.Error;

                Console.SetOut(stringWriter);
                Console.SetError(stringWriter);
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                lock (padlock)
                {
                    Console.SetOut(stdout);
                    Console.SetError(stderr);
                }

                setOutput(stringWriter.ToString());

                stringWriter.Dispose();

                disposed = true;
            }
        }

        bool disposed;

        readonly Action<string> setOutput;
        readonly StringWriter stringWriter;
        readonly TextWriter stdout;
        readonly TextWriter stderr;

        static readonly object padlock = new object();
    }
}
