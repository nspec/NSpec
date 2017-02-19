using NSpec.Domain;
using System;
using System.Reflection;

namespace NSpec.Tests
{
    /// <summary>
    /// Abstract framework ExampleBase class implements functionalities common to both sync and async example implementations.
    /// This thin wrapper allows to test those functionalities.
    /// </summary>
    class ExampleBaseWrap : ExampleBase
    {
        public ExampleBaseWrap(string name)
            : base(name)
        {
        }

        public ExampleBaseWrap()
        {
        }

        public override void Run(nspec nspec)
        {
            throw new NotImplementedException();
        }

        public override void RunPending(nspec nspec)
        {
            throw new NotImplementedException();
        }

        public override bool IsAsync
        {
            get { throw new NotImplementedException(); }
        }

        public override MethodInfo BodyMethodInfo
        {
            get { throw new NotImplementedException(); }
        }
    }
}
