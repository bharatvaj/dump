#ifndef _ZWAVE_H
#define _ZWAVE_H "ZWave"

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
class ZWave : public Network {
public:
  ZWave() {}

  bool init() override {
    return false;
  }

  std::string getUserFriendlyName() override { return _ZWAVE_H; }

  int listen(std::string networkParams) override {    
    return -1;
  }

  Device *connect(std::string conParams) override { return nullptr; }

  // TODO
  std::vector<DeviceInfo> discover() override {
    std::vector<DeviceInfo> v;
    return v;
  }

  ~ZWave() {
    // deinit
  }
};
} // namespace wa

#endif
