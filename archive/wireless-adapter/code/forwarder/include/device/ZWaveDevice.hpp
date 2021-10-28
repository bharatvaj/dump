#ifndef _ZWAVE_DEV_H
#define _ZWAVE_DEV_H

#include <Device.hpp>

namespace wa {
class ZWaveDevice : Device {

public:
  ZWaveDevice() {}

  // TODO test
  int write(char *msg, int bytesToSend) override { return -1; }

  // TODO test
  int read(char *msg, int bytesToRead) override {
    // zwave recv
    return -1;
  }
};
} // namespace wa
#endif