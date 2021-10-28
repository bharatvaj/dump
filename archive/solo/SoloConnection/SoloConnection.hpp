#ifndef _SOLO_CONNECTION_H
#define _SOLO_CONNECTION_H

namespace solo {
class SoloConnection {
private:
    SoloConnection();
public:
  //reads until the number of @param bytes is reads
  //@return char * the message is returned
  virtual char *read(int bytes);
  //writes @param message to the activated socket
  //@return the number of bytes written
  virtual int write(char *message);
};
}

#endif
