# The Version / Tag that is being built is a required param!
param ($tagName = $(throw "-tagName parameter is required!"))   
# Include the util functions....
. .\Utils.ps1

# Check validity
Test-TagName $tagName

# TODO: Push RapidCore!

# Now wait for the build to complete....
Write-Host ( -join ("Waiting for build ", $tagName , " to complete") ) -NoNewline -ForegroundColor Cyan
do {
    $buildComplete = Test-AppVeyorBuildCompletionStatus -tagName $tagName    
    Write-Host "." -NoNewline
    Start-Sleep -seconds 5
} while (!$buildComplete)
Write-Host "" # Write a new line...
Write-Host "Build complete, pushing repositories" -ForegroundColor Green

#Test-AppVeyorBuildCompletionStatus -tagName "v0.15.0" -debugOutput $true
#Test-AppVeyorBuildCompletionStatus -tagName "v0.16.0" -debugOutput $true
#Test-AppVeyorBuildCompletionStatus -tagName "v0.17.0" -debugOutput $true
#Test-AppVeyorBuildCompletionStatus -tagName "v1.1.0" -debugOutput $true