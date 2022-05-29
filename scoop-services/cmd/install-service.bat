@echo off
set SERVICE_NAME=mpd
set SERVICE_ARGS=C:\Users\Administrator\.config\mpd\mpd.conf
where /q srvany-ng
if ERRORLEVEL 1 (
	echo ERROR: srvany-ng does not exist in path
	exit /b
)
where /q %SERVICE_NAME%
if ERRORLEVEL 1 (
	echo ERROR: %SERVICE_NAME% does not name a service
	exit /b
)
echo "%~dp0srvany-ng.exe"
sc create mpd start= auto binPath= "%~dp0srvany-ng.exe"
if ERRORLEVEL 1 (
	:: echo "ERROR: Unable to create service"
	:: exit /b
	echo %SERVICE_NAME% already exists, old entry will be deleted, are you sure? [Y/n]: Y
	sc delete mpd
	sc create mpd start= auto binPath= "%~dp0srvany-ng.exe"
)

reg add HKLM\SYSTEM\CurrentControlSet\Services\%SERVICE_NAME%\Parameters /v Application /f /d "%~dp0%SERVICE_NAME%.exe"
reg add HKLM\SYSTEM\CurrentControlSet\Services\%SERVICE_NAME%\Parameters /v AppDirectory /f /d "%~dp0."
reg add HKLM\SYSTEM\CurrentControlSet\Services\%SERVICE_NAME%\Parameters /v AppParameters /f /d "%SERVICE_ARGS%"
