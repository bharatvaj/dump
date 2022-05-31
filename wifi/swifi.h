#ifndef __WIFI_H
#define __WIFI_H

/* Forward declaration, this is for internal use */
typedef struct swifi_context_native swifi_context_native;

enum return_codes {
	WIFI_NO_MEMORY = 1
};

typedef struct swifi_interface {
	char* name;
	size_t name_len;
} swifi_interface;

/* Bit shifted values that correspond to different permissions */
typedef enum swifi_permissions {
	WIFI_PERMISSION_ALL,
	WIFI_PERMISSION_NONE,
	WIFI_PERMISSION_INTERFACE_NAME
} swifi_permissions;

/* Steal from CURL, for better API */
typedef enum swifi_error_status {
	SWIFI_OK,
	SWIFI_ERROR,
	SWIFI_ERROR_CONTEXT_NULL,
	SWIFI_ERROR_UNEXPECTED,
	SWIFI_ERROR_NOT_ENOUGH_MEMORY,
} swifi_error_status;

typedef struct swifi_context {
	swifi_permissions granted_permissions;
	swifi_permissions denied_permissions;
	swifi_error_status last_error;
	/* Internal use only */
	struct swifi_context_native *native_context;
} swifi_context;

/*
 * Returns non-null `swifi_context*` if successful
 * and returns NULL on failure. NULL probably means there is no memory,
 * you should panic and exit your program.
 */
swifi_context* swifi_create_context();

void swifi_destroy_context(swifi_context* swifi_ctx);

swifi_error_status swifi_check_context(swifi_context* swifi_ctx);

/*
 * Checks whether the user has sufficient permission
 * for the given permission list.
 * `swifi_ctx` should not be NULL
 * `required_permissions` should be bit stuffed with
 * permission you require
 * Returns permissions that were denied, if denied_permssion == WIFI_PERMISSION_NONE,
 * all permssion were granted.
 * check manually for every permission before using an interface.
 * if denied_permssion != WIFI_PERMISSION_INTERFACE_NAME
 *  you can query the interfaces.
 */
swifi_permissions swifi_get_permission(swifi_context* swifi_ctx, swifi_permissions required_permissions);

/**
 * Returns a list of interfaces to enumerat
 */
swifi_error_status swifi_enumerate_interfaces(swifi_context* swifi_ctx, swifi_interface** swifi_interfaces, size_t* swifi_interfaces_len);



#endif
