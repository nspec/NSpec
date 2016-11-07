using FluentAssertions;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace NSpecSpecs
{
    public class describe_DefaultConventions
    {
        protected Conventions defaultConvention;

        [SetUp]
        public void setup_base()
        {
            defaultConvention = new DefaultConventions();

            defaultConvention.Initialize();
        }
    }

    [TestFixture]
    [Category("DefaultConvention")]
    public class when_determining_before_methods : describe_DefaultConventions
    {
        [Test]
        public void should_match_before_each()
        {
            ShouldBeBefore("before_each");
        }

        [Test]
        public void should_ignore_case()
        {
            ShouldBeBefore("Before_Each");
        }

        void ShouldBeBefore(string methodName)
        {
            defaultConvention.IsMethodLevelBefore(methodName).Should().BeTrue();

            defaultConvention.IsMethodLevelContext(methodName).Should().BeFalse();
        }
    }

    [TestFixture]
    [Category("DefaultConvention")]
    public class when_determining_act_methods : describe_DefaultConventions
    {
        [Test]
        public void should_match_act_each()
        {
            ShouldBeAct("act_each");
        }

        [Test]
        public void should_ignore_case()
        {
            ShouldBeAct("Act_Each");
        }

        void ShouldBeAct(string methodName)
        {
            defaultConvention.IsMethodLevelAct(methodName).Should().BeTrue();

            defaultConvention.IsMethodLevelContext(methodName).Should().BeFalse();
        }
    }

    [TestFixture]
    [Category("DefaultConvention")]
    public class when_determining_example_methods : describe_DefaultConventions
    {
        [Test]
        public void should_match_it()
        {
            ShouldBeExample("it_should_be_true");
        }

        [Test]
        public void should_match_specify()
        {
            ShouldBeExample("specify_should_be_true");
        }

        [Test]
        public void should_ignore_case_when_matching_it()
        {
            ShouldBeExample("It_ShouldBe_True");
        }

        [Test]
        public void should_ignore_case_when_matching_specify()
        {
            ShouldBeExample("Specify_ShouldBe_True");
        }

        [Test]
        public void should_not_match_IterationShould()
        {
            defaultConvention.IsMethodLevelExample("IterationShould").Should().BeFalse();
        }

        void ShouldBeExample(string methodName)
        {
            defaultConvention.IsMethodLevelExample(methodName).Should().BeTrue();

            defaultConvention.IsMethodLevelContext(methodName).Should().BeFalse();
        }
    }

    [TestFixture]
    [Category("DefaultConvention")]
    public class when_determining_context_methods : describe_DefaultConventions
    {
        [Test]
        public void should_be_match_describe_a_specification()
        {
            defaultConvention.IsMethodLevelContext("describe_a_specification").Should().BeTrue();
        }

        [Test]
        public void should_ignore_case()
        {
            defaultConvention.IsMethodLevelContext("Describe_A_Specification").Should().BeTrue();
        }

        [Test]
        public void should_not_match_methods_dont_contain_underscores()
        {
            defaultConvention.IsMethodLevelContext("GivenUser").Should().BeFalse();
        }
    }

}
