#ifndef _SHELL_H
#define _SHELL_H
#include <Dot/Dot.hpp>
#include <cstdarg>
#include <fmt/format.h>
#include <functional>
#include <iostream>
#include <map>
#include <vector>
#define _SHELL_PORT 50000
using namespace dot;
namespace shell4c {
class Shell {
private:
  Dot *d;
  std::map<std::string, Operation *> operations;
  std::function<void(int)> exitHandler = [](int) {};
  bool isConnected = false;
  bool isDaemon = false;
  std::string shellIcon;
  Dot *client = nullptr;


  bool endsWith(const std::string &s, const std::string &suffix) {
    return s.size() >= suffix.size() &&
           s.substr(s.size() - suffix.size()) == suffix;
  }

  std::vector<std::string> split(const std::string &s,
                                 const std::string &delimiter,
                                 const bool &removeEmptyEntries = false) {
    std::vector<std::string> tokens;

    for (size_t start = 0, end; start < s.length();
         start = end + delimiter.length()) {
      size_t position = s.find(delimiter, start);
      end = position != std::string::npos ? position : s.length();

      std::string token = s.substr(start, end - start);
      if (!removeEmptyEntries || !token.empty()) {
        tokens.push_back(token);
      }
    }

    if (!removeEmptyEntries && (s.empty() || endsWith(s, delimiter))) {
      tokens.push_back("");
    }

    return tokens;
  }

public:
  Shell(bool isDaemon, std::string shellIcon)
      : isDaemon(isDaemon), shellIcon(shellIcon) {
    if(!isDaemon) return;
    d = &Dot::getDot(_SHELL_PORT);
    d->onConnect([=](Dot &c) {
      isConnected = true;
      client = &c;
      print(shellIcon);
      client->readFor(".*").onSuccess([=](Dot &, std::string m) {
        std::vector<std::string> ins = split(m, " ", true);
        execute(ins);
        print(shellIcon);
      });
      client->onDisconnect([=](Dot &d) { exitHandler(0); });
    });
    d->onDisconnect([=](Dot &client) { isConnected = false; });
  }
  void addOperation(std::string op,
                    std::function<void(std::vector<std::string>)> opFunc) {
    // operations[op] = opFunc;
  }
  void addOperation(Operation *op) { operations[op->name] = op; }

  std::map<std::string, Operation *> getOperations() { return operations; }
  // TODO
  // void addOperation(Operation op){
  //   operations[op.name] = op;
  // }
  void execute(std::vector<std::string> ins) {
    if (ins.size() == 0) {
      return;
    }
    Operation *op = operations[ins[0]];
    if (op == nullptr) {
      print(fmt::format("{}: operation not found\n", ins[0]));
      return;
    }
    std::function<void(Operation, std::vector<std::string>)> func = op->fp;
    ins.erase(ins.begin(), ins.begin() + 1);
    try {
      func(*op, ins);
    } catch (std::bad_function_call *bfc) {
      print(fmt::format("{}: operation not found\n", ins[0]));
    }
  }

  void print(std::string message) {
    if (isDaemon) {
      if (client != nullptr) {
        client->write(message);
      }
    } else {
      fmt::print(message);
    }
  }

  void println(std::string message) { print(message + "\n"); }

  void start() {
    if (isDaemon) {
      fmt::print("forwarder server started\n");
    } else {
      std::string in;
      fmt::print(shellIcon);
      while (std::getline(std::cin, in)) {
        std::vector<std::string> ins = split(in, " ", true);
        execute(ins);
        fmt::print(shellIcon);
      }
    }
  }

  void setOnShellExit(std::function<void(int)> func) { exitHandler = func; }
};
} // namespace shell4c
#endif