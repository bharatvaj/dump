#ifndef _DEVICE_INFO
#define _DEVICE_INFO
#include <iostream>
#include <network/Network.hpp>
namespace wa {
    class Network;
    class DeviceInfo {
        public:
        std::string address;
        std::string name;
        Network *network;
    };
}
#endif