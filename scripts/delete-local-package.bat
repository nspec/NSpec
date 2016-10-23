@ECHO OFF
REM delete-local-package.bat
REM Usage: delete-local-package <package-directory>

:TopOfScript
ECHO.
ECHO Starting...

IF "%~1"=="" (
  GOTO :Usage
)

SET packageDirPath=%USERPROFILE%\.nuget\packages\%1

IF EXIST %packageDirPath%\NUL (
	ECHO Deleting files from '%packageDirPath%' ...

	DEL /F /S /Q %packageDirPath%

	FOR /F "delims=" %%d IN ('DIR /A:D /B %packageDirPath%') DO RMDIR /S /Q "%packageDirPath%\%%d"
) ELSE (
	ECHO No Files to delete in '%packageDirPath%'.
)

GOTO :EndOfScript

:Usage
ECHO This command requires 1 input argument.
ECHO.
ECHO Usage: delete-local-branch ^<package-directory^>

:EndOfScript
ECHO.
ECHO Finished
