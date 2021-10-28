#include <csignal>
#include <iostream>
#include <map>
#include <thread>

#include "Operation.hpp"
#include "Shell.hpp"
#include <Configuration.hpp>
#include <ConfigurationBuilder.hpp>
#include <Log.hpp>
#include <NetworkController.hpp>
#include <fmt/format.h>
#include <model/DeviceInfo.hpp>
#include <network/Network.hpp>
#include <network/NetworkIndexer.hpp>

using namespace std;
using namespace wa;
using namespace shell4c;

NetworkController *nc;
Shell shell(false, "adapter> ");

void exit_handler(int sig) {
  if (nc != nullptr) {
    nc->stop();
  }
  // delete left and right initiators*
  // TODO unregister callbacks
  for (auto network : nc->activeNetworks) {
    delete network.second;
  }
  exit(EXIT_SUCCESS);
}

void enable(shell4c::Operation op, vector<string> msg) {
  if (msg.size() == 0) {
    shell.println(op.usage);
    return;
  }
  Network *network = nc->networks[msg[0]];
  if (!network) {
    network = nc->activeNetworks[msg[0]];
    if (!network) {
      shell.println("Enter a valid network name");
      return;
    }
  }
  if (msg.size() > 1) {
    network->setNetworkParams(msg[1]);
  }
  nc->activeNetworks[msg[0]] = network;
  nc->networks.erase(network->getUserFriendlyName());
}

void disable(shell4c::Operation op, vector<string> msg){
  //
}

void config(shell4c::Operation op, vector<string> msg) {
  if (msg.size() == 0) {
    shell.println(op.usage);
    return;
  }
}

void list(shell4c::Operation op, vector<string> msg) {
  if (nc->activeNetworks.size() > 0) {
    shell.print(fmt::format("\u001b[32m"));
    for (auto network : nc->activeNetworks) {
      shell.print(fmt::format("{}", network.first));
      if (!network.second->getNetworkParams().empty()) {
        shell.print(fmt::format(" [{}]", network.second->getNetworkParams()));
      }
      shell.print("\n");
    }
    shell.print("\u001b[0m");
  }
  if (nc->networks.size() > 0) {
    for (auto network : nc->networks) {
      shell.print(fmt::format("{}\n", network.first));
    }
  }
}

void bind_(shell4c::Operation op, vector<string> msg) {
  if (msg.size() < 4) {
    shell.println(op.usage);
    return;
  }
  nc->bind(msg[0], msg[1], msg[2], msg[3]);
}

void scan(shell4c::Operation op, vector<string> msgs){
  if(msgs.empty()){
    shell.println(op.usage);
    return;
  }
  Network *network = nc->networks[msgs[0]];
  if (!network) {
    network = nc->activeNetworks[msgs[0]];
    if (!network) {
      shell.println("Enter a valid network name");
      return;
    }
  }
  auto devices = network->discover();
  if(devices.empty()){
    shell.println("No devices found");
    return;
  }
  for(auto d : devices){
    shell.println(d.address);
  }
}

void help(shell4c::Operation op, vector<string> messages) {
  auto ops = shell.getOperations();
  if (messages.size() == 0) {
    for (auto op : ops) {
      if (op.second == nullptr)
        continue;
      shell.println(fmt::format("\u001b[33m{}\u001b[0m\t- {}", op.first,
                                op.second->description));
    }
    return;
  }
  for (auto msg : messages) {
    if (ops.find(msg) != ops.end()) {
      shell4c::Operation *op = ops[msg];
      if (op == nullptr)
        continue;
      shell.println(op->usage);
    }
  }
}

int main(int argc, char *argv[]) {
  signal(SIGINT, exit_handler);
  Log::enable();
  nc = new NetworkController();
  shell.addOperation(new shell4c::Operation(
      "enable", "adds network to ready state", enable, "enable <network1> [options]"));
  // shell.addOperation(new shell4c::Operation(
      // "disable", "adds network to ready state", disable, "disable <network1> [network2 ...]"));
  shell.addOperation(new shell4c::Operation("bind", "binds ip address to mac",
                                            bind_, "bind <ip> <port> <mac>"));
  shell.addOperation(
      new shell4c::Operation("config", "configures medium to network", config,
                             "config <network1> [<network2> ...]"));
  shell.addOperation(new shell4c::Operation("ls", "list all available network",
                                            list, "ls [network]"));
  shell.addOperation(new shell4c::Operation("help", "prints this menu", help,
                                            "help [operation1 ...]"));
  shell.addOperation(
      new shell4c::Operation("scan", "scan a network",
                             scan, "scan <network>"));
  shell.setOnShellExit(exit_handler);
  shell.start();
  while (1)
    ;
  return 0;
}
