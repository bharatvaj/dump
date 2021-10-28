#ifndef _NODE_MANAGER
#define _NODE_MANAGER "NodeManager"

#include <vector>
#include <solo/Node.hpp>
#include <solo/NetworkManager.hpp>

#include <em/EventManager.hpp>
#include <Dot/Dot.hpp>

using namespace dot;
using namespace em;


namespace solo {
  /*
   * Handles the nodes in the added network network
   * NodeManager is tightly coupled with NetworkManager
   */
   //TODO typdef DotEvent to NodeEvent
class NodeManager : public EventManager<DotEvent, Node &> {

    private:
    static NodeManager* instance;
    NodeManager();
    NetworkManager *networkManager;
    std::map<Network *, std::vector<Node *>> *nodeMap;

    public:
    static NodeManager *getInstance();
    Dot *dot = nullptr;

    /*
     * Returns the map of Network and Nodes
     * @return std::map<Network *, std::vector<Node *>> List of Network with their nodes
     */
    std::map<Network *, std::vector<Node *>> getNodes();
    /*
     * Binds the NodeManager with the NetworkManager
     * NOTE Should be called only once
     * Only present to simplify the process of acquiring nodes from network
     * Registering and managing these within solo class creates complexity
     */
    void bind(NetworkManager *networkManager);
    /*
     * Returns the nodes that are not yet
     * added to NodeManager
     * @param network The network to find the nodes
     * @return std::vector<Node *> The list of nodes found in the given network
     */
    std::vector<Node *> scan(Network &network);
};
}
#endif
