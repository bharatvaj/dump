#include <wifi.h>

/*
 * Enumerate wifi devices
 * Enumerate SSID names
 */


int main(int argc, char *argv[]) {
	wifi_context* ctx = wifi_create_context();
	wifi_error_status err = wifi_check_context(ctx);
	if (err != WIFI_OK) {
		printf("Failed to create context\n");
	}
	wifi_permissions required_permissions = WIFI_PERMISSION_ALL;
	wifi_permissions denied_permissions = wifi_get_permission(ctx, required_permissions);
	if (wifi_check_permission(denied_permissions, WIFI_PERMISSION_NONE)) {
		printf("You don't have the following permissions to even query the wifi interface, what?");
		return WIFI_PERMISSION_INTERFACE_NAME;
	}
	wifi_interfaces* wi = wifi_enumerate_interfaces(ctx);
	for(int i = 0; i < wi->len; i++) {
		wifi_interface* wi = wifi_interface->interface;
		if (wi == NULL) {
			printf("One of the enumerated wifi interface is null, disconnected?\n");
			continue;
		}
		printf("Interface Name: %s\n", wi->name);
	}
	return 0;
}
