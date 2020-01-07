#!/usr/bin/env bash

# Perform the actual build
dotnet restore

#
# Build everything
#
dotnet build -c Release -f netstandard2.0 src/core/main/rapidcore.csproj || { echo 'main project build failure' ; exit 1; }
dotnet build -c Release -f netstandard2.0 src/google-cloud/main/rapidcore.google-cloud.csproj || { echo 'google-cloud project build failure' ; exit 1; }
dotnet build -c Release -f netstandard2.0 src/mongo/main/rapidcore.mongo.csproj || { echo 'mongo project build failure' ; exit 1; }
dotnet build -c Release -f netstandard2.0 src/postgresql/main/rapidcore.postgresql.csproj || { echo 'postgresql project build failure' ; exit 1; }
dotnet build -c Release -f netstandard2.0 src/sqlserver/main/rapidcore.sqlserver.csproj || { echo 'sqlserver project build failure' ; exit 1; }
dotnet build -c Release -f netstandard2.0 src/redis/main/rapidcore.redis.csproj || { echo 'redis project build failure' ; exit 1; }
dotnet build -c Release -f netstandard2.0 src/xunit/main/rapidcore.xunit.csproj || { echo 'xunit helpers project build failure' ; exit 1; }

#
# Run unit tests
#
dotnet test src/test-unit/unittests.csproj -c Release -f netcoreapp2.0 || { echo 'unit tests failed' ; exit 1; }

#
# Run functional tests
#
dotnet test src/core/test-functional/functionaltests.csproj -c Release -f netcoreapp2.0 || { echo 'main project functional tests failed' ; exit 1; }
dotnet test src/google-cloud/test-functional/functionaltests.csproj -c Release -f netcoreapp2.0 || { echo 'google-cloud project functional tests failed' ; exit 1; }
dotnet test src/mongo/test-functional/functionaltests.csproj -c Release -f netcoreapp2.0 || { echo 'mongo project functional tests failed' ; exit 1; }
dotnet test src/postgresql/test-functional/functionaltests.csproj -c Release -f netcoreapp2.0 || { echo 'postgresql project functional tests failed' ; exit 1; }
dotnet test src/sqlserver/test-functional/functionaltests.csproj -c Release -f netcoreapp2.0 || { echo 'sqlserver project functional tests failed' ; exit 1; }
dotnet test src/redis/test-functional/functionaltest.csproj -c Release -f netcoreapp2.0 || { echo 'redis project functional tests failed' ; exit 1; }

#
# Build documentation
#
docker build -t rapidcore:mkdocs -f Dockerfile.mkdocs .
docker run --volume=${PWD}:/app/repository:rw -p 8000:8000 -it rapidcore:mkdocs build
