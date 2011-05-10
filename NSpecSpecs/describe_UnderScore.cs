using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec.Domain;
using NSpec;

namespace NSpecSpecs
{
    public class describe_UnderScore
    {
        protected Conventions underScore;

        [SetUp]
        public void setup_base()
        {
            underScore = new UnderScore();

            underScore.Initialize();
        }
    }

    [TestFixture]
    [Category("UnderScore")]
    public class when_determining_if_a_method_name_is_a_before_context : describe_UnderScore
    {
        [Test]
        public void it_should_match_for_methods_that_start_with_the_words_before_each()
        {
            underScore.IsMethodLevelBefore("before_each").should_be_true();
        }
    }

    [TestFixture]
    [Category("UnderScore")]
    public class when_determining_if_a_method_name_is_a_act_context : describe_UnderScore
    {
        [Test]
        public void it_should_match_for_methods_that_start_with_the_words_act_each()
        {
            underScore.IsMethodLevelAct("act_each").should_be_true();
        }
    }

    [TestFixture]
    [Category("UnderScore")]
    public class when_determining_if_a_method_name_is_a_method_level_expample : describe_UnderScore
    {
        [Test]
        public void it_should_match_for_methods_that_start_with_the_words_it()
        {
            underScore.IsMethodLevelExample("it_should_be_true").should_be_true();
        }

        [Test]
        public void it_should_match_for_methods_that_start_with_the_words_specify()
        {
            underScore.IsMethodLevelExample("specify_should_be_true").should_be_true();
        }
    }

    [TestFixture]
    [Category("UnderScore")]
    public class when_determining_if_a_method_name_is_a_context : describe_UnderScore
    {
        [Test]
        public void it_should_be_match_if_method_contains_an_underscore_and_is_not_a_method_level_example_an_act_or_a_before()
        {
            underScore.IsMethodLevelContext("describe_a_specification").should_be_true();
        }
    }
}
