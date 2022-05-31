#include <swifi.h>

#include <stdio.h>
#include <stddef.h>

/*
 * Enumerate wifi devices
 * Enumerate SSID names
 */


int main(int argc, char *argv[]) {
	swifi_context* ctx = swifi_create_context();
	swifi_error_status err = swifi_check_context(ctx);
	if (err != SWIFI_OK) {
		printf("Failed to create context\n");
		return err;
	}
	swifi_permissions required_permissions = WIFI_PERMISSION_ALL;
	/* swifi_permissions denied_permissions = swifi_get_permission(ctx, required_permissions); */
	/* if (swifi_check_permission(denied_permissions, WIFI_PERMISSION_NONE)) { */
	/* 	printf("You don't have the following permissions to even query the swifi interface, what?"); */
	/* 	return WIFI_PERMISSION_INTERFACE_NAME; */
	/* } */
	swifi_interface** wis = NULL;
	size_t interfaces_len;
   	err = swifi_enumerate_interfaces(ctx, wis, &interfaces_len);
	if (err != SWIFI_OK) {
		printf("Failed to enumerate devices\n");
		return err;
	}
	for(int i = 0; i < interfaces_len; i++) {
		swifi_interface* wi = wis[i];
		if (wi == NULL) {
			printf("One of the enumerated wifi interface is null, disconnected?\n");
			continue;
		}
		printf("Interface Name: %s\n", wi->name);
	}
	return 0;
}
