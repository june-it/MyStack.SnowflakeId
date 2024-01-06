# Release
dotnet restore
dotnet build -c Release
# Push to NuGet
cd src/MyStack.SnowflakeIdGenerator/bin/Release
dotnet nuget push MyStack.SnowflakeIdGenerator.*.nupkg  --api-key $env:NUGET_API_KEY --source https://api.nuget.org/v3/index.json --skip-duplicate

cd ../../../../