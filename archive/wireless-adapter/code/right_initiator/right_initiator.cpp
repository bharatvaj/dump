#include <Bluetooth.hpp>
#include <GPIO.hpp>
#include <LED.hpp>
#include <iostream>
#include <thread>
#include <unistd.h>
#include <signal.h>

#define BUF_SIZE 256

using namespace std;
using namespace gpio;

xs_SOCKET x;

void deviceConnected(xs_SOCKET s)
{
	x = s;
	if(s == -1){
		fmt::print("Error creation of socket\n");
		return;
	}
	fmt::print("Device connected\n");
	int pin = 17;
	GPIO *g = GPIO::getGPIO(pin);
	if (g == nullptr)
	{
		fmt::print("Cannot open GPIO {}\n", pin);
		return;
	}
	fmt::print("led going to create\n");
	LED led(g);
	int bytesRead = 0;
	char *msg = (char *)calloc(sizeof(char), BUF_SIZE);
	fmt::print("going to read\n");
	while ((bytesRead = read(s, msg, BUF_SIZE - 1)) > 0)
	{
		msg[bytesRead] = '\0';
		if (strncmp("rmk on", msg, 6) == 0)
		{
			led.on();
		}
		else
		{
			led.off();
		}
		printf("%s", msg);
	}
 	fmt::print("EOF\n");
}

void exit_handler(int sig){
	close(x);
	exit(EXIT_SUCCESS);
}

int main(int argc, char *argv[])
{
	signal(SIGINT, exit_handler);
	Bluetooth b;
	b.setOnDeviceConnected(deviceConnected);
	if (b.startSlave())
	{
		fmt::print("Slave running\n");
	}
	else
	{
		fmt::print("Cannot start slave\n");
	}
	while (1)
		;
	return 0;
}
