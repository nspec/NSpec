using System;
using Gallio.Model;
using Gallio.Model.Commands;
using Gallio.Model.Contexts;
using Gallio.Model.Helpers;
using Gallio.Model.Tree;
using Gallio.Runtime.ProgressMonitoring;

namespace NSpec.GallioAdapter.Services
{
    public class NSpecController : TestController
    {
        //ISpecificationRunListener _listener;
        //RunOptions _options;
        IProgressMonitor _progressMonitor;

        protected override TestResult RunImpl( ITestCommand rootTestCommand, TestStep parentTestStep, TestExecutionOptions options, IProgressMonitor progressMonitor )
        {
            progressMonitor.BeginTask( "Verifying Specifications", rootTestCommand.TestCount );

            if( options.SkipTestExecution )
            {
                return SkipAll( rootTestCommand, parentTestStep );
            }
            else
            {
                ITestContext rootContext = rootTestCommand.StartPrimaryChildStep( parentTestStep );
                TestStep rootStep = rootContext.TestStep;
                TestOutcome outcome = TestOutcome.Passed;

                //_progressMonitor = progressMonitor;
                //SetupRunOptions( options );
                //SetupListeners( options );

                //_listener.OnRunStart();

                //foreach( ITestCommand command in rootTestCommand.Children )
                //{
                //    MachineAssemblyTest assemblyTest = command.Test as MachineAssemblyTest;
                //    if( assemblyTest == null )
                //        continue;

                //    var assemblyResult = RunAssembly( assemblyTest, command, rootStep );
                //    outcome = outcome.CombineWith( assemblyResult.Outcome );
                //}

                //_listener.OnRunEnd();

                return rootContext.FinishStep( outcome, null );
            }
        }
    }
}