#ifndef INIT_H
#define INIT_H
#include <stdio.h>
#ifdef _WIN32
#include <windows.h>
#elif _POSIX_
#include <unistd.h>
#endif
#include "config.h"

enum folks {
  USER,
  POLICE,
  ALL
};

const char *authors[] = {"Dinesh Kumar", "Goutham", "Gowri Shankar", "Bharatvaj", "Jothi Kumar", "Dhipauk Joqim"};

const char *program_logo = " ██░ ██  ▄▄▄      ▓█████▄ ▓█████   ██████ \n▓██░ ██▒▒████▄    ▒██▀ ██▌▓█   ▀ ▒██    ▒ \n▒██▀▀██░▒██  ▀█▄  ░██   █▌▒███   ░ ▓██▄   \n░▓█ ░██ ░██▄▄▄▄██ ░▓█▄   ▌▒▓█  ▄   ▒   ██▒\n░▓█▒░██▓ ▓█   ▓██▒░▒████▓ ░▒████▒▒██████▒▒\n ▒ ░░▒░▒ ▒▒   ▓▒█░ ▒▒▓  ▒ ░░ ▒░ ░▒ ▒▓▒ ▒ ░\n ▒ ░▒░ ░  ▒   ▒▒ ░ ░ ▒  ▒  ░ ░  ░░ ░▒  ░ ░\n ░  ░░ ░  ░   ▒    ░ ░  ░    ░   ░  ░  ░  \n ░  ░  ░      ░  ░   ░       ░  ░      ░  \n                   ░                      \n";


//Below are for debugging purposes and to maintain builds only
enum build_type {
  ALPHA=0,
  BETA=1,
  STABLE=5
};

void clear(){
  #ifdef _WIN32
  system("cls");
  #elif _POSIX_
  system("clear");
  #endif
}


const char *build_type = hades_VERSION_MINOR >= STABLE? "stable" : hades_VERSION_MINOR == ALPHA ? "alpha" : "beta";

void print_authors(){
  printf("developed by ");
  for(int i = 0; i < (signed int)(sizeof(authors)/sizeof(char *)); i++){
	  if(i == 0)
		  printf("%s", authors[i]);
	  else printf(", %s ", authors[i]);
   }
  printf("\n");
}
void init_hades(){
	std::ios_base::sync_with_stdio(false);
	printf("%s",program_logo);
	printf("%s v%d.%d-%s\n", PROGRAM_NAME, hades_VERSION_MAJOR, hades_VERSION_MINOR, build_type);
	print_authors();
}
#endif
