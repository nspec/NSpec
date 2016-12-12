using NSpec;
using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSpecSpecs
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
            : base()
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
    }
}
