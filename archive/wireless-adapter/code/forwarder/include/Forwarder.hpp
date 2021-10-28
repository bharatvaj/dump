#ifndef _FORWARDER_H
#define _FORWARDER_H

#include <map>
#include <thread>

#include <device/Device.hpp>
#include <Log.hpp>

#define BUF_SIZE 256

namespace wa {

class Forwarder {
private:
  Device *lp, *rp;
  std::thread *t1, *t2;
  bool shouldStop = false;

public:
  Forwarder(Device *lp, Device *rp) : lp(lp), rp(rp) {
    Log::i("Forwarder started");
  }

  //FIXME retransmission error
  void start() {
    t1 = new std::thread([&]() {
      char *str = (char *)calloc(sizeof(char), BUF_SIZE);
      int bytesRead = 0;
      while ((bytesRead = lp->read(str, BUF_SIZE - 1)) > 0) {
        if (rp->write(str, bytesRead) < 0) {
          break;
        };
      }
    });
    t2 = new std::thread([&]() {
      char *str = (char *)calloc(sizeof(char), BUF_SIZE);
      int bytesRead = 0;
      while ((bytesRead = rp->read(str, BUF_SIZE - 1)) > 0) {
        if (lp->write(str, bytesRead) < 0) {
          break;
        }
      }
    });
  }

  // TODO test
  void stop() {
    shouldStop = true;
    t1->join();
    t2->join();
  }
  // std::vector<Forwarder *> Forwarder::forwarders = std::vector<Forwarder
  // *>();
};
} // namespace wa
#endif
