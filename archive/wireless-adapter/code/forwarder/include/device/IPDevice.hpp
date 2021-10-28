#ifndef _IP_DEVICE_H
#define _IP_DEVICE_H

#include <sys/socket.h>
#include <unistd.h>

#include <net/if.h>
#include <sys/ioctl.h>
#include <sys/socket.h>
#include <unistd.h>

#include <comm.h>

#include <device/Device.hpp>

namespace wa {
class IPDevice : Device {
private:
  xs_SOCKET fd;

public:
  IPDevice(xs_SOCKET fd) : fd(fd) {}

  int read(char *str, int bytesToRead) override {
    return ::read(fd, str, bytesToRead);
  }

  int write(char *str, int bytesToSend) override {
    return ::write(fd, str, bytesToSend);
  }

  void disconnect() override {
    ::close(fd);
  }

  ~IPDevice() { comm_close_socket(fd); }
};
} // namespace wa
#endif