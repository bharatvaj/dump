#ifndef _BLUETOOTH_H
#define _BLUETOOTH_H
#include <thread>
#include <bluetooth/bluetooth.h>
#include <bluetooth/rfcomm.h>
#include <crosssocket.h>
#include <functional>
bdaddr_t inc = ((bdaddr_t){{0, 0, 0, 0, 0, 0}});

typedef std::function<void(xs_SOCKET)> OnDeviceAdded;

class Bluetooth {
public:
  Bluetooth() {}
  OnDeviceAdded onDeviceAdded;

  void setOnDeviceConnected(OnDeviceAdded onDeviceAdded) {
    this->onDeviceAdded = onDeviceAdded;
  }
  bool startSlave() {

    struct sockaddr_rc loc_addr = {0}, rem_addr = {0};
    xs_SOCKET s;
    socklen_t opt = sizeof(rem_addr);

    // allocate socket
    s = xs_socket(AF_BLUETOOTH, SOCK_STREAM, BTPROTO_RFCOMM);
    if (s < 0) {
      return false;
    }

    // bind socket to port 1 of the first available
    // local bluetooth adapter
    loc_addr.rc_family = AF_BLUETOOTH;
    loc_addr.rc_bdaddr = inc;
    loc_addr.rc_channel = (uint8_t)1;
    if (bind(s, (struct sockaddr *)&loc_addr, sizeof(loc_addr)) == -1) {
      return false;
    }

    // put socket into listening mode
    if (::listen(s, 1) == -1) {
      return false;
    }

    // accept one connection
new std::thread([&](int so){
    xs_SOCKET c = accept(so, (struct sockaddr *)&rem_addr, &opt);

    close(s);
      try {
    if (c < 0) {
	perror("socket\n");
      onDeviceAdded(-1);
	return;
    }
        onDeviceAdded(c);
      } catch (std::bad_function_call bfc) {
      }

}, s);

    return true;
  }
};
#endif
