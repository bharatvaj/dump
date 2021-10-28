#ifndef _ON_WRITE
#define _ON_WRITE

#include <model/Message.hpp>

class OnWriteListener
{
  public:
    void onWrite(Message, bool);
};
#endif