@ECHO OFF
REM move-to-local-repo.bat.bat
REM Usage: move-to-local-repo.bat <project-path> <local-repo-path>

:TopOfScript
ECHO.
ECHO Starting...

IF "%~1"=="" (
  GOTO :Usage
)

IF "%~2"=="" (
  GOTO :Usage
)

SET projectPath=%~1
SET localRepoPath=%~2

IF NOT EXIST %projectPath%\NUL (
	ECHO No project directory found at given path '%projectPath%'.
  GOTO :EndOfScript
)

IF NOT EXIST %localRepoPath%\NUL (
	ECHO No local repository directory found at given path '%localRepoPath%'.
  GOTO :EndOfScript
)

SET outputDirRelativePath=bin\Debug
SET packageDirPath=%projectPath%\%outputDirRelativePath%

IF NOT EXIST %packageDirPath%\NUL (
	ECHO No package output directory found at given path '%packageDirPath%'.
  GOTO :EndOfScript
)

ECHO Moving packages from '%packageDirPath%' to '%localRepoPath%' ...

MOVE /Y "%packageDirPath%\*.nupkg" "%localRepoPath%"

GOTO :EndOfScript

:Usage
ECHO This command requires 2 input arguments.
ECHO.
ECHO Usage: move-to-local-repo.bat ^<project-path^> ^<local-repo-path^>

:EndOfScript
ECHO.
ECHO Finished
