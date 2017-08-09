# Get the current version of RapidCore
# Check that all other libraries have this version too..

function Get-Version {
    # $path: the path to navigate to, where we should update versions and all that
    # $goTo: where to navigate after we are done
    Param ([string]$path, [string]$goTo)

    Set-Location $path
    
    $currentVersion = & dotnet version --output-format=json | ConvertFrom-Json | Select-Object -ExpandProperty currentVersion

    Set-Location $goTo
    return $currentVersion
}

function Test-Version {
    # $path: the path to navigate to, where we should update versions and all that
    # $goTo: where to navigate after we are done
    Param ([string]$path, [string]$goTo, [string]$version)

    Set-Location $path
    Write-Host "Validating" $path -ForegroundColor Blue
    $currentVersion = & dotnet version --output-format=json | ConvertFrom-Json | Select-Object -ExpandProperty currentVersion

    if($version -ne $currentVersion) {
        Set-Location $goTo
        Throw -join("Version mismatch, ",$path," has version ",$currentVersion," but ",$version," is required")
    }
    Set-Location $goTo
}


$currentVersion = Get-Version -path .\rapidcore\src -goTo ..\..\
Write-Host "Current rapidcore version is " $currentVersion -ForegroundColor Green

Validate-Version -path .\rapidcore.mongo\src -goTo ..\..\ -version $currentVersion 
Validate-Version -path .\rapidcore.redis\src -goTo ..\..\ -version $currentVersion
Validate-Version -path .\rapidcore.xunit\src -goTo ..\..\ -version $currentVersion

Write-Host "All ok" -ForegroundColor Green
