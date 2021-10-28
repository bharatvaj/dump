#ifndef _NETWORK_H
#define _NETWORK_H
#include <functional>
#include <vector>

#include <device/Device.hpp>
#include <model/DeviceInfo.hpp>

namespace wa {
class Network {
protected:
  std::string networkParams;
  std::function<void(Device &)> deviceConnected;
  std::function<void(Device &)> deviceDisconnected;
  std::vector<Device *> devices;

public:
  Network() {}
  virtual ~Network() {}
  virtual void
  setOnDeviceConnected(std::function<void(Device &)> deviceConnected) final {
    this->deviceConnected = deviceConnected;
  }
  virtual void setOnDeviceDisconnected(
      std::function<void(Device &)> deviceDisconnected) final {
    this->deviceDisconnected = deviceDisconnected;
  }
  virtual void setNetworkParams(std::string networkParams) {
    this->networkParams = networkParams;
  }
  virtual std::string getNetworkParams() final {
    return this->networkParams;
  }
  //
  virtual bool init() = 0;
  // returns a human readable user friendly name
  virtual std::string getUserFriendlyName() = 0;
  // listen
  virtual int listen(std::string) = 0;
  // connects to the given address
  virtual Device *connect(std::string) = 0;
  // bind a route to system
  virtual int bind(std::string) {
    return -1;
  }
  // unbind a route from system
  virtual int unbind(std::string) {
    return -1;
  }
  // discovers different networks
  virtual std::vector<DeviceInfo> discover() = 0;
  // should be deprecated
  std::vector<Device *> getDevices() { return devices; }
};
}; // namespace wa
#endif
