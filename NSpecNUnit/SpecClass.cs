using System;
using NSpec.Interpreter.Indexer;

namespace NSpecNUnit
{
    public class SpecClass : spec
    {
        public void public_method() { }
        private void private_method() { }
    }
    public class NonSpecClass{}
    public class SpecClassWithNoPublicMethods : spec 
    {
        private void private_method() { }
    }
}