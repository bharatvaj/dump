#ifndef _CONFIGURATION_H
#define _CONFIGURATION_H
#include <network/Network.hpp>
#include <medium/Medium.hpp>
namespace wa {
    class Configuration {
        private:
        Network *network;
        Medium *medium;
        public:
        Configuration(Network *network, Medium *medium) : network(network), medium(medium){
        }
        Network *getNetwork(){
            return network;
        }
        Medium *getMedium(){
            return medium;
        }
    };
}
#endif