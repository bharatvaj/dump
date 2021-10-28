#ifndef _ZIGBEE_DEV_H
#define _ZIGBEE_DEV_H

#include <Device.hpp>

namespace wa {
class ZigBeeDevice : Device {

public:
  ZigBeeDevice() {}

  // TODO test
  int write(char *msg, int bytesToSend) override { return -1; }

  // TODO test
  int read(char *msg, int bytesToRead) override {
    // zigbee recv
    return -1;
  }
};
} // namespace wa
#endif