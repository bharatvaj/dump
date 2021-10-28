# Ubiquitous Network Gateway
```
Abstract
Specification
  -Novel Features
  -Drawbacks of Existing and overcomes in our invention
-Figures[Accordingly]
  -Block Diagram
  -Component specific
  -Graph, table
  -Alternative solution
  -Semantic Diagram
Detailed Description
  -Specific Components Explanation
  -Alternate Embodiments
Experimental Graph and results
Application areas
Public Disclosure
```

## Abstract
The system proposed provides a modular abstract model to which any existing network which supports routing and message transmission can be added as module. The devices of different networks are connected to the adapter in star topology. Network modules provide routing functionality. Device modules provide messaging functionality such as send and receive. The routing inside the system is guided by multiple routing tables and entries are added using a bind function.

# Background
Transforming real world objects into virtual nodes that can be controlled by the user can be a real challenge considering the problem of interoperability. Internet of things aims to unify every devices under a single infrastructure.  WiFi was the de-facto standard for wireless technology for more than a decade for the internet. With growing number of wireless specifications such as Bluetooth, ZigBee, Thread, Z-Wave, etc. There is hardly any effort for having any compatibility between them to achieve this unified behavior. Further, even though ZigBee and Z-Wave are mostly targeted towards IoT. It does not solve the compatibility problem. And also there will be no wireless technology which is perfect for every use case. Hypothetically speaking even if there is a perfect wireless technology, there is always “The Hipster Effect”(Jonathan Touboul, 2014).By introducing an adapter, we can solve this issue by connecting nodes operating in different wireless specifications.

## Specification
### Novel Features
A bridge between devices of heterogeneous networks are created to pave the future for IoT. Traditionally the bridges are configured in the same system where a certain redirection of network is required. We are proposing a transparent bridge where the devices involved has no idea that it is contacting with devices of other networks.

### Drawbacks of Existing
#### IP Standardization
Interoperability between devices have been achieved by using same networks for various physical devices. For example, IP network can be used for Ethernet, WiFi and Bluetooth.

Drawbacks are,
* IP does not support ZigBee or ZWave and many other devices.
* Even support is brought in the future, new X/IP devices will not be able to work with old X/X devices, where X is a physical hardware such as ZigBee or ZWave.

#### Blu-Fi
Product called Blu-Fi is already in market which connects Bluetooth and WiFi devices using a modem-like Blu-Fi adapter.

The drawbacks are,
* Existing systems cannot be used or can be used only to a certain extent.
* There is very low coherence between the adapter and the devices.
* Applications are limited to tools offered by the company which offers Blu-Fi.
* Wildcard operations are not possible. An example would be connecting two different MQTT-SN devices under different networks.

### Block Diagram


## Detailed Description
The `adapter` is collectively the software `modules` and hardware required to convert relay messages from one network to another.

There are two phases in the working of adapter.

### Phase One
---
#### Detect
Forwarder supports various network technologies and also new network technologies can be added by adding the corresponding module. But due to cost and requirement, only some of the networks are actually needed. Detection of hardware devices are performed to load the corresponding software modules. For instance, in a adapter with a WiFi and a Bluetooth hardware loading extra modules such as ZigBee and ZWave will only slow the adapter.

#### Select
Even if the hardware component is present in the adapter. The power to use that component resides in software modules. This is controlled by the user of the adapter.

#### Bind
Once the networks which are needed are selected. The individual devices which is needed to register with other networks are specified. This step should be done for each devices and for each network. (i.e) If there are N networks and there are D devices in each network. D devices in each network should be configured to work with other N-1 networks or as need by the user. Since this process is systematic, it can be easily automated by scripts or can configured by the user as needed. Thus providing convenience and flexibility.

### Phase two
---
#### Listen
When a device is binded, the forwarder starts a listen instance. Whenever a bind is configured for a device with Network `A` to another device with Network `B`, Forwarder starts listening for `B` devices. When a device `C` tries to access `B`, connect instance of `A` is called.

#### Connect
`A` is the device with true address, whereas the device `B` is pseudo. Connection to `A` is done with the forwarder.

#### Forward
Adapter has the connection with `A` and `C`. This leaves us with stripping and repackaging
##### Strip
The `message` is the data which is to reach the end system. Suppose `C` is the first one to send message to `A`. `C` sends the message to adapter. Adapter strips the `C` related network headers and network related encryption, and stores the message.


##### Repackage
The message which is free of all the network headers and network related encryption, it is repackaged with `A` related network headers and encryption if any.
