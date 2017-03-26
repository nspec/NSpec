# Utilities

function GetNuSpecVersion([string]$path) {
	if (-Not (Test-Path $path)) {
		return "0.0.0"
	}

	$versionRegex = [regex] '<version>(.*)</version>'

	$content = Get-Content $path -Raw

	if ($content -notmatch $versionRegex) {
		return "0.0.0"
	}

	$nuSpecVersion = $matches[1].TrimEnd()

	return $nuSpecVersion
}

###

# Main

cd sln

# override AppVeyor build number, trying to avoid collisions

$nuSpecPath = "src\NSpec\NSpec.nuspec"

$buildNumber = $env:APPVEYOR_BUILD_NUMBER
$suffix = "dev-$buildNumber"

$nuSpecVersion = GetNuSpecVersion $nuSpecPath
$uniqueBuildNumber = "$nuSpecVersion-$suffix"

Write-Host "Changing build number to '$uniqueBuildNumber'..."

Update-AppveyorBuild -Version $uniqueBuildNumber
