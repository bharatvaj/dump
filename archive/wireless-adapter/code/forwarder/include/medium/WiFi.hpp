#ifndef _WIFI_H
#define _WIFI_H "WiFi"
#include <Medium.hpp>
#include <iostream>
#include <vector>
#include <wifi_scan.h>
#include <model/NetworkInfo.hpp>
namespace wa {
class WiFi : Medium {
public:
  std::vector<NetworkInfo> discover() override {
    std::vector<NetworkInfo> v;
    int status, i;
    struct bss_info bss[10];
    //FIXME add dependency to network
    struct wifi_scan *wifi = wifi_scan_init("wlan0");
    status = wifi_scan_all(wifi, bss, 10);
    for (i = 0; i < status && i < 10; ++i) {
      NetworkInfo ni;
      ni.address = bss[i].ssid;
      v.push_back(ni);
    }
    wifi_scan_close(wifi);
    return v;
  }

  std::string getUserFriendlyName() override { return _WIFI_H; }
};
} // namespace wa
#endif