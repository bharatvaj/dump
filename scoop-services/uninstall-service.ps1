$SERVICE_NAME='mpd'
$SERVICE_ARGS='C:\Users\Administrator\.config\mpd\mpd.conf'
$SrvAnyNgBinPath=$(Get-Command -Name srvany-ng -ErrorAction Ignore).Source
$exists=$?
if (-Not $exists ) {
	echo "ERROR: srvany-ng does not exist in path"
	exit
}
$ServiceBinPath=$(Get-Command -Name $SERVICE_NAME -ErrorAction Ignore).Source
$exists=$?
if (-Not $exists ) {
	echo "ERROR: $SERVICE_NAME does not name a service"
	return -1
}
sc.exe create $SERVICE_NAME start= auto binPath= "$SrvAnyNgBinPath"
$NotError=$?
if (-Not $NotError) { 
	# echo "ERROR: Unable to create service"
	# exit /b
	echo "$SERVICE_NAME already exists, old entry will be deleted, are you sure? [Y/n]: Y"
	sc.exe config $SERVICE_NAME start= auto binPath= "$SrvAnyNgBinPath"
}

# TODO if ServiceBinPath is null exit this, possibly rollback

Set-ItemProperty -Path Registry::HKLM\SYSTEM\CurrentControlSet\Services\$SERVICE_NAME\Parameters -Name Application -Value "$ServiceBinPath"
# TODO Use proper AppDir
Set-ItemProperty -Path Registry::HKLM\SYSTEM\CurrentControlSet\Services\$SERVICE_NAME\Parameters -Name AppDirectory -Value "$Env:Scoop\shims"
Set-ItemProperty -Path Registry::HKLM\SYSTEM\CurrentControlSet\Services\$SERVICE_NAME\Parameters -Name AppParameters -Value "$SERVICE_ARGS"
