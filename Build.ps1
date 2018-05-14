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
function Exec {
    [CmdletBinding()]
    param(
        [Parameter(Position = 0, Mandatory = 1)][scriptblock]$cmd,
        [Parameter(Position = 1, Mandatory = 0)][string]$errorMessage = ($msgs.error_bad_command -f $cmd)
    )
    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

function Use-NuGetReference {
    # $pathToCsproj: the path to the csproj file we want to update
    # $version: The version to apply
    Param ([string]$pathToCsproj, [string]$version)

    $localReference = "<ProjectReference Include=""..\..\core\main\rapidcore.csproj"" />"
    $nugetReference = "<PackageReference Include=""RapidCore"" Version=""$version"" />"

    (Get-Content $pathToCsproj).replace($localReference, $nugetReference) | Set-Content $pathToCsproj
}

if (Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

exec { & dotnet restore }

exec { & dotnet build -c Release }

$testProjects = '.\src\core\test-unit\unittests.csproj', '.\src\google-cloud\test-unit\unittests.csproj', '.\src\mongo\test-unit\unittests.csproj', '.\src\postgresql\test-unit\unittests.csproj', '.\src\redis\test-unit\unittests.csproj', '.\src\xunit\test-unit\unittests.csproj'

foreach ($testProject in $testProjects) {
    exec { & dotnet test $testProject -c Release }
}

##
# Get current version of project and append commit count, if we are not building a tag (thus creating a pre-release)
##
$revCount = & git rev-list HEAD --count | Out-String

Set-Location .\src\core\main
$version = & dotnet version --output-format=json | ConvertFrom-Json | Select-Object -ExpandProperty currentVersion
Set-Location ..\..\..\

# If we are not building a tag, update the version to include a version suffix
if ( (!$Env:APPVEYOR_REPO_TAG) -or ( $Env:APPVEYOR_REPO_TAG -ne "true") ) {
    $version = "$($version)-preview-$($revCount)".Trim()
}

##
# Update all packages to use nuget reference and pack them up as nugets
##
$projectsToBuild = '.\src\core\main\rapidcore.csproj', '.\src\google-cloud\main\rapidcore.google-cloud.csproj', '.\src\mongo\main\rapidcore.mongo.csproj', '.\src\postgresql\main\rapidcore.postgresql.csproj', '.\src\redis\main\rapidcore.redis.csproj', '.\src\xunit\main\rapidcore.xunit.csproj'

foreach ($project in $mainProjects) {
    Use-NuGetReference $project $version
    # Explicitly set the package version to include any version suffixes...
    exec { & dotnet pack $project -c Release -o ..\..\..\artifacts --include-source --no-build --no-restore /p:PackageVersion=$version }
}