#ifndef __WIFI_NATIVE_WIFI
#define __WIFI_NATIVE_WIFI

#include <wifi.h>

#include <wlanapi.h>

typdef struct wifi_context_native {
	DWORD major_version;
	DWORD minor_version;
	DWORD client_version;
} wifi_context_native;


#endif
