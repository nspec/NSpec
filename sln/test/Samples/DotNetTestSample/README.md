# DotNetTestSample

Until *NSpec* and *DotNetTestNSpec* packages supporting .NET Core are not publicly available on NuGet or other feed, 
please make sure to pack *NSpec* and *DotNetTestNSpec* locally and make them available respectively at:

* `Path\To\This\Repo\sln\src\NSpec\bin\Debug`
* `Path\To\This\Repo\sln\src\DotNetTestNSpec\bin\Debug`

To do that, you can open command line and run:

```
> dotnet pack Path\To\This\Repo\sln\src\NSpec\project.json
> dotnet pack Path\To\This\Repo\sln\src\DotNetTestNSpec\project.json
```
