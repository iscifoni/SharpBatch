@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%BuildCounter%" == "" (
   set version=--version-suffix ci-%BuildCounter%
)

REM (optional) build.bat is in the root of our repo, cd to the correct folder where sources/projects are
REM cd MyLibrary

REM Restore
call dotnet restore
if not "%errorlevel%"=="0" goto failure


REM Build
REM - Option 1: Run dotnet build for every source folder in the project
REM   e.g. call dotnet build <path> --configuration %config%
REM - Option 2: Let msbuild handle things and build the solution
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" SharpBatch.NoWeb.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
REM call dotnet build --configuration %config%
if not "%errorlevel%"=="0" goto failure

REM Unit tests
REM call dotnet test tests\MyLibrary.Tests --configuration %config%
REM if not "%errorlevel%"=="0" goto failure

REM Package
mkdir %cd%\Artifacts

call dotnet pack src\SharpBatch --configuration %config% %version% --output Artifacts
if not "%errorlevel%"=="0" goto failure

call dotnet pack src\SharpBatch.Traking.Abstraction --configuration %config% %version% --output Artifacts
if not "%errorlevel%"=="0" goto failure

call dotnet pack src\SharpBatch.Traking.Memory --configuration %config% %version% --output Artifacts
if not "%errorlevel%"=="0" goto failure

:success
exit 0

:failure
exit -1