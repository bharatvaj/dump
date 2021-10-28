#ifndef _CONSOLE_H
#define _CONSOLE_H "Console"

#include <iostream>
#include <network/Network.hpp>
#include <vector>

#include <device/ConsoleDevice.hpp>
#include <model/DeviceInfo.hpp>

namespace wa {
class Console : public Network {
public:
  Console() {}

  bool init() override { return true; }

  std::string getUserFriendlyName() override { return _CONSOLE_H; }

  int listen(std::string networkParams) override {
    Device *d = (Device *)new ConsoleDevice();
    devices.push_back(d);
    deviceConnected(*d);
    return 0;
  }

  int bind(std::string bindParams) override {
    return 0;
  }

  int unbind(std::string bindParams) override {
    return 0;
  }

  // TODO
  Device *connect(std::string conParam) override {
    Device *d = new ConsoleDevice();
    d->address = conParam;
    d->network = this;
    devices.push_back((Device *)new ConsoleDevice());
    return d;
  }

  std::vector<DeviceInfo> discover() override {
    devices.push_back((Device *)new ConsoleDevice());
    return std::vector<DeviceInfo>();
  }
};
} // namespace wa
#endif
