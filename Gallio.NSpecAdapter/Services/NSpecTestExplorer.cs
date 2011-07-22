using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gallio.Common.Reflection;
using Gallio.Model;
using Gallio.Model.Helpers;
using Gallio.Model.Tree;
using NSpec.GallioAdapter.Model;
using NSpec;
using NSpec.Domain;
using NSpec.Domain.Extensions;
using Reflector = Gallio.Common.Reflection.Reflector;

namespace NSpec.GallioAdapter.Services
{
    class NSpecTestExplorer : TestExplorer
    {
        protected override void ExploreImpl( IReflectionPolicy reflectionPolicy, ICodeElementInfo codeElement )
        {
            IAssemblyInfo assembly = ReflectionUtils.GetAssembly( codeElement );
            if( assembly != null )
            {
                Version frameworkVersion = GetFrameworkVersion( assembly );
                if( frameworkVersion != null )
                {
                    ITypeInfo type = ReflectionUtils.GetType( codeElement );

                    Test assemblyTest = GetAssemblyTest( assembly, TestModel.RootTest, frameworkVersion, type == null );
                }
            }
        }

        Test GetAssemblyTest( IAssemblyInfo assembly, Test parentTest, Version frameworkVersion, bool populateRecursively )
        {
            NSpecAssemblyTest assemblyTest;

            if( !assemblyTests.TryGetValue( assembly, out assemblyTest ) )
            {
                assemblyTest = new NSpecAssemblyTest( assembly.Name, assembly, frameworkVersion );
                assemblyTest.Kind = TestKinds.Assembly;

                ModelUtils.PopulateMetadataFromAssembly( assembly, assemblyTest.Metadata );

                string frameworkName = String.Format( "NSpec v{0}", frameworkVersion );
                assemblyTest.Metadata.SetValue( MetadataKeys.Framework, frameworkName );
                assemblyTest.Metadata.SetValue( MetadataKeys.File, assembly.Path );
                assemblyTest.Kind = TestKinds.Assembly;

                parentTest.AddChild( assemblyTest );
                assemblyTests.Add( assembly, assemblyTest );
            }

            if( populateRecursively )
            {
                Assembly resolvedAssembly = assembly.Resolve( false );
                var finder = new SpecFinder( resolvedAssembly, new NSpec.Domain.Reflector() );
                var builder = new ContextBuilder( finder, new DefaultConventions() );

                builder.Contexts()
                    .Select( context => this.GetContextTest( context ) )
                    .Each( test => assemblyTest.AddChild( test ) );
            }

            return assemblyTest;
        }

        NSpecContextTest GetContextTest( Context context )
        {
            NSpecContextTest contextTest = new NSpecContextTest( context );

            context.AllExamples()
                .Select( spec => this.GetSpecificationTest( context, spec ) )
                .Each( test => contextTest.AddChild( test ) );

            AddXmlComment( contextTest, Reflector.Wrap( context.GetType() ) );
            return contextTest;
        }

        NSpecExampleTest GetSpecificationTest( Context context, Example example )
        {
            NSpecExampleTest specificationTest = new NSpecExampleTest( example );

            if( example.Pending )
            {
                string reason = "This example is pending";
                specificationTest.Metadata.Add( MetadataKeys.IgnoreReason, reason );
            }

            AddXmlComment( specificationTest, Reflector.Wrap( example.MethodLevelExample ) );

            return specificationTest;
        }

        void AddXmlComment( Test test, ICodeElementInfo element )
        {
            string xml = element.GetXmlDocumentation();
            if( !string.IsNullOrEmpty( xml ) )
            {
                test.Metadata.Add( MetadataKeys.XmlDocumentation, xml );
            }
        }  

        Version GetFrameworkVersion( IAssemblyInfo assembly )
        {
            AssemblyName frameworkAssemblyName = ReflectionUtils.FindAssemblyReference(assembly, ASSEMBLY_DISPLAY_NAME);
            return frameworkAssemblyName != null ? frameworkAssemblyName.Version : null;
        }

        private const string ASSEMBLY_DISPLAY_NAME = @"NSpec";

        readonly Dictionary<IAssemblyInfo, NSpecAssemblyTest> assemblyTests = 
            new Dictionary<IAssemblyInfo, NSpecAssemblyTest>();    
    }
}