#define HADES "HADES"
#include <iostream>
#include <FL/Fl.H>
#include <FL/Fl_Window.H>
#include <thread>

#include "clog.h"
#include "shell.h"
#include "ui.h"
#include "clog/clog.h"
#include "Node/Node.h"

int help(int, char **);

node::Node *client;


//Fl_OpenCV *open_highgui;

/*
 * Checks and sends the data to the server
 */
int send_data(int count, char **args){
	if(count < 1)return -2;
	int file_i = 0;
	for(file_i = 0; file_i < count; file_i++){
		if(client->writeln(args[file_i]) < 0){ //TODO replace with write_file
			printf("write failed\n");
			break;
		}
	}
	if(file_i == count)return 0;
	return -1;
}

int display_data(int count, char **args){
	if(count < 1)return -2;
	return -1;
}

int list_devices(int count, char **args){
	client->writeln("list");
	char *buf = NULL;
	while((buf = client->readln()) != NULL){
		printf("%s\n", buf);
	}
	return -1;
}

int send_raw(int count, char **args){

	if(count < 1)return -2;
	
	int file_i = 0;
	for(file_i = 0; file_i < count; file_i++){
		cout << args[file_i] << endl;
		client->writeln(args[file_i]);
		printf("%s\n",  client->readln());
	}
	if(file_i == count)return 0;
	return -1;
}

int server_shell(){
	char *buf = (char *)malloc(256);
	int c = '\0';
	int ptr = 0;
	while( (c = getchar()) != '\n' ){
		buf[ptr++] = c;
	}
	buf[ptr] = '\0';
	client->writeln(buf);
	printf("%s\n", client->readln());
	return 0;
}
	

int hades_connect(int count, char **args){
	if(count < 2)return -2;
	log_inf(HADES, "%s,  %s",  args[0], args[1]);
	client = new node::Node();
	if(client->connect(args[0], atoi(args[1])) < 0)return -1;

	client->writeln("CON_REQ", 7);
	if(strncmp("CON_ACK", client->readln(), 7) != 0){
	return -1;
	}
		log_inf("AUTH", "Key exchange suceeded");
		int pid = fork();
		if(pid == 0){
			//child
			return server_shell();
		} else {
			//parent
			if(pid == -1){
				printf("fork failed\n");
				return -1;
			} else {
				delete client;
			}
		}
		return 0;
}

job jobs[] = {
	{"help", "prints this help line", help},
	{"connect", "connect to a hades server", hades_connect},
	{"raw", "send raw data for processing commands, for debugging purposes only", send_raw},
	{"show", "displas the selected data", display_data},
	{"list", "lists the connected devices with the server", list_devices},
	{"send", "sends data to the server for processing", send_data}
};

int jlen =  sizeof(jobs)/sizeof(job);

/*
 * Help message always returns 0
 */
int help(int count, char **args){
	if(count > 1){
		for(int arg_i = 0; arg_i < count; arg_i++){
			sh_help(jlen, jobs, args[arg_i]);
		}
		return 0;
	}
	sh_help(jlen, jobs);
	return 0;
}

int main(int argc, char *argv[]){
	std::ios_base::sync_with_stdio(false);
	log_inf(HADES, "Starting GUI");
	//connect_server("localhost", 12300); //TECH DEPT
	if(argc < 2){
		load_ui(jlen, jobs, false, "hades> ");
	} else load_ui(jlen, jobs);
	return 0;
}
