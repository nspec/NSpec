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

Push-Location sln

# override AppVeyor build version, trying to avoid collisions

$nuSpecPath = "src\NSpec\NSpec.nuspec"

$buildNumber = $env:APPVEYOR_BUILD_NUMBER
$suffix = "dev-$buildNumber"

$nuSpecVersion = GetNuSpecVersion $nuSpecPath
$uniqueBuildVersion = "$nuSpecVersion-$suffix"

Write-Host "Changing build version to '$uniqueBuildVersion'..."

Update-AppveyorBuild -Version $uniqueBuildVersion

Pop-Location
