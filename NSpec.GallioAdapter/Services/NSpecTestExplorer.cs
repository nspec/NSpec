using System;
using System.Collections.Generic;
using System.Reflection;
using Gallio.Common.Reflection;
using Gallio.Model;
using Gallio.Model.Helpers;
using Gallio.Model.Tree;
using NSpec.GallioAdapter.Model;
using NSpec.Domain;

namespace NSpec.GallioAdapter.Services
{
    class NSpecTestExplorer : TestExplorer
    {
        public NSpecTestExplorer()
        {
            assemblyTests = new Dictionary<IAssemblyInfo, NSpecAssemblyTest>();
        }

        protected override void ExploreImpl(IReflectionPolicy reflectionPolicy, ICodeElementInfo codeElement)
        {
            IAssemblyInfo assembly = ReflectionUtils.GetAssembly(codeElement);
            if (assembly != null)
            {
                Version frameworkVersion = GetFrameworkVersion(assembly);
                if (frameworkVersion != null)
                {
                    ITypeInfo type = ReflectionUtils.GetType(codeElement);

                    Test assemblyTest = GetAssemblyTest(assembly, TestModel.RootTest, frameworkVersion, type == null);
                }
            }
        }

        Test GetAssemblyTest(IAssemblyInfo assembly, Test parentTest, Version frameworkVersion, bool populateRecursively)
        {
            NSpecAssemblyTest assemblyTest;

            if (!assemblyTests.TryGetValue(assembly, out assemblyTest))
            {
                assemblyTest = new NSpecAssemblyTest(assembly.Name, assembly, frameworkVersion);
                assemblyTest.Kind = TestKinds.Assembly;

                ModelUtils.PopulateMetadataFromAssembly(assembly, assemblyTest.Metadata);

                string frameworkName = String.Format("NSpec v{0}", frameworkVersion);
                assemblyTest.Metadata.SetValue(MetadataKeys.Framework, frameworkName);
                assemblyTest.Metadata.SetValue(MetadataKeys.File, assembly.Path);
                assemblyTest.Kind = TestKinds.Assembly;

                parentTest.AddChild(assemblyTest);
                assemblyTests.Add(assembly, assemblyTest);
            }

            if (populateRecursively)
            {
                Assembly resolvedAssembly = assembly.Resolve(false);
                var finder = new SpecFinder(resolvedAssembly, new NSpec.Domain.Reflector());
                var builder = new ContextBuilder(finder, new DefaultConventions());

                ContextCollection contexts = builder.Contexts();
                contexts.Build();
                contexts.Do(c => assemblyTest.AddChild(this.CreateGallioTestFrom(c)));
            }

            return assemblyTest;
        }

        NSpecContextTest CreateGallioTestFrom(Context nspecContext)
        {
            NSpecContextTest contextTest = new NSpecContextTest(nspecContext);

            nspecContext.Examples.Do(e => contextTest.AddChild(this.CreateGallioTestFrom(e)));
            nspecContext.Contexts.Do(c => contextTest.AddChild(this.CreateGallioTestFrom(c)));

            return contextTest;
        }

        NSpecExampleTest CreateGallioTestFrom(Example nspecExample)
        {
            try
            {
                NSpecExampleTest exampleTest = new NSpecExampleTest(nspecExample);

                return exampleTest;
            }
            catch
            {
                throw new Exception(String.Format("Error adding example {0}", nspecExample.Spec));
            }
        }

        Version GetFrameworkVersion(IAssemblyInfo assembly)
        {
            AssemblyName frameworkAssemblyName = ReflectionUtils.FindAssemblyReference(assembly, ASSEMBLY_DISPLAY_NAME);
            return frameworkAssemblyName != null ? frameworkAssemblyName.Version : null;
        }

        private const string ASSEMBLY_DISPLAY_NAME = @"NSpec";

        readonly Dictionary<IAssemblyInfo, NSpecAssemblyTest> assemblyTests;
    }
}