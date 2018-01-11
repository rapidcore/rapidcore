#!/usr/bin/env bash

# Perform the actual build
dotnet restore \
&& dotnet build -c Release -f netstandard1.6 src/rapidcore.redis.csproj \
&& dotnet test test/unit/unittests.csproj -c Release -f netcoreapp1.1 \
&& dotnet test test/functional/functionaltest.csproj -c Release -f netcoreapp1.1