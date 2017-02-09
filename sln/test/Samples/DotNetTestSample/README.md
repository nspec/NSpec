# DotNetTestSample

Sample solution showing how to setup NSpec with a .NET Core project and .NET Core tooling 1.0.0-preview2 (i.e. with *project.json*, not MSBuild).

Highlights:

- Test project targets both .NET Core App 1.0.0 and .NET Framework 4.5.2
- Test project is a .NET Core console application with its `buildOptions.emitEntryPoint` property stripped from `project.json`
- Test project `project.json` has a `testRunner` property set to `nspec`
- Test project takes an explicit dependency on `"Microsoft.Extensions.Testing.Abstractions": "1.0.0-preview2-003121",`,
due to an issue between .NET Core CLI Tools Preview 2 and NuGet that will not get solved (as *project.json* is being deprecated in favour of MSBuild)
- NSpec is installed into test project as a NuGet package, version 2.0.0 minimum
- dotnet-test-nspec runner too is installed into test project as a NuGet package
- Tests can be run from command line through *dotnet test*, as in:

    ```
    > cd path\to\DotNetTestSample\test\LibrarySpecs

    > dotnet test
    ```

- Tests cannot be run yet from Visual Studio Test Explorer, until dotnet-test-nspec does not support VS integration.
- To keep consistency with other samples, solution has the typical directory structure found in ASP.NET Core project template,
where all application projects are under a `src\` directory, while test projects are in a `test\` directory.
