#ifndef _GPIO_H
#define _GPIO_H
#include <GPIO.hpp>
#include <cstdio>
#include <fmt/format.h>
#include <fstream>
#include <iostream>
#include <unistd.h>
namespace gpio {
enum class Direction { IN, OUT };
const char *GPIO_EXPORT_PATH = "/sys/class/gpio/export";
const char *GPIO_DIRECTION_PATH = "/sys/class/gpio/gpio{}/direction";
const char *GPIO_VALUE_PATH = "/sys/class/gpio/gpio{}/value";
const int offset = 458;
class GPIO {

  template <typename T> void write(std::string path, T value) {
    std::ofstream f(path);
	std::cout << value << std::endl;
    f << value;
    f.close();
  }

public:
  int pin = 0;
  Direction direction = Direction::IN;
  GPIO(int pin) : pin(pin) { 
	write(GPIO_EXPORT_PATH, pin);
}

  static GPIO *getGPIO(int pinNo) {
    int pin = offset + pinNo;
    return new GPIO(pin);
  }

  void setDirection(Direction direction) {
    std::string path = fmt::format(GPIO_DIRECTION_PATH, pin);
    write(path, direction == Direction::IN ? "in" : "out");
  }

  void setHigh() {
    std::string path = fmt::format(GPIO_VALUE_PATH, pin);
    write(path, 1);
  }

  void setLow() {
    std::string path = fmt::format(GPIO_VALUE_PATH, pin);
    write(path, 0);
  }
};
} // namespace gpio
#endif
