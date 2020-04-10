cd test\test\TaskApi.Unit.Test
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=../../_codecoverage/coverage.xml /p:ExcludeByAttribute="CompilerGenerated"
cd %~dp0
reportgenerator -reports:"_codecoverage/coverage.xml" -targetdir:"_codecoverage/reports"