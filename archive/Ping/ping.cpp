#include <iostream>
#include <Ping.hpp>
#include <Dot/Dot.hpp>

using namespace ping;

int main(int argc, char *argv[])
{
	Ping p;
	return p.run();
}
