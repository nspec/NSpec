# Breaking Changes

## 3.0.0

### Assertions

Assertions have been moved from `NSpec` namespace to `NSpec.Assertions`.

#### Reason

When client test code is also using Shouldly, NSpec assertions go in conflict
with those from `Shouldly` namespace.

#### Workaround

Everywhere NSpec assertions are used, add a `using NSpec.Assertions;` directive.

## 2.0.0

### .NET Framework 4.5

Removed support for .NET Framework 4.5. Minimum required version is now .NET 4.5.2.

#### Reason

In order to have a single .NET Core project targeting both .NET Core and .NET Frameowrk.

#### Workaround

Remain on NSpec 1.0.13 if you need to support .NET Framework 4.5 or 4.5.1.

### Assertions and NUnit dependency

Removed most assertions. The few left (on true/false expectation) have been renamed to
follow .NET naming conventions. Removed dependency from NUnit.

#### Reason

In order to remove dependency from NUnit and keep NSpec focused as a testing framework.

#### Workraround

Use an assertion library, like e.g. [FluentAssertions](http://www.fluentassertions.com/)
or [Shouldly](http://shouldly.readthedocs.io/en/latest/), and replace old NSpec assertions
with theirs.

If your project relied on NUnit transitive dependency from NSpec, install NUnit directly
in your project.
