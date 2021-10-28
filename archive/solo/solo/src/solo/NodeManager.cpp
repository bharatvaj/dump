#include <solo/NodeManager.hpp>
#include <solo/Query.hpp>

using namespace solo;

NodeManager *NodeManager::instance = nullptr;

NodeManager::NodeManager(){
  nodeMap = new std::map<Network *, std::vector<Node *>>();
}

NodeManager *NodeManager::getInstance(){
    if(instance == nullptr){
        instance = new NodeManager();
    }
    return instance;
}

std::map<Network *, std::vector<Node *>> NodeManager::getNodes(){
  return *nodeMap;
}

/*
std::vector<Node *> NodeManager::scan(Network &network){
  std::vector<Node *> nodes;
  //scan the network for nodes
  std::cout << "Scanning the general network using general methods of Network class" << std::endl;
  return nodes;
}
*/

void NodeManager::bind(NetworkManager *netManager){
    if(netManager == nullptr){
      log_err(_NODE_MANAGER, "Provided NetworkManager points to null");
      throw networkManager;
    }
    std::cout << "registering listeners" << '\n';
    netManager->addEventHandler(NetworkEvent::FOUND, [&](Network &network){
      std::cout << "wifiNetwork found" << '\n';
      Query query;
      query.query = "192.168.1.*:3500-4000";
      std::map<Network *, std::vector<Node *>> &networks = *nodeMap;
      network.addEventHandler(DotEvent::CONNECTED, [&](Dot *dot){
        /*
        if(networks[&network] == nullptr){
          networks[&network] = *(new std::vector<Node *>());
        }
        */
        Node *node = new Node(network);
        node->dot = dot;
        networks[&network].push_back(node);
      });
      network.scan(query);
    });
}
