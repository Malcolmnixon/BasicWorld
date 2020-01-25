#!/bin/sh

dotnet restore
dotnet build BasicWorld.sln --configuration Release
# dotnet test BasicWorld.Tests/BasicWorld.Tests.csproj
dotnet publish BasicWorld.sln --configuration Release
cp BasicWorld/bin/Release/netstandard2.0/publish/*.dll ../Player/Assets/Libraries/
