using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSpec.Domain;
using NSpec;

namespace NSpecSpecs
{
    [TestFixture]
    [Category("Conventions")]
    public class when_find_before
    {
        private Conventions convensions;

        public class class_with_before : nspec
        {
            void before_each()
            {
                
            }
        }

        [SetUp]
        public void Setup()
        {
            convensions = new DefaultConvention();
        }
    }

    [TestFixture]
    [Category("Conventions")]
    public class specifying_new_before_convension
    {
        private Conventions convensions;

        public class ClassWithBefore : nspec
        {
            void BeforeEach()
            {

            }
        }

        [SetUp]
        public void Setup()
        {

        }
    }
}
