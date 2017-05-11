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

function GetVersionOptions() {
	$isContinuous = [bool]$env:APPVEYOR
	$isProduction = [bool]$env:APPVEYOR_REPO_TAG_NAME

	if ($isContinuous) {
		if ($isProduction) {
			Write-Host "Continuous Delivery, Production package, keeping nupkg version as is."
			$versionOpts = @()
		} else {
			# this should have already been updated to development version number (<nuspec vers>-dev-<build nr>)
			$devPackageVersion = $env:APPVEYOR_BUILD_VERSION

			Write-Host "Continuous Delivery, Development package, changing nupkg version to '$devPackageVersion'."

			$versionOpts = @( "-version", $devPackageVersion )
		}
	} else {
		Write-Host "Local machine, keeping nupkg version as is."
		$versionOpts = @()
	}

	return $versionOpts
}

# Toolchain commands

function RestoreProject([string]$projectPath) {
	Exec { & dotnet restore $projectPath } "Restoring $projectPath"
}

function BuildProject([string]$projectPath) {
	Exec { & dotnet build -c Release $projectPath } "Building $projectPath"
}

function TestProject([string]$projectPath) {
	Exec { & dotnet test -c Release $projectPath } "Testing $projectPath"
}

function PackageProject([string]$projectPath, [string[]]$versionOpts) {
	$projName = Split-Path $projectPath -Leaf
	$nuspecFile = Join-Path $projectPath "$projName.nuspec"
	$publishDir = Join-Path $projectPath "publish"

	Exec {
		& nuget pack $nuspecFile $versionOpts -outputdirectory $publishDir -properties Configuration=Release

	} "Packaging $_"
}

###

# Main

# move to global.json directory
Push-Location sln


# Clean
@(
	"src\NSpec", `
	"src\NSpecRunner", `
	"test\NSpec.Tests", `
	"test\Samples\SampleSpecs", `
	"test\Samples\SampleSpecsApi", `
	"test\Samples\SampleSpecsFocus"

) | ForEach-Object { CleanProject $_ }


# Restore
@(
	"src\NSpec", `
	"src\NSpecRunner", `
	"test\NSpec.Tests", `
	"test\Samples\SampleSpecs", `
	"test\Samples\SampleSpecsApi", `
	"test\Samples\SampleSpecsFocus"

) | ForEach-Object { RestoreProject $_ }


# Build
@(
	"src\NSpec", `
	"src\NSpecRunner", `
	"test\NSpec.Tests"

) | ForEach-Object { BuildProject $_ }


# Test
@(
	"test\NSpec.Tests"

) | ForEach-Object { TestProject $_ }


# Package
$versionOpts = GetVersionOptions

@(
	"src\NSpec"

) | ForEach-Object { PackageProject $_ $versionOpts }


Pop-Location
