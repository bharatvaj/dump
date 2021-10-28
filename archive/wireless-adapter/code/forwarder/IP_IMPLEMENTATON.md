### IP
#### Network Module

```cpp
class IP : public Network {

  std::string networkParams;
  int configPort = 2500;

public:
  IP() {}

  bool init() override {
    return true;
    struct ifreq ifr;
    int sock = socket(PF_INET, SOCK_DGRAM, IPPROTO_IP);
    memset(&ifr, 0, sizeof(ifr));
    strcpy(ifr.ifr_name, networkParams.c_str());
    if (ioctl(sock, SIOCGIFFLAGS, &ifr) < 0) {
      perror("SIOCGIFFLAGS");
    }
    close(sock);
    return !!(ifr.ifr_flags & IFF_UP);
  }

  std::string getUserFriendlyName() override { return _IP_H; }

  xs_SOCKET start_server(bool isTcp, int port) {
    static xs_SOCKET servfd;

    struct sockaddr_in server, client;
    socklen_t cli_size;

    if (comm_check_port(port) < 0) {
      return -1;
    }
    // Prepare the sockaddr_in structure
    server.sin_family = AF_INET;
    server.sin_addr.s_addr = INADDR_ANY;
    server.sin_port = htons(port);
    cli_size = sizeof(struct sockaddr_in);

    // Create socket
    if (isTcp) {
      servfd = socket(AF_INET, SOCK_STREAM, 0);
    } else {
      servfd = socket(AF_INET, SOCK_DGRAM, 0);
    }
    if (servfd == SOCKET_ERROR) {
      clog_e(_COMM, "could not create socket");
      return -1;
    }
    // Bind
    if (::bind(servfd, (struct sockaddr *)&server, sizeof(server)) < 0) {
      clog_e(_COMM, "bind failed");
      return -1;
    }
    // Listen
    ::listen(servfd, COMM_SERV_BACKLOG);
    // Accept and incoming connection
    clog_i(_COMM, "Waiting for incoming connections...");
    // accept connection from an incoming client
    xs_SOCKET clifd = accept(servfd, (struct sockaddr *)&client, &cli_size);
    if (clifd == SOCKET_ERROR) {
      clog_i(_COMM, "Accept failed");
      return -1;
    }
    clog_i(_COMM, "Connection accepted");
    return clifd;
  }

  int bind(std::string params) override {
    int stat = system(
        fmt::format("ip address add {} dev {}", params, getNetworkParams())
            .c_str());
    if (stat != 0) {
      Log::e("Operation failed");
      return -1;
    }
    return 0;
  }

  int unbind(std::string params) override {
    return -1;
  }

  int listen(std::string networkParams) override {
    cout << "IP listening" << endl;
    struct sockaddr_in peer;
    socklen_t peerLen = sizeof(peer);
    static int cont;
    bool isTcp = true;
    xs_SOCKET fd = start_server(isTcp, 2500);

    if (fd < 0) {
      cout << "Socket error" << endl;
    }
    cout << fd << endl;
    getsockname(fd, (struct sockaddr *)&peer, &peerLen);
    Device *d = (Device *)new IPDevice(fd);
    // FIXME NAMING ERROR
    d->name = "IPDevice " + std::to_string(devices.size());
    d->address = inet_ntoa(peer.sin_addr);
    devices.push_back(d);

    if (fd == SOCKET_ERROR) {
      return -1;
    }
    deviceConnected(*d);
    return 0;
  }

  int addDevice(std::string ipaddress) { return -1; }

  void setNetworkParams(std::string networkParams) override {
    Network::setNetworkParams(networkParams);
  }

  Device *connect(std::string conParams) override {
    xs_SOCKET fd = ::comm_connect_server(conParams.c_str(), 21);
    if (fd == SOCKET_ERROR) {
      return nullptr;
    }
    devices.push_back((Device *)new IPDevice(fd));
    return (Device *)new IPDevice(0);
  }

  std::vector<DeviceInfo> discover() override {
    return std::vector<DeviceInfo>();
  }
};
```

#### Device Module
```cpp
class IPDevice : Device {
private:
  xs_SOCKET fd;
  bool isTcp;

public:
  IPDevice(xs_SOCKET fd) : fd(fd), isTcp(true) {}

  int read(char *str, int bytesToRead) override {
    return ::read(fd, str, bytesToRead);
  }

  int write(char *str, int bytesToSend) override {
    return ::write(fd, str, bytesToSend);
  }

  void disconnect(){
    ::close(fd);
  }

  ~IPDevice() { comm_close_socket(fd); }
};
```
