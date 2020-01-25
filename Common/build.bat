dotnet restore
dotnet build BasicWorld.sln --configuration Release
REM dotnet test Chompies.Shared.Tests/Chompies.Shared.Tests.csproj
dotnet publish BasicWorld.sln --configuration Release
copy BasicWorld\bin\Release\netstandard2.0\publish\*.dll ..\Player\Assets\Libraries
