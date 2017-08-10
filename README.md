# Rapidcore versioning

This repository is used for bumping the version on all rapidcore libraries.
We have decided that the version number across all the libraries should be the same, even when a new feature might not have been pushed in a given repository.
All libraries follow the version of `RapidCore`.

The various rapidcore repos are included as submodules so clone this repository with the `--recursive` flag.

## Using it

Ensure you have `Powershell` installed ([get it here](https://github.com/PowerShell/PowerShell#get-powershell)).

```shell
$ git clone <this_repository>
$ cd versioning
$ git submodule init
$ git submodule update
$ powershell ./Go.ps1 <major|minor|patch>
```
