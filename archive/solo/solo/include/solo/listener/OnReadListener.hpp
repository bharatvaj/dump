#ifndef _ON_READ
#define _ON_READ

#include <model/Message.hpp>

class OnReadListener
{
  public:
    void onRead(Message, bool);
};
#endif