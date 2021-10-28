#include <solo/NetworkManager.hpp>
#include <solo/WifiNetwork.hpp>
#include <future>
#include <unistd.h>
#include <thread>
#include <chrono>
#include <clog/clog.h>

using namespace solo;

NetworkManager* NetworkManager::instance = nullptr;

void NetworkManager::listenForNetwork(NetworkManager *networkManager){
  //hook with the operating system
  //NOTE In linux, one can listen to the /dev/ folder to find change in networks
  WifiNetwork *wifiNetwork = new WifiNetwork();
  //FIXME assumng a network is found fireEvent is called after five seconds
  //std::this_thread::sleep_for(std::chrono::seconds(5));
  std::cout << "Event called" << '\n';
  std::this_thread::sleep_for(std::chrono::seconds(5));
  networkManager->notify(NetworkEvent::FOUND, *wifiNetwork);
}

void NetworkManager::notify(NetworkEvent networkEvent, Network &network){
  fireEvent(networkEvent, network);
}

NetworkManager::NetworkManager(){
  //start listening for new networks in seperate thread
  networkListenerThread = new std::thread(listenForNetwork, this);
  networkListenerThread->detach();
  //std::cout << networkListenerThread->get_id() << '\n';
  //t.detach();
}

NetworkManager *solo::NetworkManager::getInstance(){
    if(instance == nullptr){
        instance = new NetworkManager();
    }
    return instance;
}

std::vector<Network *> NetworkManager::getNetworks(){
  return networks;
}

NetworkManager::~NetworkManager(){
  networkListenerThread->join();
}
