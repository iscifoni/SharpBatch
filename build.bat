@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)

REM Detect MSBuild 15.0 path
if exist "%programfiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe" (
    set msbuild="%programfiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"
)
if exist "%programfiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe" (
    set msbuild="%programfiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\MSBuild.exe"
)
if exist "%programfiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe" (
    set msbuild="%programfiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MSBuild.exe"
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

call "%msbuild%" MyLibrary.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

REM call dotnet build SharpBatch.NoWeb.sln --configuration %config%

if not "%errorlevel%"=="0" goto failure

REM Unit tests
cd test\SharpBatchTest
call dotnet test --configuration %config% -verbose
cd ..\..\
if not "%errorlevel%"=="0" goto failure

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
