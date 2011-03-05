using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using N = NSpec;
using I = NSpec.Interpreter.Indexer;    
using D = NSpec.Domain;
using NSpec.Extensions;

namespace NSpecNUnit.SpecFinder
{
    [TestFixture]
    public class when_finding_root_context
    {
        [Test]
        public void should_be_class_that_inherits_directly_from_spec_as_root_context()
        {
            GivenSpecFinderForTypes(typeof(sample_spec));
            TheRootContextFor<sample_spec>().Name.should_be("given sample_spec");
        }

        [Test]
        public void should_be_root_class_that_inherits_directly_from_spec()
        {
            GivenSpecFinderForTypes(typeof(sample_spec), typeof(child_sample_spec));
            TheRootContextFor<child_sample_spec>().Name.should_be("given sample_spec");
        }

        [Test]
        public void derived_spec_class_should_be_contained_in_root_context_as_child()
        {
            GivenSpecFinderForTypes(typeof(sample_spec), typeof(child_sample_spec));
            var root = TheRootContextFor<child_sample_spec>();

            root.Contexts.First().Name.should_be("given child_sample_spec");
        }

        [Test]
        public void should_capture_before_each_private_member_on_spec_class()
        {
            GivenSpecFinderForTypes(typeof(sample_spec));

            TheRootContextFor<sample_spec>().Before.should_not_be_null();
        }

        [Test]
        public void should_capture_before_each_private_member_on_derived_class()
        {
            GivenSpecFinderForTypes(typeof(sample_spec), typeof(child_sample_spec));
            var root = TheRootContextFor<child_sample_spec>();

            root.Contexts.First().Before.should_not_be_null();
        }

        private N.SpecFinder _specFinder;
        private N.SpecFinder GivenSpecFinderForTypes(params Type[] args)
        {
            _specFinder = new N.SpecFinder(args);
            return _specFinder;
        }

        private N.Domain.Context TheRootContextFor<T>() where T : I.spec, new()
        {
            T spec = new T();
            return _specFinder.ConstructRootContext(spec, typeof(T), new D.Context(spec.GetType().Name));
        }
    }

    public class sample_spec : I.spec
    {
        I.before<dynamic> each = (d) =>
        {

        };
    }

    public class child_sample_spec : sample_spec
    {
        I.before<dynamic> each = (d) =>
        {

        };
    }
}
