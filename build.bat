@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%BuildCounter%" == "" (
   set packversionsuffix= ci-%BuildCounter%
)
echo *****************************************************************************************************************
echo ****  packversionsuffix --> %packversionsuffix% 
echo *****************************************************************************************************************

REM (optional) build.bat is in the root of our repo, cd to the correct folder where sources/projects are
REM cd MyLibrary

REM Restore
echo *********************************
echo ****  Restoring package
echo *********************************

REM call dotnet restore SharpBatch.NoWeb.sln
REM if not "%errorlevel%"=="0" goto failure

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
call "%MsBuildExe%" SharpBatch.NoWeb.sln /t:restore /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
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
call dotnet test --configuration %config% -verbose

cd ..\SharpBatch.Serialization.Xml
call dotnet test --configuration %config% -verbose

echo *********************************
echo ****  Test ended
echo *********************************

cd ..\..\
if not "%errorlevel%"=="0" goto failure

REM Package

mkdir %cd%\Artifacts

call dotnet pack src\SharpBatch --configuration %config% %version% --output Artifacts
if not "%errorlevel%"=="0" goto failure

call dotnet pack src\SharpBatch.Traking.Abstraction --configuration %config% %version% --output Artifacts
if not "%errorlevel%"=="0" goto failure

call dotnet pack src\SharpBatch.Traking.Abstraction --configuration %config% %version% --output Artifacts
if not "%errorlevel%"=="0" goto failure

call dotnet pack src\SharpBatch.Traking.Memory --configuration %config% %version% --output Artifacts
if not "%errorlevel%"=="0" goto failure

call dotnet pack src\SharpBatch.Tracking.DB --configuration %config% %version% --output Artifacts
if not "%errorlevel%"=="0" goto failure

:success
exit 0

:failure
exit -1
