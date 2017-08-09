# This is the primary script - call it to start bumping the version on all checked out submodules...
# The user must specify an -updatestrategy when invoking the script
param ($UpdateStrategy = $(throw "Update strategy parameter is required!"))   

# Bump
.\Version-Bump.ps1  -UpdateStrategy $UpdateStrategy

# Validate that all repos got updated to the same version
.\Version-Verify.ps1