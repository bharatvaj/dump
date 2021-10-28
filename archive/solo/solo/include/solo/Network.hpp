#ifndef _NETWORK
#define _NETWORK

#include <functional>
#include <solo/model/Message.hpp>
#include <solo/Query.hpp>

#include <Dot/Dot.hpp>

#include <em/EventManager.hpp>

using namespace em;
using namespace dot;

namespace solo {
class Network : public EventManager<DotEvent, Dot *>  {
  /*
   * The Network class provides abstraction for scanning
   * networks and provides events that can be used by the User
   */
public:
  std::string networkId;
  Network();
  Dot *dot = nullptr;
  /*
   * scan is pulse operation to find nodes in the respective network
   * i.e Scans the network only once and calls the events
   * based on the status of the operation
   * Network class operations are by default are no-op
   * The implementations should only be done by the base class
   */
  virtual void scan(Query);
};
}

#endif
