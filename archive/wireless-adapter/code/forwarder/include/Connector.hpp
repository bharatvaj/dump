#ifndef _CONNECTOR_H
#define _CONNECTOR_H

#include <functional>
#include <iostream>
#include <vector>

#include <network/Network.hpp>

using namespace std;

namespace wa {
class Connector {

private:
  // onAddedSuccessfully
  std::function<void(Network &n)> listener;
  std::map<std::string, Network *> networks;

public:
  Connector(std::map<std::string, Network *> networks) : networks(networks) {}
};
} // namespace wa

#endif
