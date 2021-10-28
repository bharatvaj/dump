#include <iostream>
#include <Dot/Dot.hpp>

using namespace std;
using namespace dot;

int main(int argc, char *argv[]){
	Dot &dot = Dot::getDot(2500);
	dot.onConnect([](Dot &d){
		d.write("hello");
		cout << "Wrote Hello" << endl;
	});
	while(1);
	return 0;
}
