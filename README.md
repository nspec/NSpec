# NSpec

NSpec is a BDD framework for .NET of the xSpec (context/specification) flavor. NSpec is intended to be used to drive development through specifying behavior at the unit level. NSpec is heavily inspired by RSpec and built upon the NUnit assertion library.

## Contributing

To begin contributing to the NSpec project, you will first want to make sure that you can run the tests.  To do so make sure that you have the current version of NUnit installed as referenced in the dotnet.watchr.rb file.  The current support version of NUnit is 2.5.9 and the file references this via the following line of code:

` NUnitRunner.nunit_path = 'C:\program files (x86)\nunit 2.5.9\bin\net-2.0\nunit-console-x86.exe' `