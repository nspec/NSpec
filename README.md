# NSpec

NSpec is a BDD framework for .NET of the xSpec (context/specification) flavor. NSpec is intended to be used to drive development through specifying behavior at the unit level. NSpec is heavily inspired by RSpec and built upon the NUnit assertion library.

## Contributing

The Nspec test suite is written in NUnit. The test project is NSpecSpecs. Not to be confused with SampleSpecs which hosts numerous tests written in NSpec, some of which are intended to fail.

I prefer using ncrunch to run the NSpec test suite. But you can also use Specwatchr, since it has support for NUnit. To do so make sure that you have the current version of NUnit installed since the dotnet.watchr.rb file contains a hard reference to the 2.5.9 binary.

` NUnitRunner.nunit_path = 'C:\program files (x86)\nunit 2.5.9\bin\net-2.0\nunit-console-x86.exe' `
