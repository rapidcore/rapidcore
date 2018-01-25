# Bumping the version

We have decided that the version number across all the packages should be the same, even when a new feature have not affected a certain sub-package.

All packages follow the version of `RapidCore`.

## Using it

Ensure you have `Powershell` installed ([get it here](https://github.com/PowerShell/PowerShell#get-powershell)).

```shell
$ powershell ./Bump-Version.ps1 <major|minor|patch>
$ git push
$ git push --tags
```

This will bump the version of all packages and create a commit and a tag with the new version.

The repository must be clean - otherwise an error is thrown.
