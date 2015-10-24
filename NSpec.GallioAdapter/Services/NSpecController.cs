using System;
using Gallio.Common.Collections;
using Gallio.Framework;
using Gallio.Model;
using Gallio.Model.Commands;
using Gallio.Model.Contexts;
using Gallio.Model.Helpers;
using Gallio.Runtime.ProgressMonitoring;
using NSpec.GallioAdapter.Model;
using TestStep = Gallio.Model.Tree.TestStep;

namespace NSpec.GallioAdapter.Services
{
    public class NSpecController : TestController
    {
        protected override TestResult RunImpl(ITestCommand rootTestCommand, TestStep parentTestStep, TestExecutionOptions options, IProgressMonitor progressMonitor)
        {
            using (progressMonitor.BeginTask("Verifying Specifications", rootTestCommand.TestCount))
            {
                if (options.SkipTestExecution)
                {
                    return SkipAll(rootTestCommand, parentTestStep);
                }
                else
                {
                    ITestContext rootContext = rootTestCommand.StartPrimaryChildStep(parentTestStep);
                    TestStep rootStep = rootContext.TestStep;
                    TestOutcome outcome = TestOutcome.Passed;

                    foreach (ITestCommand command in rootTestCommand.Children)
                    {
                        NSpecAssemblyTest assemblyTest = command.Test as NSpecAssemblyTest;
                        if (assemblyTest == null)
                            continue;

                        var assemblyResult = this.RunAssembly(command, rootStep);
                        outcome = outcome.CombineWith(assemblyResult.Outcome);
                    }

                    return rootContext.FinishStep(outcome, null);
                }
            }
        }

        private TestResult RunAssembly(ITestCommand command, TestStep rootStep)
        {
            ITestContext assemblyContext = command.StartPrimaryChildStep(rootStep);

            TestOutcome outcome = TestOutcome.Passed;

            foreach (ITestCommand contextCommand in command.Children)
            {
                NSpecContextTest contextTest = contextCommand.Test as NSpecContextTest;
                if (contextTest == null)
                    continue;

                var contextResult = this.RunContext(contextTest, contextCommand, assemblyContext.TestStep);
                outcome = outcome.CombineWith(contextResult.Outcome);
                assemblyContext.SetInterimOutcome(outcome);
            }

            return assemblyContext.FinishStep(outcome, null);
        }

        private TestResult RunContext(NSpecContextTest contextTest, ITestCommand command, TestStep testStep)
        {
            ITestContext testContext = command.StartPrimaryChildStep(testStep);
            TestOutcome outcome = TestOutcome.Passed;

            foreach (ITestCommand testCommand in command.Children)
            {
                NSpecExampleTest exampleTest = testCommand.Test as NSpecExampleTest;
                if (exampleTest == null)
                {
                    continue;
                }
                outcome = outcome.CombineWith(this.RunTest(contextTest, exampleTest, testCommand, testContext.TestStep).Outcome);
            }
            foreach (ITestCommand testCommand in command.Children)
            {
                NSpecContextTest contextTestChild = testCommand.Test as NSpecContextTest;
                if (contextTestChild == null)
                {
                    continue;
                }
                outcome = outcome.CombineWith(this.RunContext(contextTestChild, testCommand, testContext.TestStep).Outcome);
            }

            return testContext.FinishStep(outcome, null);
        }

        TestResult RunTest(NSpecContextTest contextTest, NSpecExampleTest exampleTest,
            ITestCommand testCommand, TestStep testStep)
        {
            ITestContext testContext = testCommand.StartPrimaryChildStep(testStep);
            TestOutcome outcome = TestOutcome.Passed;

            if (exampleTest.Example.Pending)
            {
                outcome = TestOutcome.Pending;
                testContext.AddMetadata(MetadataKeys.PendingReason, "Needs to be implemented");
            }
            else
            {
                Exception inheritedException = null;

                contextTest.Context.Exercise(exampleTest.Example, inheritedException, contextTest.Context.GetInstance());

                if (exampleTest.Example.Exception != null)
                {
                    TestLog.Failures.WriteException(ConvertException(exampleTest.Example.Exception));
                    TestLog.Failures.Flush();

                    outcome = TestOutcome.Failed;
                }
            }

            return testContext.FinishStep(outcome, null);
        }

        Gallio.Common.Diagnostics.ExceptionData ConvertException(Exception exception)
        {
            if (exception == null)
                return null;

            Gallio.Common.Diagnostics.ExceptionData inner = this.ConvertException(exception.InnerException);
            return new Gallio.Common.Diagnostics.ExceptionData(exception.GetType().ToString(), exception.Message, exception.StackTrace ?? "", new PropertySet(), inner);
        }
    }
}