using System;
using System.Linq;
using Mono.Cecil;
using NUnit.Framework;

namespace NSpecSpecs
{
    [TestFixture]
    public class describe_cecil
    {
        [Test, Ignore]
        public void it_reflects_methods()
        {
            var def = AssemblyDefinition.ReadAssembly(@"C:\Projects\NSpec\NSpecSpecs\bin\Debug\SampleSpecs.dll");

            var before = def.MainModule.Types.First(t => t.Name == "describe_before");

            before.Methods.ToList().ForEach(Console.WriteLine);
        }
    }
}