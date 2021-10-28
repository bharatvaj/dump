#ifndef _FLOG_H
#define _FLOG_H
#include <clog/clog.h>
#include <iostream>
static std::string TAG = "";
class Log {
public:
  static void enable() { clog_enable(); }
  static void disable() { clog_disable(); }
  static void i(std::string msg, std::string tag = TAG) {
    clog_i(tag.c_str(), msg.c_str());
  }
  static void e(std::string msg, std::string tag = TAG) {
    clog_e(tag.c_str(), msg.c_str());
  }
  static void w(std::string msg, std::string tag = TAG) {
    clog_w(tag.c_str(), msg.c_str());
  }
  static void f(std::string msg, std::string tag = TAG) {
    clog_f(tag.c_str(), msg.c_str());
  }
  static void p(std::string msg, std::string tag = TAG) {
    clog_e(tag.c_str(), msg.c_str());
  }
  static void v(std::string msg, std::string tag = TAG) {
    clog_v(tag.c_str(), msg.c_str());
  }
};
#endif
