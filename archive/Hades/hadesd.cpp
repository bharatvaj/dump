#define HADES_D "HADES DAEMON"
#include <iostream>
#include <vector>
#include <thread>
#include <signal.h>

#if defined(__linux__) || defined(__APPLE__)
#include <fcntl.h>
#include <sys/stat.h>
#elif defined(_WIN32)
#include <process.h>
#include <dir.h>
#else
#error Unknown OS
#endif

#include <opencv2/core.hpp>
#include <opencv2/imgproc.hpp>
#include <opencv2/face.hpp>
#include <opencv2/highgui.hpp>
#include <sqlite3.h>

#include "clog/clog.h"
#include "Node/Node.h"
#include "init.h"
#include "shell.h"

using namespace std;
using namespace cv;
using namespace cv::face;


int help(int, char **, node::Node *);

int inform(enum folks folk);

//int label = 0;

vector<Mat> images;
vector<int> labels;
Ptr<FaceRecognizer> model;

vector<Mat> suspects;

int i_n = 0;
char *zErrMsg = 0;
int sqlite_callback(void *NotUsed, int argc, char**argv, char **azColName){
	int i;
	for(i=0; i<argc; i++){
		log_inf(HADES_D, "callback for sqlite3, executed the command");
		//TODO print executed command
	}
	return 0;
}

void *start_dvr(void *L){
	log_inf(HADES_D, "DVR started");
	VideoCapture cap;
	if(!cap.open(0)){
		log_err(HADES_D, "cannot open CAM");
		return NULL;
	}
	cv::Mat image;
	model = createEigenFaceRecognizer();
	cv::Mat grayMat;
	//
	cap >> image;
	images.push_back(image);
	labels.push_back(i_n++);
	model -> train(images, labels);
	log_err(HADES_D, "First frame is given as sample for training");
	while(true){
		cap >> image;
		/*
		cv::imshow("Hades", image);
		try {
			cv::waitKey(100);
		} catch(...){
			log_err(HADES_D, "error in the code");
		}
		*/
		//this function is used to process frames
		//verification part
		cv::cvtColor(image, grayMat, cv::COLOR_BGR2GRAY);
		int predicted = -1;
		if( (predicted = model -> predict(grayMat)) == 0){
			//new member
			if(false){ //FIXME enter only if the detected member is a new member
				images.push_back(image);
				labels.push_back(i_n++);
				model->train(images, labels);
			}
			else {
				suspects.push_back(grayMat);
				sleep(2);
				log_inf(HADES_D, "warn the user");
				//TODO alert through app
			}
		}
		//log_inf(HADES_D, "predicted = %d", predicted);
	}
	return NULL;
}

sqlite3 *db;

void exit_operations(){
	printf("\n");
	log_inf(HADES_D, "writing to Database...please wait");
	//sqlite3_exec(db, "select * from table1", sqlite_callback, 0 ,&zErrMsg);
	sqlite3_free(zErrMsg);
	sqlite3_close(db);
	//write suspect and persons to db and exit
	exit(0);
}

void exitHandler(int sig){
	exit_operations();
}


int open_hades_db(const char *pathToDB){
	if(sqlite3_initialize() != SQLITE_OK){
		log_fat(HADES_D, "cannot initialize sqlite3 library");
		return -1;
	}
	int ret = 0;
        // open connection to a DB
        if ((ret = sqlite3_open(pathToDB, &db)) != SQLITE_OK){ //FIXME directory work
		log_fat(HADES_D, "cannot create db, error code : %d", ret);
		return -1;
	}
	log_inf(HADES_D, "database is opened");
	return 0;
}
//returns 0 on success
//loads images, labels, suspects, suspectsl
int load_trained_data(){
	//retreive path form sql and push to stacks
	return 0;
}

void modules_thread(void *opt){
	//while searching for available devices if device found, let them get seen by the user
	//if user desires, configure each devices
	//and save their id for future use until reset
	//if the id fails the devices is connecting for the first time or the device has been resetted
	//return -1;
}


int list_devices(int count, char **args, node::Node *n){
	char *str = (char *)malloc(512);
	strcpy(str, "DHT11 \t For measuring temperature and Humidity\nPIR Sensor \t For detecting movements\nCamera Module \t Not working\n");
	n->writeln(str);
	return 0;
}

int echo(int count, char **args, node::Node *n){
	char *buffer = (char *)malloc(256);//FIXME
	for(int i = 0; i < count; i++){
		strcat(buffer, args[i]);
	}
	n->writeln(buffer);
	return 0;
}

node::Node::job jobs[] = {
	{"help", "prints this help line", help},
	{"list", "list the devices connected to the server", list_devices},
	{"echo", "echoes the sent line", echo}
};
int jlen =  sizeof(jobs)/sizeof(node::Node::job);

int help(int count, char **args, node::Node *n){
		char *buf = (char *)malloc(2048);
    for(int i = 0; i < jlen; i++){
				char *buffer = (char *)malloc(2048);
        sprintf(buffer, "%s\t - %s\n", jobs[i].command, jobs[i].info);
				strcat(buf, buffer);
        for(int j = 0; j < jobs[i].opt_length; j++){
            sprintf(buffer, "  |--%s[-%c] - %s\n", jobs[i].options[j].word, jobs[i].options[j].letter, jobs[i].options[j].description);
						strcat(buf, buffer);
        }
    }
		n->writeln(buf);
    return 0;
}

int handle_client(node::Node *client){
if(strncmp(client->readln(), "CON_REQ", 7) != 0){
	log_inf(HADES_D, "Client Unrecognized");
	client->writeln("Protocol Mismatch");
	return -1;
}
	client->writeln("CON_ACK");
	log_inf(HADES_D, "Client connected, waiting for commands");
	while(client->process(jlen, jobs, client->readln()) != -1){
		log_inf("SERVER", "Request Processed");
	}
	return 0;
}

int main(int argc, char *argv[]){
	init_hades();
	signal(SIGINT, exitHandler);
	if(open_hades_db("db.sqlite3")< 0) //TODO clean up return types **db_error
		return -1;
	log_inf(HADES_D, "Database loaded successfully");
	if(load_trained_data() != 0){ //no trained data
		//TODO train data
	}
	//capture from camera
	//process the frames
	pthread_t dvr_thread;
	pthread_create(&dvr_thread, NULL, start_dvr, NULL);

	log_inf(HADES_D, "Main thread continues");
	//if the stored person is in DB do not alert the home
	//else if check he is breaking in to the house the inform(folks)
	enum folks folk = USER;
	while(inform(folk) == -1){}

	//async--> if there is a fire in the house.
	//NOTE alloted for Dinesh Kumar , DHT22 :{}

		node::Node *n = new node::Node();
		int pid;
		while(1){ //accept infinite connections
			node::Node *client = n -> accept(12300);
			if(client == NULL){
				printf("Internal Error\n");
				return -1;
			}
			pid = fork();
			if(pid == 0){
				//child
				delete n;
				return handle_client(client);
			} else {
				//parent
				if(pid == -1){
					printf("fork failed\n");
					return -1;
				} else {
					delete client;
				}
			}
		}
	pthread_join(dvr_thread, NULL);
	return 0;
}

//TODO use with terminology of levels
// inform the folks in the home that there is a stranger
int inform(enum folks folk){
	if(folk == USER){
		//alert the user
		return 0;
	}
	else if(folk == POLICE){
		//alert the police and user
		return 0;
	}
	else if(folk == ALL){
		//alert user and police
		return 0;
	}
	else return -1; //TODO rather returning failure, try to inform the user using messgage like hike!
}
