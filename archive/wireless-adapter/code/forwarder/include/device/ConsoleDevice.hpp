#include <cstdio>
#include <unistd.h>
namespace wa {
class ConsoleDevice : public Device {
public:
  ConsoleDevice() { address = "console"; }

  int read(char *msg, int bytesToRead){
    // FILE *f = fopen(fmt::format("/proc/{}/fd/1").c_str(), "r");
    int status = ::read(STDIN_FILENO, msg, bytesToRead-1);
    msg[bytesToRead-1] = '\0';
    return status;
  }

  int write(char *str, int bytesToWrite){
    return ::write(STDOUT_FILENO, str, bytesToWrite);
  }

};
} // namespace wa
