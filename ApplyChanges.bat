rem This script should be run everytime when contents of the ModData directory changes

@echo off

rem Set current working directory to directory where this script is located
cd /d "%~dp0"

rem Initialize variables
set "BANNERLORD_PATH=C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules"
set "MOD_NAME=Accompany"
set "MOD_DIRECTORY=%BANNERLORD_PATH%\%MOD_NAME%"
set "MOD_DATA=ModData"

rem Copy mod data into mod directory, which is in Bannerlord Modules directory (only newer files)
xcopy "%MOD_DATA%\" "%MOD_DIRECTORY%\" /d /e /y
