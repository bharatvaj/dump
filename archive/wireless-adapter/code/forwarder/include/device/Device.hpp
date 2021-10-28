#ifndef _DEVICE_H
#define _DEVICE_H
#include <iostream>
#include <model/DeviceInfo.hpp>
namespace wa {
class Device : public DeviceInfo {
  // bool proxy = true;
public:
  Device() {}
  // disconnects the device from the network
  virtual void disconnect() {}
  virtual int read(char *message, int bytesToRead) = 0;
  virtual int write(char *message, int bytesToWrite) = 0;
};
} // namespace wa
#endif