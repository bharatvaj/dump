#ifndef __SWIFI_PRIVATE_H
#define __SWIFI_PRIVATE_H

#include <swifi.h>
#ifdef __SWIFI_ENABLE_NATIVE_WIFI__
#include <private/nativewifi.h>
#endif

swifi_context_native* swifi_create_context_native(const swifi_context* swifi_ctx);

/*
 * Cleans up wifi interfaces structs by calling and frees up
 * swifi_ctx->native_context
 * Returns an expliciti `swifi_error_status` as the `swifi_context_native` is freed
 */
swifi_error_status swifi_destroy_context_native(const swifi_context* swifi_ctx);

#endif
