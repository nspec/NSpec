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
        public void should_only_include_direct_private_methods()
        {
            var methodInfos = typeof(child).Methods();

            methodInfos.Select(m => m.Name).should_be(new[] { "private_child_method" });
        }

        [Test]
        public void should_exclude_weird_methods()
        {
            var methodInfos = typeof(child).Methods();

            methodInfos.Count().should_be(1);
        }
    }
}