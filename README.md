# NSpec

[![NuGet Version and Downloads count](https://buildstats.info/nuget/NSpec)](https://www.nuget.org/packages/NSpec) 
[![Build status](https://ci.appveyor.com/api/projects/status/5mmtg044ds5xx8xr/branch/master?svg=true)](https://ci.appveyor.com/project/BrainCrumbz/nspec/branch/master)

NSpec is a BDD framework for .NET of the xSpec (context/specification) flavor. NSpec is
intended to be used to drive development through specifying behavior at the unit level.
NSpec is heavily inspired by RSpec and built upon the NUnit assertion library.

## Documentation

See [nspec.org](http://nspec.org/) for instructions on getting started and documentation.

## Examples

See under [examples/](./examples):

- [DotNetTestSample](./examples/DotNetTestSample)  
Sample solution showing how to setup a NSpec test project targeting .NET Core

- [NetFrameworkSample](./examples/NetFrameworkSample)  
Sample solution showing how to setup a NSpec test project targeting .NET Framework

Also, there are a couple of projects under [sln/test/Samples/](./sln/test/Samples) path,
[SampleSpecs](./sln/test/Samples/SampleSpecs) and [SampleSpecsFocus](./sln/test/Samples/SampleSpecsFocus).
Those are part of main NSpec solution, needed when testing NSpec itself, and contain several
mixed examples of NSpec test classes.

## Breaking changes

To check for potential breaking changes, see [BREAKING-CHANGES.md](./BREAKING-CHANGES.md).

## Contributing

See [contributing](CONTRIBUTING.md) doc page.

## License

[MIT](./license.txt)

## Credits

NSpec is written by [Matt Florence](http://twitter.com/mattflo) and
[Amir Rajan](http://twitter.com/amirrajan). It's shaped and benefited by hard work from
our [contributors](https://github.com/nspec/NSpec/contributors).
