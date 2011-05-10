using NUnit.Framework;
using NSpec.Domain;
using NSpec;

namespace NSpecSpecs
{
    public class describe_DefaultConvention
    {
        protected Conventions underScore;

        [SetUp]
        public void setup_base()
        {
            underScore = new DefaultConvention();

            underScore.Initialize();
        }
    }

    [TestFixture]
    [Category("DefaultConvention")]
    public class when_determining_before_methods : describe_DefaultConvention
    {
        [Test]
        public void should_match_before_each()
        {
            ShouldBeBefore("before_each");
        }

        [Test]
        public void should_match_BeforeEach()
        {
            ShouldBeBefore("BeforeEach");
        }

        void ShouldBeBefore(string methodName)
        {
            underScore.IsMethodLevelBefore(methodName).should_be_true();

            underScore.IsMethodLevelContext(methodName).should_be_false();
        }
    }

    [TestFixture]
    [Category("DefaultConvention")]
    public class when_determining_act_methods : describe_DefaultConvention
    {
        [Test]
        public void should_match_act_each()
        {
            ShouldBeAct("act_each");
        }

        [Test]
        public void should_match_ActEach()
        {
            ShouldBeAct("ActEach");
        }

        void ShouldBeAct(string methodName)
        {
            underScore.IsMethodLevelAct(methodName).should_be_true();

            underScore.IsMethodLevelContext(methodName).should_be_false();
        }
    }

    [TestFixture]
    [Category("DefaultConvention")]
    public class when_determining_example_methods : describe_DefaultConvention
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
        public void should_match_ItShouldBeTrue()
        {
            ShouldBeExample("ItShouldBeTrue");
        }

        [Test]
        public void should_match_SpecifyShouldBeTrue()
        {
            ShouldBeExample("SpecifyShouldBeTrue");
        }

        [Test]
        public void should_not_match_IterationShould()
        {
            underScore.IsMethodLevelExample("IterationShould").should_be_false();
        }

        void ShouldBeExample(string methodName)
        {
            underScore.IsMethodLevelExample(methodName).should_be_true();

            underScore.IsMethodLevelContext(methodName).should_be_false();
        }
    }

    [TestFixture]
    [Category("DefaultConvention")]
    public class when_determining_context_methods : describe_DefaultConvention
    {
        [Test]
        public void should_be_match_describe_a_specification()
        {
            underScore.IsMethodLevelContext("describe_a_specification").should_be_true();
        }

        [Test]
        public void should_be_match_DescribeASpecification()
        {
            underScore.IsMethodLevelContext("DescribeASpecification").should_be_true();
        }
    }
}
