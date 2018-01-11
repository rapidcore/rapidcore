#!/usr/bin/env bash

# Perform the actual build
dotnet restore

# Build the core and test
dotnet build -c Release -f netstandard1.6 src/core/main/rapidcore.csproj \
&& dotnet test src/core/test-unit/unittests.csproj -c Release -f netcoreapp1.1 \
&& dotnet test src/core/test-functional/functionaltests.csproj -c Release -f netcoreapp1.1

# Build mongo and test
dotnet build -c Release -f netstandard1.6 src/mongo/main/rapidcore.mongo.csproj \
&& dotnet test src/mongo/test-unit/unittests.csproj -c Release -f netcoreapp1.1 \
&& dotnet test src/mongo/test-functional/functionaltests.csproj -c Release -f netcoreapp1.1

# Build redis and test
dotnet build -c Release -f netstandard1.6 src/redis/main/rapidcore.redis.csproj \
&& dotnet test src/redis/test-unit/unittests.csproj -c Release -f netcoreapp1.1 \
&& dotnet test src/redis/test-functional/functionaltest.csproj -c Release -f netcoreapp1.1

# Build xunit extensions and test
dotnet build -c Release -f netstandard1.6 src/xunit/main/rapidcore.xunit.csproj \
&& dotnet test src/xunit/test-unit/unittests.csproj -c Release -f netcoreapp1.1