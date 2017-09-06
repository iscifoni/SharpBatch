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
if not "%BuildCounter%" == "" (
   set packversionsuffix= ci-%BuildCounter%
)
set PackageVersion=1.0.0
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
REM - Option 1: Run dotnet build for every source folder in the project
REM   e.g. call dotnet build <path> --configuration %config%
REM - Option 2: Let msbuild handle things and build the solution
echo *********************************
echo ****  Build started
echo *********************************
REM C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin
call "%MsBuildExe%" SharpBatch.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
REM call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe" SharpBatch.NoWeb.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

REM call dotnet build SharpBatch.NoWeb.sln --configuration %config%
echo *********************************
echo ****  Build ended
echo *********************************

if not "%errorlevel%"=="0" goto failure

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

cd ..\..\

REM Package
REM mkdir %cd%\Artifacts

REM call dotnet pack src\SharpBatch --configuration %config% %version% --output Artifacts
REM if not "%errorlevel%"=="0" goto failure

REM call dotnet pack src\SharpBatch.Traking.Abstraction --configuration %config% %version% --output Artifacts
REM if not "%errorlevel%"=="0" goto failure

REM call dotnet pack src\SharpBatch.Traking.Abstraction --configuration %config% %version% --output Artifacts
REM if not "%errorlevel%"=="0" goto failure

REM call dotnet pack src\SharpBatch.Traking.Memory --configuration %config% %version% --output Artifacts
REM if not "%errorlevel%"=="0" goto failure

REM call dotnet pack src\SharpBatch.Tracking.DB --configuration %config% %version% --output Artifacts
REM if not "%errorlevel%"=="0" goto failure

:success
exit 0

:failure
exit -1
