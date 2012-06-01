# NSpec

NSpec is a BDD framework for .NET of the xSpec (context/specification) flavor. NSpec is intended to be used to drive development through specifying behavior at the unit level. NSpec is heavily inspired by RSpec and built upon the NUnit assertion library.

NSpec is written by [Matt Florence](http://twitter.com/mattflo) and [Amir Rajan] (http://twitter.com/amirrajan). It's shaped and benefited by hard work from our [contributors](https://github.com/mattflo/NSpec/contributors)

## Contributing

The Nspec test suite is written in NUnit. The test project is NSpecSpecs. Not to be confused with SampleSpecs which hosts numerous tests written in NSpec, some of which are intended to fail.

To run the NSpec test suite, you can use ncrunch or [Specwatchr](http://nspec.org/continuoustesting) which as support for `NUnit 2.5.9`. For Specwatchr, the `dotnet.watchr.rb` file contains a hard reference to the `2.5.9` binary which may need to be updated to your installed version. To do so, locate the following line:

    NUnitRunner.nunit_path = 'C:\program files (x86)\nunit 2.5.9\bin\net-2.0\nunit-console-x86.exe'

Other wise you can get started by the running the following commands:

    bundle install              (installs all required gems)
    rake                        (builds and runs unit tests)
    rake build                  (builds solution)
    rake spec                   (runs NSpecSpecs test suite with NUnit)
    rake samples [spec_name]    (runs spec_name in SampleSpecs with NSpecRunner)

If you have Resharper 6.1 there is a team-shared settings file in the repository. Please use the settings to format any new code you write.

Fork the project, make your changes, and then send a Pull Request.
