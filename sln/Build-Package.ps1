# Utilities

# Taken from psake https://github.com/psake/psake
<#
.SYNOPSIS
  This is a helper function that runs a scriptblock and checks the PS variable $lastexitcode
  to see if an error occcured. If an error is detected then an exception is thrown.
  This function allows you to run command-line programs without having to
  explicitly check the $lastexitcode variable.
.EXAMPLE
  exec { svn info $repository_trunk } "Error executing SVN. Please verify SVN command-line client is installed"
#>
function Exec
{
	[CmdletBinding()]
	param(
		[Parameter(Position=0,Mandatory=1)][scriptblock]$cmd,
		[Parameter(Position=1,Mandatory=0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
	)
	$global:lastexitcode = 0
	& $cmd
	if ($lastexitcode -ne 0) {
		throw ("Exec: " + $errorMessage)
	}
}

function CleanContent([string]$path) {
	if (Test-Path $path) {
		$globPath = Join-Path $path *
		Remove-Item -Force -Recurse $globPath
	}
}

function CleanProject([string]$projectPath) {
	@(
		(Join-Path $projectPath bin\ ), `
		(Join-Path $projectPath obj\ ), `
		(Join-Path $projectPath publish\ ) `

	) | ForEach-Object { CleanContent $_ }
}

###

# Clean
@(
	"sln\src\NSpec", `
	"sln\src\NSpecRunner", `
	"sln\src\DotNetTestNSpec", `
	"sln\test\NSpecSpecs", `
	"sln\test\Samples\SampleSpecs", `
	"sln\test\Samples\SampleSpecsFocus", `
	"sln\test\DotNetTestNSpecSpecs"

) | ForEach-Object { CleanProject $_ }

# Initialize
@(
	"sln\src\NSpec", `
	"sln\src\NSpecRunner", `
	"sln\src\DotNetTestNSpec", `
	"sln\test\NSpecSpecs", `
	"sln\test\Samples\SampleSpecs", `
	"sln\test\Samples\SampleSpecsFocus", `
	"sln\test\DotNetTestNSpecSpecs"

) | ForEach-Object { Exec { & "dotnet" restore $_ } }


# Build
@(
	"sln\src\NSpec", `
	"sln\src\NSpecRunner", `
	"sln\src\DotNetTestNSpec", `
	"sln\test\NSpecSpecs", `
	"sln\test\DotNetTestNSpecSpecs"

) | ForEach-Object { Exec { & "dotnet" build -c Release $_ } }


# Test
@(
	"sln\test\NSpecSpecs", `
	"sln\test\DotNetTestNSpecSpecs"

) | ForEach-Object { Exec { & "dotnet" test -c Release $_ } }


# Package
$isContinuous = [bool]$env:APPVEYOR_BUILD_NUMBER
$isProduction = [bool]$env:APPVEYOR_REPO_TAG_NAME

$versioningOpt = if ($isContinuous) {
	if ($isProduction) {
		Write-Host "Continuous Delivery, Production package, keeping nuspec version."
		@()
	} else {
		$suffix = "dev-$env:APPVEYOR_BUILD_NUMBER"
		Write-Host "Continuous Delivery, Development package, version suffix: '$suffix'."
		@( "-suffix", $suffix )
	}
} else {
	Write-Host "Local machine, keeping nuspec version."
	@()
}

Exec {
	& "nuget" pack sln\src\NSpec\NSpec.nuspec `
		$versioningOpt `
		-outputdirectory sln\src\NSpec\publish\ `
		-properties Configuration=Release
}

Exec {
	& "nuget" pack sln\src\DotNetTestNSpec\DotNetTestNSpec.nuspec `
		$versioningOpt `
		-outputdirectory sln\src\DotNetTestNSpec\publish\ `
		-properties Configuration=Release
}
