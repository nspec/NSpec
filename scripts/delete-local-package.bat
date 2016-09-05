@ECHO OFF
REM delete-local-package.bat
REM Usage: delete-local-package <package-directory>

:TopOfScript
ECHO.
ECHO Starting...

IF "%~1"=="" (
  ECHO This command requires 1 input argument.
  ECHO.
  ECHO Usage: delete-local-branch ^<package-directory^>

  GOTO :EndOfScript
)

DEL /F /S /Q %USERPROFILE%\.nuget\packages\%1

:EndOfScript
ECHO.
ECHO Finished
