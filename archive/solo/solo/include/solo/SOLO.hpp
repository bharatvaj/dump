#ifndef _SOLO_H
#define _SOLO_H
#include <solo/NetworkManager.hpp>
#include <solo/NodeManager.hpp>
#include <Dot/Dot.hpp>

using namespace dot;

namespace solo {
  class SOLO {
  public:
    Dot *soloDot;
    NetworkManager *networkManager;
    NodeManager *nodeManager;
    SOLO();
    void checkForNetworks();
  };
}
#endif
