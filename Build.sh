#!/usr/bin/env bash

# Perform the actual build
dotnet restore

# Build the core and test
dotnet build -c Release -f netstandard2.0 src/core/main/rapidcore.csproj \
&& dotnet test src/core/test-unit/unittests.csproj -c Release -f netcoreapp2.0 \
&& dotnet test src/core/test-functional/functionaltests.csproj -c Release -f netcoreapp2.0 || { echo 'main project build / test failure' ; exit 1; }

# Build google-cloud and test
dotnet build -c Release -f netstandard2.0 src/google-cloud/main/rapidcore.google-cloud.csproj \
&& dotnet test src/google-cloud/test-unit/unittests.csproj -c Release -f netcoreapp2.0 \
&& dotnet test src/google-cloud/test-functional/functionaltests.csproj -c Release -f netcoreapp2.0 || { echo 'google-cloud project build / test failure' ; exit 1; }

# Build mongo and test
dotnet build -c Release -f netstandard2.0 src/mongo/main/rapidcore.mongo.csproj \
&& dotnet test src/mongo/test-unit/unittests.csproj -c Release -f netcoreapp2.0 \
&& dotnet test src/mongo/test-functional/functionaltests.csproj -c Release -f netcoreapp2.0 || { echo 'mongo project build / test failure' ; exit 1; }

# Build postgreSql and test
dotnet build -c Release -f netstandard2.0 src/postgresql/main/rapidcore.postgresql.csproj \
&& dotnet test src/postgresql/test-unit/unittests.csproj -c Release -f netcoreapp2.0 \
&& dotnet test src/postgresql/test-functional/functionaltests.csproj -c Release -f netcoreapp2.0 || { echo 'postgresql project build / test failure' ; exit 1; }

# Build postgreSql and test
dotnet build -c Release -f netstandard2.0 src/sqlserver/main/rapidcore.sqlserver.csproj \
&& dotnet test src/sqlserver/test-unit/unittests.csproj -c Release -f netcoreapp2.0 \
&& dotnet test src/sqlserver/test-functional/functionaltests.csproj -c Release -f netcoreapp2.0 || { echo 'sqlserver project build / test failure' ; exit 1; }

# Build redis and test
dotnet build -c Release -f netstandard2.0 src/redis/main/rapidcore.redis.csproj \
&& dotnet test src/redis/test-unit/unittests.csproj -c Release -f netcoreapp2.0 \
&& dotnet test src/redis/test-functional/functionaltest.csproj -c Release -f netcoreapp2.0 || { echo 'redis project build / test failure' ; exit 1; }

# Build xunit extensions and test
dotnet build -c Release -f netstandard2.0 src/xunit/main/rapidcore.xunit.csproj \
&& dotnet test src/xunit/test-unit/unittests.csproj -c Release -f netcoreapp2.0 || { echo 'xunit helpers project build / test failure' ; exit 1; }

# Build documentation
docker build -t rapidcore:mkdocs -f Dockerfile.mkdocs .
docker run --volume=${PWD}:/app/repository:rw -p 8000:8000 -it rapidcore:mkdocs build
