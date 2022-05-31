#include <swifi.h>

#include <stddef.h>

#include <private/swifiprivate.h>

/**
 * This file contains common functions
 */

swifi_error_status swifi_check_context(swifi_context* ctx) {
	if (ctx == NULL) {
		return SWIFI_ERROR_CONTEXT_NULL;
	}
	return ctx->last_error;
}

swifi_context* swifi_create_context() {
	swifi_context *swifi_ctx = (swifi_context*) malloc(sizeof(swifi_context));
	swifi_ctx->last_error = SWIFI_OK;
	if(swifi_ctx == NULL) {
		return swifi_ctx;
	}
	swifi_ctx->native_context = swifi_create_context_native(swifi_ctx);
	swifi_ctx->last_error = swifi_ctx->native_context->last_error;
	return swifi_ctx;
}

void swifi_destroy_context(swifi_context* swifi_ctx) {
	swifi_error_status err = swifi_destroy_context_native(swifi_ctx);
	if (err != SWIFI_OK) {
		// native context destroy failed, check log for reasons
	}
	free(swifi_ctx);
}
