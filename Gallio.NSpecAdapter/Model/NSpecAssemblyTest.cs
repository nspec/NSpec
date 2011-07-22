using System;
using System.Collections.Generic;
using Gallio.Common.Reflection;

namespace NSpec.GallioAdapter.Model
{
    public class NSpecAssemblyTest : NSpecTest
    {
        public string AssemblyFilePath
        {
            get { return ( (IAssemblyInfo)CodeElement ).Path; }
        }

        public Version FrameworkVersion
        {
            get { return _frameworkVersion; }
        }

        //public IList<IAssemblyContext> AssemblyContexts { get; set; }

        public NSpecAssemblyTest( string name, ICodeElementInfo codeElement, Version frameworkVersion ) 
            : base( name, codeElement )
        {
            _frameworkVersion = frameworkVersion;
        }

        readonly Version _frameworkVersion;
    }
}