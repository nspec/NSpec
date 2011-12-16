using System;
using System.Collections.Generic;
using System.Linq;
using NSpec;
using NSpec.Domain.Extensions;
using NUnit.Framework;

namespace NSpecSpecs
{
    [TestFixture]
    [Category("DomainExtensions")]
    public class describe_DomainExtensions
    {
        abstract class indirectAbstractAncestor : nspec
        {
            void indirect_ancestor_method() { }
        }

        class concreteAncestor : indirectAbstractAncestor
        {
            void concrete_ancestor_method() { }
        }

        abstract class abstractAncestor : concreteAncestor
        {
            void ancestor_method() { }
        }

        abstract class abstractParent : abstractAncestor
        {
            void parent_method() { }
        }

        class child : abstractParent
        {
            public void public_child_method() { }
            void private_child_method() { }
            void helper_method_with_paramter(int i) { }
            void NoUnderscores() { }
        }

        class grandChild : child
        {
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

        [Test]
        public void should_disregard_methods_with_out_underscores()
        {
            ShouldNotContain("NoUnderscores", typeof(child));
        }

        [Test]
        public void should_include_methods_from_abstract_parent()
        {
            ShouldContain("parent_method");
        }

        [Test]
        public void should_include_methods_from_direct_abstract_ancestor()
        {
            ShouldContain("ancestor_method");
        }

        [Test]
        public void should_disregard_methods_from_concrete_ancestor()
        {
            ShouldNotContain("concrete_ancestor_method", typeof(child));
        }

        [Test]
        public void should_disregard_methods_from_indirect_abstract_ancestor()
        {
            ShouldNotContain("indirect_ancestor_method", typeof(child));
        }

        [Test]
        public void should_disregard_methods_from_concrete_parent()
        {
            ShouldNotContain("private_child_method", typeof(grandChild));
        }

        public void ShouldContain(string name)
        {
            var methodInfos = typeof(child).Methods();

            methodInfos.Any(m => m.Name == name).should_be(true);
        }

        public void ShouldNotContain(string name, Type type)
        {
            var methodInfos = type.Methods();

            methodInfos.Any(m => m.Name == name).should_be(false);
        }

        class Foo1{}

        abstract class Foo2 : Foo1{}

        class Foo3 : Foo2{}

        abstract class Foo4 : Foo3{}

        abstract class Foo5 : Foo4{}

        class Foo6 : Foo5{}

        [Test,
         TestCase(typeof(Foo1), new [] {typeof(Foo1)}),
         TestCase(typeof(Foo2), new [] {typeof(Foo2)}),
         TestCase(typeof(Foo3), new [] {typeof(Foo2), typeof(Foo3)}),
         TestCase(typeof(Foo4), new [] {typeof(Foo4)}),
         TestCase(typeof(Foo5), new [] {typeof(Foo4), typeof(Foo5)}),
         TestCase(typeof(Foo6), new [] {typeof(Foo4), typeof(Foo5), typeof(Foo6)})]
        public void should_build_immediate_abstract_class_chain(Type type, Type[] chain)
        {
            IEnumerable<Type> generatedChain = type.GetAbstractBaseClassChainWithClass();

            generatedChain.SequenceEqual(chain).should_be_true();
        }

        class Bar1{}

        class Bar11 : Bar1{}

        class Bar2<TBaz1>{}

        class Bar21 : Bar2<Bar1>{}

        class Bar3<TBaz1, TBaz2>{}

        class Bar31 : Bar3<Bar1, Bar1>{}

        class Bar32 : Bar3<Bar1, Bar2<Bar1>>{}

        [Test,
         TestCase(typeof(Bar11), "Bar1"),
         TestCase(typeof(Bar21), "Bar2< Bar1 >"),
         TestCase(typeof(Bar31), "Bar3< Bar1, Bar1 >"),
         TestCase(typeof(Bar32), "Bar3< Bar1, Bar2< Bar1 > >")]
        public void should_generate_pretty_type_names(Type derivedType, string expectedNameForBaseType)
        {
            string name = derivedType.BaseType.GetPrettyName();

            name.should_be(expectedNameForBaseType);
        }
    }
}
