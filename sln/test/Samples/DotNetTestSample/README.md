# DotNetTestSample

Until *NSpec* and *DotnetTestNSpec* packages in this same repository are not 
publicly available on NuGet, please add the following package sources in 
*Tools\Options\NuGet Package Manager\Package Sources* menu:

* `YourDrive:\Path\To\NSpec\Repo\sln\src\NSpec\bin\Debug`
* `YourDrive:\Path\To\NSpec\Repo\sln\src\DotnetTestNSpec\bin\Debug`

then manually build and create local packages for those two projects 
from command line:

```
> YourDrive:
> cd Path\To\NSpec\Repo\sln\src\NSpec
> dotnet pack
> Path\To\NSpec\Repo\sln\src\DotnetTestNSpec
> dotnet pack
```
