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

if(Test-Path .\artifacts) { Remove-Item .\artifacts -Force -Recurse }

exec { & dotnet restore }

$revision = @{ $true = $env:APPVEYOR_BUILD_NUMBER; $false = 1 }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$revision = "{0:D4}" -f [convert]::ToInt32($revision, 10)

exec { & dotnet build }

exec { & dotnet test '.\src\core\test-unit\unittests.csproj' -c Release }
exec { & dotnet test '.\src\mongo\test-unit\unittests.csproj' -c Release }
exec { & dotnet test '.\src\redis\test-unit\unittests.csproj' -c Release }
exec { & dotnet test '.\src\xunit\test-unit\unittests.csproj' -c Release }


# TODO - replace csproj references with nuget references to the libraries
exec { & dotnet pack .\src\core\main\rapidcore.csproj -c Release -o ..\..\..\artifacts --include-source }
exec { & dotnet pack .\src\mongo\main\rapidcore.mongo.csproj -c Release -o ..\..\..\artifacts --include-source }
exec { & dotnet pack .\src\redis\main\rapidcore.redis.csproj -c Release -o ..\..\..\artifacts --include-source }
exec { & dotnet pack .\src\xunit\main\rapidcore.xunit.csproj -c Release -o ..\..\..\artifacts --include-source }