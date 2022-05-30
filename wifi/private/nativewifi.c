#include <private/nativewifi.h>

#include <windows.h>
#include <wlanapi.h>

#include <objbase.h>
#include <wtypes.h>
#include <wchar.h>

// Need to link with Wlanapi.lib and Ole32.lib
#pragma comment(lib, "wlanapi.lib")
#pragma comment(lib, "ole32.lib")
/* #include <clog/clog.h> */

#define WIFI_CTX_NATIVE(CTX) \
	(swifi_context_native*) CTX->native_context

/*
 * Updates error code from swifi_context_native to wifi_context
 */
#define WIFI_CTX_UPDATE_ERROR_CODE(CTX_NATIVE) \
	switch(CTX_NATIVE->last_ret_code) { \
	case ERROR_NOT_ENOUGH_MEMORY: \
		CTX_NATIVE->last_error = WIFI_ERROR_NOT_ENOUGH_MEMORY; \
		/* clog_info(L"WlanOpenHandle failed with error: %u\n", dwResult); */ \
		break; \
	/* case RPC_STATUS: */ \
	/*didn't handle ERROR_INVALID_PARAMETER explicitly*/ \
	case ERROR_REMOTE_SESSION_LIMIT_EXCEEDED: \
		CTX_NATIVE->last_error = WIFI_ERROR_NOT_ENOUGH_MEMORY; \
		break; \
	}

swifi_context_native* swifi_create_context_native(const swifi_context* swifi_ctx) {
	swifi_context_native* ctx_native = (swifi_context_native*) malloc(sizeof(swifi_context_native));
	ctx_native->major_version = WLAN_API_VERSION_MAJOR(2);
	ctx_native->minor_version = WLAN_API_VERSION_MINOR(0);
	ctx_native->client_version = WLAN_API_MAKE_VERSION(ctx_native->major_version, ctx_native->minor_version);
	/* ctx_native->client_handle = NULL; */
	ctx_native->last_ret_code = WlanOpenHandle(ctx_native->client_version, NULL, &ctx_native->negotiated_version, &ctx_native->client_handle);
	WIFI_CTX_UPDATE_ERROR_CODE(ctx_native);
	return ctx_native;
}

void swifi_destroy_context_native(swifi_context* swifi_ctx) {
	swifi_context_native* ctx_native = WIFI_CTX_NATIVE(swifi_ctx);
	DWORD ret_code = WlanCloseHandle(ctx_native->client_handle, NULL);
	// tech-debt should we return ret_code?
}


swifi_error_status swifi_enumerate_interfaces(swifi_context* swifi_ctx, swifi_interface** swifi_interfaces, size_t* swifi_interfaces_len) {
	swifi_context_native* ctx_native = WIFI_CTX_NATIVE(swifi_ctx);
    PWLAN_INTERFACE_INFO_LIST pIfList = NULL;
    PWLAN_INTERFACE_INFO pIfInfo = NULL;
    WCHAR GuidString[40] = {0};
    int iRet = 0;
	int i = 0;
    ctx_native->last_ret_code = WlanEnumInterfaces(ctx_native->client_handle, NULL, &pIfList);
	WIFI_CTX_UPDATE_ERROR_CODE(ctx_native);
     wprintf(L"Current Index: %lu\n", pIfList->dwIndex);
        for (i = 0; i < (int) pIfList->dwNumberOfItems; i++) {
            pIfInfo = (WLAN_INTERFACE_INFO *) &pIfList->InterfaceInfo[i];
            wprintf(L"  Interface Index[%d]:\t %lu\n", i, i);
            /* iRet = StringFromGUID2(pIfInfo->InterfaceGuid, (LPOLESTR) &GuidString, 39); */
            // For c rather than C++ source code, the above line needs to be
            iRet = StringFromGUID2(&pIfInfo->InterfaceGuid, (LPOLESTR) &GuidString, 39);
            if (iRet == 0) {
                wprintf(L"StringFromGUID2 failed\n");
			} else {
                wprintf(L"  InterfaceGUID[%d]: %ws\n",i, GuidString);
            }
            wprintf(L"  Interface Description[%d]: %ws", i,pIfInfo->strInterfaceDescription);
            wprintf(L"\n");
            wprintf(L"  Interface State[%d]:\t ", i);
            switch (pIfInfo->isState) {
            case wlan_interface_state_not_ready:
                wprintf(L"Not ready\n");
                break;
            case wlan_interface_state_connected:
                wprintf(L"Connected\n");
                break;
            case wlan_interface_state_ad_hoc_network_formed:
                wprintf(L"First node in a ad hoc network\n");
                break;
            case wlan_interface_state_disconnecting:
                wprintf(L"Disconnecting\n");
                break;
            case wlan_interface_state_disconnected:
                wprintf(L"Not connected\n");
                break;
            case wlan_interface_state_associating:
                wprintf(L"Attempting to associate with a network\n");
                break;
            case wlan_interface_state_discovering:
                wprintf(L"Auto configuration is discovering settings for the network\n");
                break;
            case wlan_interface_state_authenticating:
                wprintf(L"In process of authenticating\n");
                break;
            default:
                wprintf(L"Unknown state %ld\n", pIfInfo->isState);
                break;
            }
            wprintf(L"\n");
        }

    if (pIfList != NULL) {
        WlanFreeMemory(pIfList);
        pIfList = NULL;
    }
	return swifi_ctx->last_error;
}
