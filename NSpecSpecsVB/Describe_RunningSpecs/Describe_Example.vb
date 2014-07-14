Imports NSpec
Imports NUnit.Framework
Imports NSpecSpecs.WhenRunningSpecs

Namespace WhenRunningSpecs

    <TestFixture()>
    <Category("RunningSpecs")>
    Public Class DescribeExample
        Inherits when_running_specs

        Public Class SpecClass
            Inherits NSpec.nspec

            Sub It_Changes_Status_After_Run()

            End Sub

            Sub It_Passes()

            End Sub

            Sub It_Fails()
                Throw New Exception()
            End Sub
        End Class

        <Test()>
        Sub Execution_Status_Changes_After_Run()
            Run(GetType(SpecClass))

            Dim ex = TheExample("It Changes Status After Run")

            ex.HasRun.should_be_true()
        End Sub
    End Class

End Namespace

