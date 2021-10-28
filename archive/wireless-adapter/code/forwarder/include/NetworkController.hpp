#ifndef _NETWORK_CONTROLLER_H
#define _NETWORK_CONTROLLER_H
#include <Forwarder.hpp>
#include <iostream>
#include <map>
#include <network/Network.hpp>
#include <network/NetworkIndexer.hpp>
namespace wa {
class NetworkController {
private:
  std::vector<Forwarder *> forwarders;

  class BindInfo {
  public:
    std::string networkName;
    std::string params;
    Device *device;
    BindInfo() {}
  };

public:
  std::map<std::string, Network *> networks;
  std::map<std::string, Network *> activeNetworks;
  std::map<BindInfo *, BindInfo *> bindMap;
  NetworkController() {
    for (Network *n : NetworkIndexer::getNetworks()) {
      if (n->init()) {
        networks[n->getUserFriendlyName()] = n;
      }
    }
    activeNetworks = networks;
  }
  void stop() {
    for (auto forwarder : forwarders) {
      forwarder->stop();
    }
  }

  void bind(BindInfo *b1, BindInfo *b2) { bindMap[b1] = b2; }

  void unbind(BindInfo *b1) { bindMap.erase(b1); }

  // TODO test: consistency, atomic
  void bind(std::string n1, std::string cp1, std::string n2, std::string cp2) {
    Network *net1 = activeNetworks[n1];
    Network *net2 = activeNetworks[n2];
    if (net1 == nullptr && net2 == nullptr) {
      Log::e(fmt::format("Network {} and {} are unavailable", n1, n2));
      return;
    } else if (net1 == nullptr || net2 == nullptr) {
      Log::e(
          fmt::format("Network {} is unavailable", net1 == nullptr ? n2 : n1));
      return;
    }
    if (net1->bind(cp1) < 0) {
      Log::e(fmt::format("Binding failed: error in param net1"));
      return;
    }
    if (net2->bind(cp2) < 0) {
      // error
      Log::e(fmt::format("Binding failed: error in param net2"));
      net1->unbind(cp1);
      return;
    }
    BindInfo *bi1 = new BindInfo();
    bi1->networkName = n1;
    bi1->params = cp1;
    BindInfo *bi2 = new BindInfo();
    bi2->networkName = n2;
    bi2->params = cp2;

    bind(bi1, bi2);
    if (!start(bi1)) {
      unbind(bi1);
      //stop server
      Log::e("Bind unsuccesful");
      return;
    }
    Log::i("Bind successful");
  }

  // todo test
  BindInfo *getPairOf(Device *d) {
    for (auto p : bindMap) {
      if (p.first->params == d->address) {
        return p.second;
      }
    }
    return nullptr;
  }

  bool start(BindInfo *bi2) {
    Network *n = activeNetworks[bi2->networkName];
    if (n == nullptr) {
      // TODO ERROR HANDLE
      return false;
    }
    std::string nParams = bi2->params;
    n->setOnDeviceConnected([&](Device &d) {
      BindInfo *bi = getPairOf(&d);
      if (bi == nullptr) {
        // TODO ERROR HANDLE
        return;
      }
      Network *destNetwork = activeNetworks[bi->networkName];
      if (destNetwork == nullptr) {
        // TODO ERROR HANDLE
        return;
      }
      Device *d2 = destNetwork->connect(bi->params);
      if (d2 == nullptr) {
        fmt::print("Destination not found\n");
        d.disconnect();
        // TODO ERROR HANDLE
        return;
      }
      Forwarder *forwarder = new Forwarder(&d, d2);
      forwarder->start();
      forwarders.push_back(forwarder);
    });
    return !n->listen(nParams);
  }
};
} // namespace wa
#endif
