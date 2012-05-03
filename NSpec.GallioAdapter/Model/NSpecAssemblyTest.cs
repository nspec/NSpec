using System;
using Gallio.Common.Reflection;
using Gallio.Model.Tree;

namespace NSpec.GallioAdapter.Model
{
    public class NSpecAssemblyTest : Test
    {
        public string AssemblyFilePath
        {
            get { return ( (IAssemblyInfo)CodeElement ).Path; }
        }

        public Version FrameworkVersion
        {
            get { return _frameworkVersion; }
        }

        public NSpecAssemblyTest( string name, ICodeElementInfo codeElement, Version frameworkVersion ) 
            : base( name, codeElement )
        {
            _frameworkVersion = frameworkVersion;
        }

        readonly Version _frameworkVersion;
    }
}