#include <wifi.h>

/*
 * Enumerate wifi devices
 * Enumerate SSID names
 */

int main(int argc, char*argv[]) {
	wifi_interfaces* wi = wifi_enumerate_devices();
	for(int i = 0; i < wi->len; i++) {
		wifi_interface* wi = wifi_interface->interface;
		printf("Interface Name: %s\n", wi->name);
	}
}
