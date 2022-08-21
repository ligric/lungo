ECHO ON
SETLOCAL
SET VERSION=%1

rmdir obj /s /q
rmdir Release /s /q
dotnet build tools\versionupdater\versionupdater.csproj /p:Configuration=Release /p:OutputPath=..\bin || exit /B 1
tools\bin\versionupdater -v %VERSION% || exit /B 1

dotnet build /p:RestorePackages=true /p:Configuration=Release src/Lungo.Wpf/Lungo.Wpf.csproj
nuget pack build\Lungo.nuspec -Version %VERSION% -outputdirectory Artifacts  || exit /B 1
