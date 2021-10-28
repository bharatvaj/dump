#ifndef _MEDIUM_H
#define _MEDIUM_H
#include <model/DeviceInfo.hpp>
namespace wa {
class Medium {
public:
  Medium() {}
  virtual std::vector<DeviceInfo> discover() = 0;
  virtual std::string getUserFriendlyName() = 0;
};
} // namespace wa
// Todo Bluetooth Medium
#endif