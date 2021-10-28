### Core Implementation
#### Network Module
```cpp
// initialize the module
virtual bool init();
// returns a human readable user friendly name
virtual std::string getUserFriendlyName();
// listen
virtual int listen(std::string);
// connects to the given address
virtual Device *connect(std::string);
// bind a route to system
virtual int bind(std::string);
// unbind a route from the table
virtual int unbind(std::string);
// discovers different networks
virtual std::vector<DeviceInfo> discover();
// should be deprecated
std::vector<Device *> getDevices();
```

#### Device Module
```cpp
// disconnects the device from the network
virtual void disconnect();
// read from the connected device
virtual int read(char *message, int bytesToRead);
// write to the connected device
virtual int write(char *message, int bytesToWrite);
```

### Core Routing and Message Transmission

#### Routing
`BindInfo` holds the binding Information
```cpp
class BindInfo {
  std::string networkName;
  std::string params;
  Device *device;
};
```

`NetworkController` is guided by the rules set in place by the `nBindMap`.
```cpp
std::vector<std::map<BindInfo *, BindInfo *>> nBindMap;
```

`bind` is called for updating the routing table
```cpp
void bind(std::string n1, std::string cp1, std::string n2, std::string cp2) {
  Network *net1 = activeNetworks[n1];
  Network *net2 = activeNetworks[n2];
  if (net1 == nullptr && net2 == nullptr) {
    // network not recognized
    return;
  } else if (net1 == nullptr || net2 == nullptr) {
    // network not available
    return;
  }
  if (net1->bind(cp1) < 0) {
    // net1: param error
    return;
  }
  if (net2->bind(cp2) < 0) {
    // net2: param error
    net1->unbind(cp1);
    return;
  }
  BindInfo *bi1 = new BindInfo();
  bi1->networkName = n1;
  bi1->params = cp1;
  BindInfo *bi2 = new BindInfo();
  bi2->networkName = n2;
  bi2->params = cp2;

  bind(bi1, bi2);
  if (!start(bi1)) {
    //server start error
    unbind(bi1);
    return;
  }
}
```

`start` initializous the local hardware and is responsible for arranging message transmission.

```cpp

  bool start(BindInfo *bi2) {
    Network *n = activeNetworks[bi2->networkName];
    if (n == nullptr) {
      return false;
    }
    std::string nParams = bi2->params;
    n->setOnDeviceConnected([&](Device &d) {
      BindInfo *bi = getPairOf(&d);
      if (bi == nullptr) {
        // invalid route
        return;
      }
      Network *destNetwork = activeNetworks[bi->networkName];
      if (destNetwork == nullptr) {
        // invalid route
        return;
      }
      Device *d2 = destNetwork->connect(bi->params);
      if (d2 == nullptr) {
        // destination not found
        d.disconnect();
        return;
      }
      Forwarder *forwarder = new Forwarder(&d, d2);
      forwarder->start();
      forwarders.push_back(forwarder);
  });
  return !n->listen(nParams);
}
```

 Once connection is established, the connected device references are delegated to the `Forwarder` for message transmission.

#### Message Transmission

`DeviceInfo` is an container for storing information of a device which is later used by the Forwarder.
 ```cpp
 class DeviceInfo {
     public:
     std::string address;
     std::string name;
     Network *network;
 };
 ```

`Forwarder` performs a simple bi-directional message transmission.

 ```cpp
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
 ```
