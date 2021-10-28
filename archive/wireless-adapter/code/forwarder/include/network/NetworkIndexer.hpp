#ifndef _NET_INDEXER_H
#define _NET_INDEXER_H
#include <network/Network.hpp>

#include <network/Bluetooth.hpp>
#include <network/Console.hpp>
#include <network/IP.hpp>
#include <network/ZigBee.hpp>
#include <network/ZWave.hpp>

namespace wa {
class NetworkIndexer {
public:
  static std::vector<Network *> getNetworks() {
    std::vector<Network *> networks;
    networks.push_back(new Bluetooth());
    networks.push_back(new Console());
    networks.push_back(new IP());
    networks.push_back(new ZigBee());
    networks.push_back(new ZWave());
    return networks;
  }
};
} // namespace wa
#endif
