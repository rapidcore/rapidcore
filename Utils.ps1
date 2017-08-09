function Test-UpdateStrategy {
    Param(
        [Parameter(Mandatory=$true)]
        [string]$UpdateStrategy
    )
    
    # Check that it's an allowed value
    $validStrategy = $UpdateStrategy -match "^major|minor|patch$"
    if ($validStrategy -ne $true) {
        throw "Invalid update strategy, must be one of: major, minor, patch"
    }
}