#ifndef _ZIGBEE_H
#define _ZIGBEE_H "ZigBee"

#include <cstdlib>
#include <cstring>
#include <iostream>
#include <net/if.h>
#include <sys/ioctl.h>
#include <sys/socket.h>
#include <unistd.h>

#include <xbee/atcmd.h>
#include <xbee/device.h>
#include <xbee/sxa.h>
#include <xbee/wpan.h>

#include <network/Network.hpp>
#include <model/DeviceInfo.hpp>
#include <comm.h>

using namespace std;

namespace wa {
class ZigBee : public Network {

public:
  ZigBee() {}

  bool init() override {
    return false;
  }

  std::string getUserFriendlyName() override { return _ZIGBEE_H; }

  int listen(std::string networkParams) override {
    // deviceConnected(*this);
    return -1;
  }

  Device *connect(std::string conParams) override { return nullptr; }

  // TODO
  std::vector<DeviceInfo> discover() override {
    std::vector<DeviceInfo> v;
    return v;
  }

  ~ZigBee() {
    // deinit
  }
};
} // namespace wa

#endif
