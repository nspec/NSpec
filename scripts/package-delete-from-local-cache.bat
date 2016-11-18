@ECHO OFF
REM package-delete-from-local-cache.bat
REM Deletes a named package from NuGet local cache on machine.
REM Usage: package-delete-from-local-cache <package-name>

SET scriptName=package-delete-from-local-cache

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
ECHO Deletes a named package from NuGet local cache on machine.
ECHO.
ECHO Usage: %scriptName% ^<package-name^>

:EndOfScript
ECHO.
ECHO Finished
