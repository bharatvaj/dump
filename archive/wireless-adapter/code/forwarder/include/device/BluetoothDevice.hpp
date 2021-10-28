#ifndef _BLUETOOTH_DEV_H
#define _BLUETOOTH_DEV_H

#include <device/Device.hpp>

#include <bluetooth/bluetooth.h>
#include <bluetooth/hci.h>
#include <bluetooth/hci_lib.h>
#include <bluetooth/rfcomm.h>
#include <iostream>
#include <stdio.h>
#include <sys/socket.h>
#include <unistd.h>

#include <crosssocket.h>

namespace wa {
class BluetoothDevice : Device {
private:
  xs_SOCKET s;
  std::string name;
  //address

public:
  BluetoothDevice(xs_SOCKET s) : s(s) {}

  int write(char *str, int bytesToSend) override {
    return ::write(s, str, bytesToSend);
  }

  int read(char *str, int bytesToRead) override {
    return ::read(s, str, bytesToRead);
  }
};
} // namespace wa
#endif
