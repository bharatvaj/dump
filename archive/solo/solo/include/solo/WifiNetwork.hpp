#ifndef _WIFI_NETWORK
#define _WIFI_NETWORK "WifiNetwork"
#include <solo/Network.hpp>
#include <solo/Query.hpp>

namespace solo {
class WifiNetwork : public Network {
public:
  WifiNetwork();
  /*
   * This WifiNetwork class only supports subnet scanning
   * unless explicitly specifying the query with a global IP
   * 
   */
  void scan(Query) override;
};
}
#endif
