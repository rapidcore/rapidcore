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

#
# install Sonar Scanner (from SonarQube)
#
exec { & dotnet tool install --global dotnet-sonarscanner }
exec { & dotnet tool install --global dotnet-version-cli }

##
# Get current version of project and append commit count, if we are not building a tag (thus creating a pre-release)
##
$revCount = & git rev-list HEAD --count | Out-String

Set-Location .\src\core\main
$version = & dotnet version --output-format=json | ConvertFrom-Json | Select-Object -ExpandProperty currentVersion

# If the environment variable APPVEYOR_PULL_REQUEST_NUMBER is not present, then this is not a pull request
$isNotPullRequest = -not $env:APPVEYOR_PULL_REQUEST_NUMBER

# If we are not building a tag, update the version to include a version suffix and a minor bump
if ( (!$Env:APPVEYOR_REPO_TAG) -or ( $Env:APPVEYOR_REPO_TAG -ne "true") ) {
    $version = & dotnet version --output-format=json --dry-run patch | ConvertFrom-Json | Select-Object -ExpandProperty newVersion
    if($isNotPullRequest) {
        $version = "$($version)-preview-$($revCount)".Trim()
    } else {
        $version = "$($version)-pr-$($env:APPVEYOR_PULL_REQUEST_NUMBER)".Trim()
    }
}

exec { & dotnet restore }

$sonarProjectKey = "rapidcore_rapidcore"
$sonarHostUrl = "https://sonarcloud.io"

Set-Location ..\..\..\
# initialize Sonar Scanner
# If the environment variable APPVEYOR_PULL_REQUEST_NUMBER is not present, then this is not a pull request
if($isNotPullRequest) {
    exec {
        & dotnet sonarscanner begin `
            /k:rapidcore_rapidcore `
            /o:rapidcore `
            /v:$version `
            /d:sonar.host.url=$sonarHostUrl `
            /d:sonar.login="$Env:SONARCLOUD_TOKEN"
    }
} else {
    exec {
        & dotnet sonarscanner begin `
            /k:rapidcore_rapidcore `
            /o:rapidcore `
            /v:$version `
            /d:sonar.host.url=$sonarHostUrl `
            /d:sonar.login="$Env:SONARCLOUD_TOKEN" `
            /d:sonar.pullrequest.branch=$Env:APPVEYOR_PULL_REQUEST_HEAD_REPO_BRANCH `
            /d:sonar.pullrequest.base=$Env:APPVEYOR_REPO_BRANCH `
            /d:sonar.pullrequest.key=$Env:APPVEYOR_PULL_REQUEST_NUMBER
    }
}

exec { & dotnet build -c Release }

$testProjects = '.\src\core\test-unit\unittests.csproj', '.\src\google-cloud\test-unit\unittests.csproj', '.\src\mongo\test-unit\unittests.csproj', '.\src\postgresql\test-unit\unittests.csproj', '.\src\redis\test-unit\unittests.csproj', '.\src\xunit\test-unit\unittests.csproj', '.\src\sqlserver\test-unit\unittests.csproj' 

foreach ($testProject in $testProjects) {
    exec { & dotnet test $testProject -c Release }
}

# trigger Sonar Scanner analysis
exec { & dotnet sonarscanner end /d:sonar.login="$Env:SONARCLOUD_TOKEN" }

##
# Update all packages to use nuget reference and pack them up as nugets
##
$mainProjects = '.\src\core\main\rapidcore.csproj', '.\src\google-cloud\main\rapidcore.google-cloud.csproj', '.\src\mongo\main\rapidcore.mongo.csproj', '.\src\postgresql\main\rapidcore.postgresql.csproj', '.\src\redis\main\rapidcore.redis.csproj', '.\src\xunit\main\rapidcore.xunit.csproj', '.\src\sqlserver\main\rapidcore.sqlserver.csproj' 

foreach ($project in $mainProjects) {
    Use-NuGetReference $project $version
    # Explicitly set the package version to include any version suffixes...
    exec { & dotnet pack $project -c Release -o ..\..\..\artifacts --include-source --no-build --no-restore /p:PackageVersion=$version }
}
