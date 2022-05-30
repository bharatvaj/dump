#ifndef __SWIFI_NATIVE_WIFI
#define __SWIFI_NATIVE_WIFI

#include <swifi.h>

#include <windows.h>
#include <swifi.h>

typedef struct swifi_context_native {
	DWORD major_version;
	DWORD minor_version;
	DWORD client_version;
	DWORD negotiated_version;
	HANDLE client_handle;
	DWORD last_ret_code;
	swifi_error_status last_error;
} swifi_context_native;


#endif
