Imports NSpec
Imports NUnit.Framework
Imports NSpecSpecs
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
        Public Sub Execution_Status_Changes_After_Run()
            Run(GetType(SpecClass))

            Dim ex = TheExample("It Changes Status After Run")

            ex.HasRun.should_be_true()
        End Sub

        <Test()>
        Public Sub Passing_Status_Is_Passed_When_It_Succeeds()
            Run(GetType(SpecClass))

            TheExample("It Passes").should_have_passed()
        End Sub

        <Test()>
        Public Sub Passing_Status_Is_Not_Passed_When_It_Fails()
            Run(GetType(SpecClass))

            TheExample("It Fails").should_have_failed()
        End Sub

        Public Class SpecClassWithAnonymousLambdas
            Inherits NSpec.nspec

            Sub Describe_Specs_In_VB_With_Anonymous_Lambdas()
                context("Some context with an anonymous Lambda") =
                    Sub()
                        it("has an anonymous lambda") = Sub()
                                                        End Sub
                    End Sub
            End Sub
        End Class

        <Test()>
        Public Sub Finds_And_Runs_Three_Class_Level_Examples()
            Run(GetType(SpecClass))

            TheExampleCount().should_be(3)
        End Sub

        <Test()>
        Public Sub Finds_Only_One_Example_Ignoring_Anonymous_Lambdas()
            Run(GetType(SpecClassWithAnonymousLambdas))

            TheExampleCount().should_be(1)
        End Sub

    End Class

End Namespace

