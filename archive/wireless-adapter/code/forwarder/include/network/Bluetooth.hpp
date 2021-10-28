#ifndef _BLUETOOTH_H
#define _BLUETOOTH_H "Bluetooth"

#include <bluetooth/bluetooth.h>
#include <bluetooth/hci.h>
#include <bluetooth/hci_lib.h>
#include <bluetooth/rfcomm.h>
#include <iostream>
#include <stdio.h>
#include <sys/socket.h>
#include <unistd.h>

#include <device/BluetoothDevice.hpp>
#include <model/DeviceInfo.hpp>
#include <network/Network.hpp>

namespace wa {
class Bluetooth : public Network {
private:
  bdaddr_t inc = ((bdaddr_t){{0, 0, 0, 0, 0, 0}});

public:
  Bluetooth() {}

  bool init() override {
    int s = socket(AF_BLUETOOTH, SOCK_STREAM, BTPROTO_RFCOMM);
    close(s);
    return !(s < 0);
  }

  std::string getUserFriendlyName() override { return _BLUETOOTH_H; }

  int listen(std::string networkParams) override {
    struct sockaddr_rc loc_addr = {0}, rem_addr = {0};
    xs_SOCKET s;
    socklen_t opt = sizeof(rem_addr);

    // allocate socket
    s = socket(AF_BLUETOOTH, SOCK_STREAM, BTPROTO_RFCOMM);
    if (s < 0) {
      throw s;
    }

    // bind socket to port 1 of the first available
    // local bluetooth adapter
    loc_addr.rc_family = AF_BLUETOOTH;
    loc_addr.rc_bdaddr = inc;
    loc_addr.rc_channel = (uint8_t)6;
    ::bind(s, (struct sockaddr *)&loc_addr, sizeof(loc_addr));

    // put socket into listening mode
    ::listen(s, 1);

    // accept one connection
    xs_SOCKET c = accept(s, (struct sockaddr *)&rem_addr, &opt);
    Device *d = (Device *)new BluetoothDevice(c);
    d->address = "B8:27:EB:A5:A1:70";
    devices.push_back(d);
    close(s);
    if (c == xs_ERROR) {
      return -1;
    }
    deviceConnected(*d);
    return 0;
  }

  Device *connect(std::string conParams) override {
    struct sockaddr_rc addr = {0};
    xs_SOCKET s;
    const char *dest = conParams.c_str();
    // allocate a socket
    s = socket(AF_BLUETOOTH, SOCK_STREAM, BTPROTO_RFCOMM);

    // set the connection parameters (who to connect to)
    addr.rc_family = AF_BLUETOOTH;
    addr.rc_bdaddr = inc;
    addr.rc_channel = (uint8_t)1;
    str2ba(dest, &addr.rc_bdaddr);

    // connect to server
    int status = ::connect(s, (struct sockaddr *)&addr, sizeof(addr));
    Device *d = (Device *)new BluetoothDevice(s);
    perror("socket");
    if(status < 0){
	    return nullptr;
    }
    return d;
  }

  int bind(std::string bindParams) override { 
    return -1; 
    }

  int unbind(std::string bindParams) override {
    return -1;
  }

  // TODO
  std::vector<DeviceInfo> discover() override {
    std::vector<DeviceInfo> values;
    //name , mac, ......BluetoothDevice
    inquiry_info *ii = NULL;
    int max_rsp, num_rsp;
    int dev_id, sock, len, flags;
    int i;
    char addr[19] = {0};
    char name[248] = {0};

    dev_id = hci_get_route(NULL);
    sock = hci_open_dev(dev_id);
    if (dev_id < 0 || sock < 0) {
      perror("opening socket");
      return std::vector<DeviceInfo>();
    }

    len = 8;
    max_rsp = 255;
    flags = IREQ_CACHE_FLUSH;
    ii = (inquiry_info *)malloc(max_rsp * sizeof(inquiry_info));

    num_rsp = hci_inquiry(dev_id, len, max_rsp, NULL, &ii, flags);
    if (num_rsp < 0)
      perror("hci_inquiry");

    for (i = 0; i < num_rsp; i++) {
      ba2str(&(ii + i)->bdaddr, addr);
      hci_read_remote_name(sock, &(ii + i)->bdaddr, sizeof(name), name, 0);
      DeviceInfo ni;
      ni.name = name;
      values.push_back(ni);
    }

    free(ii);
    close(sock);
    return values;
  }

  ~Bluetooth() {
    // deinit
  }
};
} // namespace wa
#endif
