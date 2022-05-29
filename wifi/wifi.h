#ifndef __WIFI_H
#define __WIFI_H
typedef struct wifi_interface {
	char* name;
	size_t name_len;
} wifi_interfaces;

struct wifi_interfaces {
	/* array of wifi_interface */
	wifi_interface* wi;
	/* length of wi array */
	size_t len;
};



#endif
