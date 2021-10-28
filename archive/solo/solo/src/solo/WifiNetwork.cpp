#include <solo/WifiNetwork.hpp>
#include <solo/Query.hpp>
#include <solo/Network.hpp>
#include <Dot/DotEvent.hpp>
#include <solo/Node.hpp>

using namespace solo;

WifiNetwork::WifiNetwork(){

}

std::vector<std::string> getHosts(Query query){
  std::vector<std::string> hosts;
  //FIXME possible code smell
  char *buffer = strdup(query.field.c_str());
  char *temp = strtok(buffer, "-");
  while(temp != NULL){
    temp = strtok(buffer, NULL);
  }
  //iterate the range of ips
  return hosts;
}

std::vector<int> getPorts(Query query){
  std::vector<int> ports;
  //TODO add logic to extract ports from Query
  return ports;
}
//TODO execute operations in threads with mutex
void WifiNetwork::scan(Query query){
    std::vector<int> ports = getPorts(query);
    for(std::string host : getHosts(query)){
      for(int port : ports){
      dot->connect(host, port);
      dot->addDotEventHandler(DotEvent::CONNECTED, [&](Dot &dot){
        //fire a Node perhaps?
        //TODO encapsulate the Dot and fire as Node
        fireEvent(DotEvent::CONNECTED, &dot);
      });
    }
    }
}
