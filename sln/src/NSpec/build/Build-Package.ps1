# Utilities

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

) | ForEach-Object { & "dotnet" restore $_ }


# Build
@(
	"sln\src\NSpec", `
	"sln\src\NSpecRunner" `
	# Skip test until issue with restoring samples is fixed
	###"sln\test\NSpecSpecs\"

) | ForEach-Object { & "dotnet" build -c Release $_ }

# Package
$isContinuous = ($env:APPVEYOR_BUILD_NUMBER -ne $null)
$isProduction = ($env:APPVEYOR_REPO_TAG -ne $null)

$versioningOpt = if ($isContinuous) {
	if ($isProduction) {
		@( "-version", $env:APPVEYOR_REPO_TAG )
	} else {
		@( "-suffix", "dev-$env:APPVEYOR_BUILD_NUMBER" )
	}
} else {
	@()
}

& "nuget" pack sln\src\NSpec\NSpec.nuspec -outputdirectory sln\src\NSpec\publish\ -properties Configuration=Release $versioningOpt
