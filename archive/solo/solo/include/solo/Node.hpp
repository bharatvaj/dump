#ifndef _NODE
#define _NODE

#include <solo/Network.hpp>

namespace solo
{
class Node
{
  public:
    Node();
    Node(Network&);
    Dot *dot = nullptr;

  private:
    std::string id;
};
}

#endif
