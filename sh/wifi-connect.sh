#!/bin/sh

WIFI_SSID=Home
WIFI_PASS=Home

# linux
if iwctl wlan0 connect "${WIFI_SSID}" "${WIFI_PASS}"; then
	exit 0
fi

# mac
if networksetup -setairportnetwork en0 "${WIFI_SSID}" "${WIFI_PASS}"; then
	exit 0
fi
# win
if netsh wlan connect "${WIFI_SSID}" "${WIFI_PASS}"; then
	exit 0
fi
