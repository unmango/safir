@echo off

REM https://github.com/dotnet/core-sdk/blob/master/eng/setbuildinfo.bat

setlocal

set Architecture=%1
set Config=%2

if "%AdditionalBuildParameters%" == "$(_AdditionalBuildParameters)" (
    REM Prevent the literal "$(_AdditionalBuildParameters)" to be passed to the build script
    ECHO Setting AdditionalBuildParameters to empty
    ECHO ##vso[task.setvariable variable=AdditionalBuildParameters]
) ELSE (
    ECHO AdditionalBuildParameters is already set to: %AdditionalBuildParameters%
)
