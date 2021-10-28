#ifndef _NETWORK_CONFIGURATION
#define _NETWORK_CONFIGURATION

#include <network/Network.hpp>
#include <medium/medium.hpp>

namespace wa {
    class NetworkConfiguration {
        public:
        Network *network;
        Medium *medium;
    };
}
#endif