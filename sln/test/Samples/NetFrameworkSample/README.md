# NetFrameworkSample

Sample solution showing how to setup NSpec with a .NET Framework 4.5.2 project.

Highlights:

- Test project is a class library targeting .NET Framework 4.5.2 (or above)
- NSpec is installed into test project as a NuGet package
- Tests can be run from command line through NSpecRunner, shipped with NSpec framework, as in:

    ```
    > cd path\to\NetFrameworkSample

    > packages\NSpec.x.y.z\tools\net452\NSpecRunner.exe ^
        test\LibrarySpecs\bin\Debug\LibrarySpecs.dll
    ```
- Tests can be run from Visual Studio Test Explorer, if a suitable adapter is installed, e.g. as a VSIX extension
(shameless plug: [NSpec.VsAdapter](https://github.com/BrainCrumbz/NSpec.VsAdapter)).
- To keep consistency with other samples, solution has the typical directory structure found in ASP.NET Core project template,
where all application projects are under a `src\` directory, while test projects are in a `test\` directory.
