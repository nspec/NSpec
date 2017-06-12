using System;
using NSpec.Domain.Formatters;

namespace NSpec.Domain
{
    public class RunnableExample
    {
        public void Exercise(nspec instance)
        {
            if (example.ShouldSkip(instance.tagsFilter))
            {
                return;
            }

            example.HasRun = true;

            if (example.Pending)
            {
                ContextUtils.RunAndHandleException(example.RunPending, instance, ref example.Exception);

                return;
            }

            var stopWatch = example.StartTiming();

            using (new ConsoleCatcher(output => example.CapturedOutput = output))
            {
                context.BeforeChain.Run(instance);

                context.ActChain.Run(instance);

                if (CanRun())
                {
                    ContextUtils.RunAndHandleException(example.Run, instance, ref example.Exception);
                }

                context.AfterChain.Run(instance);
            }

            // when an expected exception is thrown and is set to be cleared by 'expect<>',
            // a subsequent exception thrown in 'after' hooks would go unnoticed, so do not clear in this case

            if (context.AfterChain.AnyThrew()) context.ClearExpectedException = false;

            example.StopTiming(stopWatch);
        }

        bool CanRun()
        {
            return
                !context.BeforeAllChain.AnyThrew() &&
                !context.BeforeChain.AnyThrew();
        }

        public void AssignException(Exception beforeAllException, Exception afterAllException)
        {
            if (example.Pending) return;

            // if an exception was thrown before the example (only in `act`) but was expected, ignore it
            Exception unexpectedException = context.ClearExpectedException
                ? null
                : context.ActChain.AnyException();

            Exception previousException =
                beforeAllException ??
                context.BeforeChain.AnyException() ??
                unexpectedException;

            Exception followingException =
                context.AfterChain.AnyException() ??
                afterAllException;

            if (previousException == null && followingException == null)
            {
                // stick with whatever exception may or may not be set on this example
                return;
            }

            if (previousException != null)
            {
                if (example.Exception != null && example.Exception.GetType() != typeof(ExceptionNotThrown))
                {
                    example.Exception = ExampleFailureException
                        .FromContextAndExample(previousException, example.Exception);

                    return;
                }

                if (example.Exception == null)
                {
                    example.Exception = ExampleFailureException
                        .FromContext(previousException);

                    return;
                }
            }

            if (example.Exception == null)
            {
                example.Exception = ExampleFailureException.FromContext(followingException);
            }
        }

        public void Write(ILiveFormatter formatter)
        {
            if (example.HasRun)
            {
                context.WriteAncestors(formatter);

                formatter.Write(example, context.Level);
            }
        }

        public RunnableExample(Context context, ExampleBase example)
        {
            this.context = context;
            this.example = example;
        }

        readonly Context context;
        readonly ExampleBase example;
    }
}
