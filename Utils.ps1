function Test-UpdateStrategy {
    Param(
        [Parameter(Mandatory = $true)]
        [string]$UpdateStrategy
    )
    
    # Check that it's an allowed value
    $validStrategy = $UpdateStrategy -match "^major|minor|patch$"
    if ($validStrategy -ne $true) {
        throw "Invalid update strategy, must be one of: major, minor, patch"
    }
}

function Test-TagName {
    ##############################
    #.SYNOPSIS Test the given tag name for validity
    ##
    #
    #.DESCRIPTION
    # Tests the given tag name for whether it starts with v which it should!
    #
    #.PARAMETER tagName
    # The tag to test
    #
    #.EXAMPLE
    # Test-TagName "v1.0" => $true
    # Test-TagName "1.0" => $false
    #
    #.NOTES
    #
    ##############################
    Param(
        [Parameter(Mandatory = $true)]
        [string]$tagName
    )
    
    if (! ($tagName -match "^v") ) {
        throw -join ("Tag names should start with 'v' - you provided: ", $tagName)
    }
}

function Test-AppVeyorBuildCompletionStatus {
    ##############################
    #.SYNOPSIS Query the AppVeyor Build API for the build status on RapidCore for a given tag name
    ##
    #
    #.DESCRIPTION
    #
    #
    #.PARAMETER tagName
    #The tag, e.g "v0.16.0" to check for build completion
    #
    #.PARAMETER debugOutput
    #Whether to print debug output
    #
    #.EXAMPLE
    # Get-AppVeyorBuildCompletionStatus -tagName "v0.16.0"
    #
    ##############################
    Param(
        [Parameter(Mandatory = $true)]
        [string]$tagName,
        [bool]$debugOutput
    )

    if ($debugOutput -eq $true) {
        Write-Host ( -join ("Build status for ", $tagName, " ")) -NoNewline -ForegroundColor Cyan
    }

    # Get the last 5 builds from AppVeyor on RapidCore...
    $headers = @{
        "Content-type" = "application/json"
    }
    $response = Invoke-RestMethod -Uri 'https://ci.appveyor.com/api/projects/nover/rapidcore/history?recordsNumber=5' -Headers $headers -Method Get

    #if ($debugOutput) {
    #    Write-Host "Have the following builds... " -ForegroundColor cyan
    #    foreach ($obj in $response.builds) {
    #        Write-Host ( $obj ) -ForegroundColor cyan
    #    }
    #}
    
    # Send all fetched builds into the pipeline and select the one where AppVeyor is building / has built the given tag
    # Where returns a $null object if it can't find a match...
    # The $_ operator means "the current object we are working on"
    $daBuild = $response.builds | Where-Object {$_.tag -eq $tagName}

    # Is that build even there? 
    if ($daBuild -eq $null) {
        return $false
    }

    # Okay, tag is there, but was it successful?
    $buildComplete = $daBuild.status -match "success"
    return $buildComplete
}