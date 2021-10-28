#ifndef _LED_H
#define _LED_H
#include <GPIO.hpp>
#include <fmt/format.h>
#include <unistd.h>
using namespace gpio;
class LED
{
  public:
    GPIO *g = nullptr;
    LED(GPIO *g) : g(g) { g->setDirection(Direction::OUT); }

    void on() { g->setHigh(); }

    void off() { g->setLow(); }
};
#endif