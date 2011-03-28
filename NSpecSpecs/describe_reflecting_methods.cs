using System.Linq;
using NSpec;
using NSpec.Domain.Extensions;
using NUnit.Framework;

namespace NSpecSpecs
{
    [TestFixture]
    public class describe_reflecting_methods
    {
        class parent : nspec
        {
            void parent_method() { }
        }

        class child : nspec
        {
            public void public_child_method() { }
            void private_child_method() { }
            void helper_method_with_paramter(int i) { }
        }

        [Test]
        public void should_include_direct_private_methods()
        {
            ShouldContain("private_child_method");
        }

        [Test]
        public void should_include_direct_public_methods()
        {
            ShouldContain("public_child_method");
        }

        public void ShouldContain(string name)
        {
            var methodInfos = typeof(child).Methods();

            methodInfos.Any(m => m.Name == name).should_be(true);
        }
    }
}