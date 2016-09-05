@ECHO OFF
REM archive-branch.bat
REM Usage: archive-branch <your-branch-name>

:TopOfScript
ECHO.
ECHO Starting...

IF "%~1"=="" (
  ECHO This command requires 1 input argument.
  ECHO.
  ECHO Usage: archive-branch ^<your-branch-name^>

  GOTO :EndOfScript
)

ECHO.
ECHO * 1. Get local branch from remote, if needed
ECHO.
git checkout %1
IF %ERRORLEVEL% neq 0 GOTO :Err

ECHO.
ECHO * 2. Go back to master
ECHO.
git checkout master
IF %ERRORLEVEL% neq 0 GOTO :Err

ECHO.
ECHO * 3. Create local tag
ECHO.
git tag archive/%1 %1
IF %ERRORLEVEL% neq 0 GOTO :Err

ECHO.
ECHO * 4. Create remote tag
ECHO.
git push origin archive/%1
IF %ERRORLEVEL% neq 0 GOTO :Err

ECHO.
ECHO * 5. Delete local branch
ECHO.
git branch -d %1
IF %ERRORLEVEL% neq 0 GOTO :Err

ECHO.
ECHO * 6. Delete remote branch
ECHO.
git push origin --delete %1
IF %ERRORLEVEL% neq 0 GOTO :Err

GOTO :EndOfScript

:Err
ECHO Errors encountered during execution.  Last command exited with status: %ERRORLEVEL%.

:EndOfScript
ECHO.
ECHO Finished
