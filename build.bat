@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)


echo **************************************************
echo ****  version            %version% 
echo ****  BuildCounter       %BuildCounter%
echo ****  packversionsuffix  %packversionsuffix% 
echo ****  PackageVersion     %PackageVersion%
echo **************************************************

set version=
REM if not "%BuildCounter%" == "" (
REM   set packversionsuffix=-%BuildCounter%
REM )
REM set PackageVersion=1.0.0-Beta1-%BuildCounter%
set PackageVersion=1.0.0
set version = %PackageVersion%

echo **************************************************
echo ****  version            %version% 
echo ****  BuildCounter       %BuildCounter%
echo ****  packversionsuffix  %packversionsuffix% 
echo ****  PackageVersion     %PackageVersion%
echo **************************************************

REM (optional) build.bat is in the root of our repo, cd to the correct folder where sources/projects are
REM cd MyLibrary

REM Restore
echo *********************************
echo ****  Restoring package
echo *********************************

call dotnet restore SharpBatch.sln --verbosity m
if not "%errorlevel%"=="0" goto failure

echo *********************************
echo ****  Restore completed
echo *********************************

REM Build
echo *********************************
echo ****  Build started
echo *********************************
call "%MsBuildExe%" SharpBatch.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
REM call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe" SharpBatch.NoWeb.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
if not "%errorlevel%"=="0" goto failure

echo *********************************
echo ****  Build ended
echo *********************************

REM Meta Package
echo *********************************
echo ****  Nuget SharpBatch.all
echo *********************************
cd src\SharpBatch.All
call "nuget.exe pack SharpBatch.All.nuspec"
cd ..\..\
echo *********************************
echo ****  Nuget SharpBatch.all Ended
echo *********************************

REM Unit tests
echo *********************************
echo ****  Test Started
echo *********************************
cd test\SharpBatchTest
call dotnet test --configuration %config% --verbosity m
if not "%errorlevel%"=="0" goto failure

cd ..\SharpBatch.Serialization.Xml
call dotnet test --configuration %config% --verbosity m
if not "%errorlevel%"=="0" goto failure

echo *********************************
echo ****  Test ended
echo *********************************

:success
echo success
exit 0

:failure
echo failed 
exit -1
