#include <solo/SOLO.hpp>
#include <Dot/Dot.hpp>

using namespace solo;
using namespace dot;

SOLO::SOLO(){
  //soloDot is a multi responder
  soloDot = &Dot::getDot();
  soloDot->readFor("update").addDotOperationEventHandler(DotOperationEvent::SUCCESS, [](Dot &dot){
    dot.write("nil");
  });

  networkManager = NetworkManager::getInstance();
  networkManager->dot = soloDot;
  nodeManager = NodeManager::getInstance();

  //nodeManager registers with networkManager and populates the node list
  nodeManager->bind(networkManager);

  //TODO delegate the task of updating the UI to seperate class
  ///////////////////SEPERATE///////////////////////////////////
  nodeManager->addEventHandler(DotEvent::CONNECTED, [](Node &node){
    std::cout << "node connected" << '\n';
  });
  ///////////////////SEPERATE///////////////////////////////////

  soloDot->run();
}

//TODO implement for bluetooth
void SOLO::checkForNetworks(){

}
