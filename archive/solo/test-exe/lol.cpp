#include <comm.h>

int main(){
  comm_init();
  comm_start_server(3400);
  while(1);
  return 0;
}
