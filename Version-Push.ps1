
. .\Utils.ps1

Get-AppVeyorBuildCompletionStatus -tagName "v0.15.0" -debugOutput $true
Get-AppVeyorBuildCompletionStatus -tagName "v0.16.0" -debugOutput $true
Get-AppVeyorBuildCompletionStatus -tagName "v0.17.0" -debugOutput $true
Get-AppVeyorBuildCompletionStatus -tagName "v1.1.0" -debugOutput $true