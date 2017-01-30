# Clean
if (Test-Path sln\src\NSpec\bin\) { Remove-Item sln\src\NSpec\bin\* -Force -Recurse }
if (Test-Path sln\src\NSpec\obj\) { Remove-Item sln\src\NSpec\obj\* -Force -Recurse }
if (Test-Path sln\src\NSpec\publish\ ) { Remove-Item sln\src\NSpec\publish\* -Force -Recurse }

# Initialize
& "dotnet" restore sln\src\NSpec\
# Skip test until issue with restoring samples is fixed
###& "dotnet" restore sln\test\NSpecSpecs\

# Build
& "dotnet" build -c Release sln\src\NSpec\

# Test
# Skip test until issue with restoring samples is fixed
###& "dotnet" test -c Release sln\test\NSpecSpecs\

# Package
& "dotnet" pack -c Release sln\src\NSpec\ -o sln\src\NSpec\publish\ --version-suffix=$env:APPVEYOR_BUILD_NUMBER
