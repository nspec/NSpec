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
	"sln\src\NSpecRunner" `

) | ForEach-Object { CleanProject $_ }

# Initialize
@(
	"sln\src\NSpec", `
	"sln\src\NSpecRunner" `
	# Skip test until issue with restoring samples is fixed
	###"sln\test\NSpecSpecs\"

) | ForEach-Object { Exec { & "dotnet" restore $_ } }


# Build
@(
	"sln\src\NSpec", `
	"sln\src\NSpecRunner" `
	# Skip test until issue with restoring samples is fixed
	###"sln\test\NSpecSpecs\"

) | ForEach-Object { Exec { & "dotnet" build -c Release $_ } }

# Package
$isContinuous = ($env:APPVEYOR_BUILD_NUMBER -ne $null)
$isProduction = ($env:APPVEYOR_REPO_TAG -ne $null)

Write-Host "Repo tag__$env:APPVEYOR_REPO_TAG__"

$versioningOpt = if ($isContinuous) {
	if ($isProduction) {
		@( "-version", $env:APPVEYOR_REPO_TAG )
	} else {
		@( "-suffix", "dev-$env:APPVEYOR_BUILD_NUMBER" )
	}
} else {
	@()
}

Exec {
	& "nuget" pack sln\src\NSpec\NSpec.nuspec `
		$versioningOpt `
		-outputdirectory sln\src\NSpec\publish\ `
		-properties Configuration=Release
}
