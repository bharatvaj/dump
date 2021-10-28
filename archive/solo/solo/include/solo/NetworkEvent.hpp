#ifndef _NETWORK_EVENT_H
#define _NETWORK_EVENT_H
/*
 * Network states for the different networks connected by solo
 * NEW - New network connection found
 * LOST - An existing network connection is lost
 */
 namespace solo {
enum class NetworkEvent {
  FOUND,
  LOST
};
}
#endif
