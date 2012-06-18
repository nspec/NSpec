using System.Collections.Generic;

namespace NSpecSpecs.describe_RunningSpecs.after_all
{
    class DerivedClass : SpecClass
    {
        protected IList<string> log;

        void after_all()
        {
            log.Add("DerivedClass after_all");
        }

        void after_each()
        {
            log.Add("DerivedClass after_each");
        }

        void context_b()
        {
            beforeEach = () => log.Add("context_b beforeEach");
            it["1"] = () => log.Add("context_b it 1");
            it["2"] = () => log.Add("context_b it 2");
            afterEach = () => log.Add("context_b afterEach");
            afterAll = () => log.Add("context_b afterAll");
        }
    }

    public class describe_after_all_derived_classes_
    {
        
    }
}