@REM https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish
@REM https://docs.microsoft.com/en-us/dotnet/core/rid-catalog
dotnet publish vtf-converter -c Release
@REM -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true --runtime win-x64