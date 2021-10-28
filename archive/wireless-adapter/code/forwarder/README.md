# Universal IP Adapter

## Abstract
The system proposed provides a modular abstract model to which any existing network which supports routing and message transmission can be added as a module. The devices of different networks are connected to the adapter in a star topology. Network modules provide routing functionality. Device modules provide messaging functionality such as to send and receive. The routing inside the system is guided by multiple routing tables and entries are added using a bind function.

## Background
Transforming real-world objects into virtual nodes that can be controlled by the user can be a real challenge considering the problem of interoperability. Internet of things aims to unify every device under a single infrastructure.  WiFi was the de-facto standard for wireless technology for more than a decade for the internet. With the growing number of wireless specifications such as Bluetooth, ZigBee, Thread, Z-Wave, etc. There is hardly any effort for having any compatibility between them to achieve this unified behaviour. Further, even though ZigBee and Z-Wave are mostly targeted towards IoT. It does not solve the compatibility problem. And also there will be no wireless technology which is perfect for every use case. Hypothetically speaking even if there is a perfect wireless technology, there is always “The Hipster Effect”(Jonathan Touboul, 2014). By introducing an adapter, we can solve this issue by connecting nodes operating in different wireless specifications.

## Specification
### Novel Features
A bridge between devices of heterogeneous networks is created to pave the future for IoT. Traditionally the bridges are configured in the same system where a certain redirection of network is required. We are proposing a system where the devices involved has no idea that it is contacting devices of other networks.

### Drawbacks of Existing System
* Existing systems are only limited to selected networks.
    - An interface is provided to add new protocols as modules that are not in the existing system.
* There is very low coherence between the adapter and the devices.
    - Coherence between the adapter and the devices is increased by providing an abstract adapter interface
* Wildcard operations are limited in existing products. An example would be connecting two different MQTT-SN devices under unsupported networks.
    - The interface allows configuration of different protocols that supports connect and listen operations

## Detailed Description
The `adapter` collectively consists of `software modules` and respective hardwares required to convert and relay messages from one network to another.

There are two phases in the working of adapter.

### Phase One
Phase One allows the adapter to setup the routes for communication.

#### Detect
The adapter provides compatibility among various network technologies along with the capability to add new technologies by adding their respective modules
The adapter identifies all available network hardware modules present in it through the software method, init().
init() returns true if that particular hardware module is present else it returns false.
All the values returned by the init() are taken into account and the network are displayed iff the returned statement is true.

#### Select
Select provides the functionality to choose which hardware module must be turned on through the given interface for the process of inter operable communication based on their requirements.

#### Discover
The adapter searches for the devices based on the user selected networks and stores the details of those devices according to their network in their respective software modules.

#### Bind
Devices recognized by the adapter is given an alias in the form of IP address, is the gist of bind. Bind process involves,
* Acquiring IP address
* Aliasing IP alias

##### Acquiring IP address
The local router is polled for free IP addresses via DHCP and stored in the adapter for lookup.

##### Aliasing IP address
Obtained IP address is registered as an IP alias(RFC2219) to the adapter. Address of detected devices of other network is stored in local memory and lookep upon connection. Address is canonical and subjective to the respective network. Address of detected devices are assigned to the obtained IP in a one-one relationship.

### Phase two
Phase two enables the adapter to communicate.

#### Listen
When a device is binded, the adapter starts a listen instance for that particular device based on it's network.

#### Connect
An IP device is connected with an IP device or any other device using a pseudo IP address which is assigned to the adapter, provided by the bind()

#### Forward
Adapter has the connection between the devices which allows it to transfer the data by stripping and then repackaging based on their respective network.

##### Strip
The `message` is the data that has to be transferred between the devices irrespective of their networks, so the unnecessary details other than the message that is transmitted from the sender is stripped and discarded.

##### Repackage
The message which is free of all the network headers and network related encryption is repacked based on the receiver's network.

## Process
Adapter discovers all the available network compatible within the adapter using modules. Adapter allows for the selection of the network modules to be used. Based on the selected network modules the devices that are in a closer proximity to the adapter are found and saved. All non IP devices are bound with a pseudo IP address.

Phase two depends on Phase one to be completed at least once.

Once connection is established using listen/connect, the data without the connection information is transmitted.
The adapter starts the listening once the binding is completed, then as soon as the device establishes a connect signal a bridge is created which enables two or more homogenous networks or heterogeneous networks to communicate.
Once the connection is established between the devices the data transfer is done based on the sender and receiver networks, the adapter uses the respective modules for stripping and repackaging the transferred data.

## Claims
1. Adding network devices to IP network using an intermediate adapter via IP aliasing.
2. Throughput of the system will be lowest of the two with additional constant K time. Where K is the time required to strip and repackage.
3. Range is effectively increased on certain combinations of network setup.
4. Existing protocol is used, no new protocol is created.
5. The application data is never read by the adapter. Only network information is read.
6. The new networks that are supported can be extended using software modules.
