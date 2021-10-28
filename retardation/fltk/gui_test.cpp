#include <FL/Fl.H>
#include <FL/Fl_Window.H>
#include <FL/Fl_Box.H>

int main(int argc, char* argv[]){
	Fl_Window* window = new Fl_Window(340, 180);
	Fl_Box* box = new Fl_Box(20, 40, 300, 100, "Hello, World!");
	box->box(FL_UP_BOX);
	box->labelfont(FL_BOLD);
	window->end();
	window->show(argc, argv);
	return Fl::run();
}
