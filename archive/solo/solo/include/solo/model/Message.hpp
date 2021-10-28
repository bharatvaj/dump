#ifndef _MESSAGE
#define _MESSAGE

#include <iostream>

namespace solo {
class Message
{
  public:
    std::string nodeId;
    std::string data;
    Message(std::string &nodeId, std::string data)
    {
        this->nodeId = nodeId;
        this->data = data;
    }
};
}

#endif
