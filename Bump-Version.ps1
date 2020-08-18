# Bump the version of the core package with the given level.
# Then set the new version on all other packages.

param ($BumpLevel = $(throw "Bump level parameter is required!"))   

function Update-Package {
    # $path: the path to navigate to, where we should update versions and all that
    # $goTo: where to navigate after we are done
    # $version: The version to apply
    Param ([string]$path, [string]$goTo, [string]$version)
    
    Write-Host "updating " $path -ForegroundColor Blue
    Set-Location $path

    # Invoke the dotnet version tool to create version commit + git tag
    $newVersion = & dotnet version --output-format=json --skip-vcs $version | ConvertFrom-Json | Select-Object -ExpandProperty newVersion

    Set-Location $goTo
    return $newVersion
}


function Update-InfoForActions {
    Param ([string]$version)

    $file = "info_for_actions.json"

    Write-Host "updating " $file

    $info = Get-Content $file -Raw | ConvertFrom-Json
    $info.version = $version
    $info | ConvertTo-Json -Depth 32 | Set-Content $file
}


# Go to the rapidcore repo and update
Set-Location ./src/core/main
$newVersion = & dotnet version --output-format=json --skip-vcs $BumpLevel | ConvertFrom-Json | Select-Object -ExpandProperty newVersion
Set-Location ../../

Write-Host "Version of rapidcore is: " $newVersion " setting this explicit version on all other rapidcore libs" -ForegroundColor Green

Update-Package -path .\google-cloud\main -goTo ..\..\ -version $newVersion |Out-Null
Update-Package -path .\mongo\main -goTo ..\..\ -version $newVersion |Out-Null
Update-Package -path .\postgresql\main -goTo ..\..\ -version $newVersion |Out-Null
Update-Package -path .\redis\main -goTo ..\..\ -version $newVersion |Out-Null
Update-Package -path .\xunit\main -goTo ..\..\ -version $newVersion |Out-Null

Set-Location ../ # move to the root of the repo

Update-InfoForActions $newVersion | Out-Null

$versionString = -join("v",$newVersion);
$output = (& git add -Av 2>&1)
$output = (& git commit -m $versionString 2>&1)
$output = (& git tag $versionString 2>&1)
