#include <swifi.h>

#include <stddef.h>

#include <private/swifiprivate.h>

/**
 * This file contains common functions
 */

swifi_error_status swifi_check_context(swifi_context* ctx) {
	if (ctx == NULL) {
		return WIFI_ERROR_CONTEXT_NULL;
	}
	return ctx->last_error;
}

swifi_context* swifi_create_context() {
	swifi_context *swifi_ctx = (swifi_context*) malloc(sizeof(swifi_context));
	swifi_ctx->last_error = WIFI_OK;
	if(swifi_ctx == NULL) {
		return swifi_ctx;
	}
	swifi_ctx->native_context = swifi_create_context_native(swifi_ctx);
	return swifi_ctx;
}

void swifi_destroy_context(swifi_context* swifi_ctx) {
	swifi_destroy_context_native(swifi_ctx);
	free(swifi_ctx);
}
