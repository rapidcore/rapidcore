enum VersionMode {
    Production
    Preview
    PullRequest
}

function Get-Version {
    # $mode: Production, preview or PR
    # $pullRequestNumber: The number of the pull request - when relevant
    Param ([VersionMode]$mode, [int]$pullRequestNumber = 0)

    $infoForActionsFile = "info_for_actions.json"

    $version = Get-Content $infoForActionsFile | ConvertFrom-Json | Select-Object -ExpandProperty version

    switch ($mode) {
        ([VersionMode]::Production) {
            # this is just a noop
            $version = $version
        }

        ([VersionMode]::PullRequest) {
            $version = "${version}-pr-${pullRequestNumber}"
        }
        
        Default {
            $revCount = & git rev-list HEAD --count | Out-String
            $version = "$($version)-preview-$($revCount)".Trim()
        }
    }

    return $version
}
