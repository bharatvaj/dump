#include <stdio.h>
#include <stdlib.h>

typedef struct map map;

struct map{
  int key;
  int value;
  map *next;
};

void add(map m, int k, int v){

}

int main(int argc, char *argv[]){
  map *m = (map *)malloc(sizeof(map));
  add(m, 0, 1);
  add(m, 2, 2);
  return 0;
}
