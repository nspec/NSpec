# NetFrameworkSample

Until *NSpec* package supporting .NET Core is not publicly available on NuGet or other feed, 
please make sure to pack *NSpec* locally and make that available at:

* `Path\To\This\Repo\sln\src\NSpec\bin\Debug`

To do that, you can open command line and run:

```
> dotnet pack Path\To\This\Repo\sln\src\NSpec\project.json
```
