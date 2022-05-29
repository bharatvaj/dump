#include <private/nativewifi.h>

#define WIFI_CTX_NATIVE(CTX)
	((wifi_context_native*) CTX->native_context)


wifi_context* wifi_create_context() {
	wifi_context wifi_ctx = (wifi_context*) malloc(sizeof(wifi_context));
	if(wifi_ctx == NULL) {
		return wifi_ctx;
	}
	wifi_context_native* ctx_native = WIFI_CTX_NATIVE(ctx);
	ctx_native->major_version = WLAN_API_VERSION_MAJOR();
	ctx_native->minor_version = WLAN_API_VERSION_MINOR();
	ctx_native->client_version = WLAN_API_MAKE_VERSION(major_version, minor_version);
	DWORD ret_code = WlanOpenHandle(ctx_native->client_version, NULL, ctx_native->client_version,l
	// didn't handle ERROR_INVALID_PARAMETER explicitly
	switch(ret_code) {
	case ERROR_NOT_ENOUGH_MEMORY:
		return WIFI_ERROR_NOT_ENOUGH_MEMORY;
	case RPC_STATUS:
	case ERROR_REMOTE_SESSION_LIMIT_EXCEEDED:
		return WIFI_ERROR_NATIVE_UNKNOWN;
	}
	return 0;
}

void wifi_destroy_context(wifi_context* wifi_ctx) {
	free(wifi_ctx);
	}

