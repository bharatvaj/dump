#ifndef _CONFIGURATION_BUILDER_H
#define _CONFIGURATION_BUILDER_H
#include <Configuration.hpp>
#include <network/Network.hpp>
#include <medium/Medium.hpp>
namespace wa {
class ConfigurationBuilder {
  public:
  Network *network;
  Medium *medium;
  Configuration *configuration;
  ConfigurationBuilder(){
  }
  ConfigurationBuilder &setNetworkParams(std::string networkParams){
    network->setNetworkParams(networkParams);
    return *this;
  }
  ConfigurationBuilder &setNetwork(Network *network){
    this->network = network;
    return *this;
  }
  ConfigurationBuilder &setMedium(Medium *medium){
    this->medium = medium;
    return *this;
  }
  Configuration *build(){
    return new Configuration(network, medium);
  }
};
}
#endif