#ifndef _NETWORK_MANAGER
#define _NETWORK_MANAGER "NetworkManager"

#include <vector>
#include <solo/NetworkEvent.hpp>
#include <solo/Network.hpp>
#include <thread>

#include <em/EventManager.hpp>
#include <Dot/Dot.hpp>

using namespace em;

namespace solo{
  /* Handles the network of solo
   * Constantly scans the device for new network and adds them to the list
   * This class also considers USB as a network
   */
class NetworkManager : public EventManager<NetworkEvent, Network &>{

    private:
    static NetworkManager* instance;
    std::thread *networkListenerThread;
    //start the NetworkLooper
    NetworkManager();
    std::vector<Network *> networks;
    static void listenForNetwork(NetworkManager *);

    public:
    static NetworkManager *getInstance();
    void notify(NetworkEvent, Network &);
    Dot *dot = nullptr;

    std::vector<solo::Network*> getNetworks();
    ~NetworkManager();
};
}

#endif
