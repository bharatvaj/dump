#ifndef __SWIFI_PRIVATE_H
#define __SWIFI_PRIVATE_H

#ifdef __SWIFI_ENABLE_NATIVE_WIFI__
#include <private/nativewifi.h>
#endif

swifi_context_native* swifi_create_context_native(swifi_context* swifi_ctx);

void swifi_destroy_context_native(swifi_context* swifi_ctx);

#endif
