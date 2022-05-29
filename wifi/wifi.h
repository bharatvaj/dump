#ifndef __WIFI_H
#define __WIFI_H

enum return_codes {
	WIFI_NO_MEMORY = 1
};

typedef struct wifi_interface {
	char* name;
	size_t name_len;
} wifi_interfaces;

typedef struct wifi_interfaces {
	/* array of wifi_interface */
	wifi_interface* wi;
	/* length of wi array */
	size_t len;
};

/* Bit shifted values that correspond to different permissions */
enum wifi_permissions {
	WIFI_PERMISSION_ALL,
	WIFI_PERMISSION_NONE
	WIFI_PERMISSION_INTERFACE_NAME,
};

/* Steal from CURL, for better API */
enum wifi_error_status {
	WIFI_OK,
	WIFI_ERROR,
	WIFI_ERROR_CONTEXT_NULL,
	WIFI_ERROR_NOT_ENOUGH_MEMORY,
};

typedef struct wifi_context {
	wifi_permissions granted_permissions;
	wifi_permissions denied_permissions;
	int last_error;
	/* Internal use only */
	void *native_object;
} wifi_context;

/*
 * Returns non-null `wifi_context*` if successful
 * and returns NULL on failure. NULL probably means there is no memory,
 * you should panic and exit your program.
 */
wifi_context* wifi_create_context();

void wifi_destroy_context(wifi_context* wifi_ctx);

wifi_code wifi_check_context(wifi_context* wifi_ctx);


/*
 * Checks whether the user has sufficient permission
 * for the given permission list.
 * `wifi_ctx` should not be NULL
 * `required_permissions` should be bit stuffed with
 * permission you require
 * Returns permissions that were denied, if denied_permssion == WIFI_PERMISSION_NONE,
 * all permssion were granted.
 * check manually for every permission before using an interface.
 * if denied_permssion != WIFI_PERMISSION_INTERFACE_NAME
 *  you can query the interfaces.
 */
wifi_permission wifi_get_permission(wifi_context* wifi_ctx, wifi_permissions required_permissions);

wifi_permission wifi_enumerate_interfaces(wifi_context* wifi_ctx, wifi_permissions required_permissions);



#endif
