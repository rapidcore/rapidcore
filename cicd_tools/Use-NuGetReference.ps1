. ./cicd_tools/Exec.ps1
. ./cicd_tools/Get-MainProjects.ps1

function Use-NuGetReference {
    # $pathToCsproj: the path to the csproj file we want to update
    # $version: The version to apply
    Param ([string]$pathToCsproj, [string]$version)

    $localReference = "<ProjectReference Include=""..\..\core\main\rapidcore.csproj"" />"
    $nugetReference = "<PackageReference Include=""RapidCore"" Version=""$version"" />"

    (Get-Content $pathToCsproj).replace($localReference, $nugetReference) | Set-Content $pathToCsproj
}

$version = $args[0]

Write-Host "Will use $version instead of project reference"
Get-Location | Write-Host

##
# Update all packages to use nuget reference and pack them up as nugets
##
$mainProjects = Get-MainProjects "src\"

foreach ($csproj in $mainProjects) {
    Use-NuGetReference $csproj $version
}
