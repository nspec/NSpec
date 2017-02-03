# NSpec

[![NuGet Version and Downloads count](https://buildstats.info/nuget/NSpec)](https://www.nuget.org/packages/NSpec) [![Build status](https://ci.appveyor.com/api/projects/status/5mmtg044ds5xx8xr/branch/master?svg=true)](https://ci.appveyor.com/project/BrainCrumbz/nspec/branch/master)

NSpec is a BDD framework for .NET of the xSpec (context/specification) flavor. NSpec is intended to be used to drive development through specifying behavior at the unit level. NSpec is heavily inspired by RSpec and built upon the NUnit assertion library.

NSpec is written by [Matt Florence](http://twitter.com/mattflo) and [Amir Rajan](http://twitter.com/amirrajan). It's shaped and benefited by hard work from our [contributors](https://github.com/nspec/NSpec/contributors).

## Documentation

See [nspec.org](http://nspec.org/) for instructions on getting started and documentation.

## Samples

See under [sln/test/Samples](./sln/test/Samples):

- [DotNetTestSample](./sln/test/Samples/DotNetTestSample) and [NetFrameworkSample](./sln/test/Samples/NetFrameworkSample):
those are independent solutions with code under test and test project, importing NSpec packages

- [SampleSpecs](./sln/test/Samples/SampleSpecs) and [SampleSpecsFocus](./sln/test/Samples/SampleSpecsFocus):
those are projects found within main NSpec solution, needed when testing NSpec itself, with examples of test classes

## Breaking changes

To check for potential breaking changes, see [BREAKING-CHANGES.md](./BREAKING-CHANGES.md).

## Contributing

See [contributing](CONTRIBUTING.md) doc page.

## License

[MIT](./license.txt)
