#include<iostream>
#include<cstring>
#include<FL/Fl.H>
#include<FL/Fl_Window.H>
#include<FL/Fl_Double_Window.H>
#include<FL/Fl_Text_Display.H>
#include<FL/Fl_Menu_Bar.H>
#include<FL/Fl_Button.H>
#include <FL/Fl_Box.H>
#include<FL/Fl_Scroll.H>
using namespace std;

Fl_Double_Window *root = NULL;
Fl_Scroll *scroll = NULL;
Fl_Menu_Bar *menu_bar = NULL;
Fl_Text_Display *out_txt = NULL;

pthread_mutex_t _plock;
bool is_gui = false;

#define WIN_W 256
#define WIN_H 256+20

void println(const char *msg, ...){
	char *buffer = (char *)malloc(sizeof(msg)+1);
	pthread_mutex_lock(&_plock);
	va_list vl;
	va_start(vl, msg);
	vsprintf(buffer, msg, vl);
	va_end(vl);
	printf("%s\n", buffer);
	pthread_mutex_unlock(&_plock);
}

void gprintln(char *msg, ...){
    char *str = (char *)malloc(BUFFER_SIZE);
    va_list vl;
    va_start(vl, msg);
    vsprintf(str, msg, vl);
    va_end(vl);
    char *buffer = (char *)malloc(BUFFER_SIZE);
    sprintf(buffer, "%s\n", str);
    if(is_gui){
        out_txt->insert(buffer);
    }
    else {
        printf("%s", buffer);
    }
}


int initialize(){
	root = new Fl_Double_Window(0,0, WIN_W, WIN_H, "Hades");
	menu_bar = new Fl_Menu_Bar(0,0, WIN_W, 20);
	scroll = new Fl_Scroll(0,20, WIN_W, WIN_H);
	root->resizable(scroll);
	return 0;
	
}

class Fl_Grid : public Fl_Group {
	int pos_x = 0;
	int pos_y = 0;
	public:
	Fl_Grid(int _x, int _y, int _w, int _h) : Fl_Group::Fl_Group(_x, _y, _w, _h) {
		pos_x += _x;
		pos_y += _y;
	}
	void add(Fl_Widget *widg, int _x = 0, int _y = 0){
		Fl_Group::add(widg);
		printf("%d %d\n", _x, _y);
	}

};

const char *rooms[] = {
	"Living Room", 
	"Kitchen",
	"Garage",
	"Compound",
	"Bedroom",
	"Master Bedroom"
};

int rooms_size = sizeof(rooms)/sizeof(char *);

Fl_Group *temperature_room(int x, int y, int temp){
	Fl_Group *grp = new Fl_Group(0, 0, 256, 256);
	Fl_Text_Display *txt = new Fl_Text_Display(0,0,200, 50);
	txt->buffer(new Fl_Text_Buffer(29));
	char *buf = (char *)malloc(200);
	sprintf(buf, "Temperature: %d", temp);
	txt->insert(buf);
	grp->add(txt);
	Fl_Button *btn = new Fl_Button(0, 75, 150, 50, "Normalize Temp");
	grp->add(btn);
	return grp;
}
Fl_Group *camera_device(int x, int y){
	Fl_Group *grp = new Fl_Group(0, 0, 256, 256);
	Fl_Text_Display *txt = new Fl_Text_Display(0,0,200, 50);
	txt->buffer(new Fl_Text_Buffer(29));
	txt->insert("Camera");
	grp->add(txt);
	return grp;
}
Fl_Group *relay_switch(int x, int y){
	Fl_Group *grp = new Fl_Group(0, 0, 256, 256);
	Fl_Text_Display *txt = new Fl_Text_Display(0,0,200, 50);
	txt->buffer(new Fl_Text_Buffer(29));
	txt->insert("Relay Switch");
	grp->add(txt);
	return grp;
}
void btn_click(Fl_Widget *obj){
	static int cnt;
	Fl_Button *btn = (Fl_Button *)obj; //TODO get name from btn and compare it with rooms
	cnt++;
	Fl_Double_Window *child_win = new Fl_Double_Window(0,0, 256, 256);
	int c = cnt % 3;
	if(c == 0)
		child_win->add(temperature_room(0,0, 26));
	else if(c == 1)
		child_win->add(temperature_room(0,0, 32));
	else if(c == 2)
		child_win->add(temperature_room(0,0, 28));
	child_win->show();
}

Fl_Group *main_home(int x, int y){
	int count=0;
	int w = Fl::w() /3 ;
	int h = Fl::h() / 2;
	int columns = 3;
	Fl_Grid *grd = new Fl_Grid(0, 0, Fl::w(), Fl::h());
	for(int row  = 0 ; row < rooms_size/columns; row++)
	{
		for(int j = 0; j < columns ; j++){
		Fl_Button *btn = new Fl_Button(w*j,h*row, w, h, rooms[count++]);
		btn->callback(btn_click);
		grd->add(btn);
		}

	}
	return grd;
}

Fl_Group *camera_4(int _x, int _y){
	int x = _x;
	int y = _y;
	Fl_Grid *grp = new Fl_Grid(x,y,128, 2048);
	Fl_Box *box1 = new Fl_Box(0,0, 64, 64);
	box1->color(FL_GRAY0);
	box1->box(FL_UP_BOX);
	Fl_Box *box2 = new Fl_Box(0,0, 64, 64);
	box2->color(FL_GRAY0);
	box2->box(FL_UP_BOX);
	Fl_Box *box3 = new Fl_Box(64,64, 64, 64);
	box3->color(FL_GRAY0);
	box3->box(FL_UP_BOX);
	Fl_Box *box4 = new Fl_Box(64,0, 64, 64);
	box4->color(FL_GRAY0);
	box4->box(FL_UP_BOX);
	grp->add(box1);
	grp->add(box2);
	grp->add(box3, 64, 64);
	grp->add(box2, 0, 64);
	return grp;
}

int start_gui(int jlen, job *jobs){
	initialize();
	root->add(menu_bar);
	root->add(scroll);
	scroll->add(main_home(0,0));
	scroll->type(Fl_Scroll::BOTH_ALWAYS);
	root->show();
	return Fl::run();
}

int load_ui(int jlen, job *jbs,  bool _is_gui = true, const char *shell_name = "$ "){
    //add checks for CLI or GUI from args
    	is_gui = _is_gui;
	if(!_is_gui /*|| screen not found*/){
        //TODO if screen not found, run in CLI mode
        /*
         if(screen not found){
            println("screen not found continuing with command line interface");
         }
         */
		start_shell(jlen, jbs, shell_name);
	}
	else start_gui(jlen, jbs);
	return 0;
}
