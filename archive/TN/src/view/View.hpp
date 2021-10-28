#ifndef _TN_VIEW
#define _TN_VIEW "View"

#include <iostream>
#include <clog/clog.h>
#include <FL/Fl.H>
#include <FL/Fl_Double_Window.H>
#include <FL/Fl_Button.H>
#include <FL/Fl_Input.H>
#include <FL/Fl_File_Chooser.H>

#include <em/EventManager.hpp>

namespace tn
{
class View;
}

namespace tn
{
const char *app_name = "TN";
const int EXCEL = 0;
const int IMAGE = 1;

enum class ViewEvent
{
    Start,
    Process,
    Close
};

class View : public em::EventManager<ViewEvent, View *>
{

  private:
    Fl_Double_Window *window;
    int _x = 0;
    int _y = 20;
    int leftMargin = 20;
    int topMargin = 20;
    int windowWidth = 500;
    int windowHeight = 256;

    int buttonWidth = 60;
    int buttonHeight = 40;

    Fl_Input *imageInput = nullptr;
    Fl_Input *excelInput = nullptr;

    const char *name[20] = {
        "Excel",
        "Image"};

    const char *filter_excel[20] = {
        "*.xlsx"
        "*.xls",
    };

    const char *filter_image[20] = {
        ".tiff",
        ".jpg",
        "*png"};

    static void
    processCallback(Fl_Widget *w, void *o)
    {
        View *view = (View *)o;
        view->fireEvent(ViewEvent::Process, view);
        //TNEvent::getInstance()->fireEvent(Event::Process, view);
    }

    static void browseExcel(Fl_Widget *w, void *o)
    {
        View *v = (View *)o;
        v->runFileChooser(EXCEL);
    }
    static void browseImage(Fl_Widget *w, void *o)
    {
        View *v = (View *)o;
        v->runFileChooser(IMAGE);
    }

    static void windowCallback(Fl_Widget *w, void *o)
    {
        View *view = (View *)o;
        view->fireEvent(ViewEvent::Close, view);
    }

    Fl_Double_Window *createWindow()
    {
        Fl_Double_Window *window = new Fl_Double_Window(0, 0, windowWidth, windowHeight, app_name);
        window->callback(windowCallback, this);
        return window;
    }

    Fl_Widget *createField(int opt)
    {
        //Fl_Frame *frame = new Fl
        int textBoxWidth = windowWidth - buttonWidth - (2 * leftMargin);
        int textBoxHeight = buttonHeight;
        Fl_Input *text = new Fl_Input(leftMargin, _y, textBoxWidth, textBoxHeight);
        Fl_Button *btn = new Fl_Button(leftMargin + textBoxWidth, _y, buttonWidth, buttonHeight, name[opt]);
        if (opt == EXCEL)
        {
            excelInput = text;
            btn->callback(browseExcel, this);
        }
        else if (opt == IMAGE)
        {
            imageInput = text;
            btn->callback(browseImage, this);
        }
        _y += buttonHeight + 20;
        return btn;
    }

    Fl_Button *createProcessButton()
    {
        Fl_Button *btn = new Fl_Button(leftMargin, _y, buttonWidth, buttonHeight, "Process");
        btn->callback(processCallback, this);
        return btn;
    }

  public:
    const char *getExcelPath()
    {
        return excelInput->value();
    }

    const char *getImagePath()
    {
        return imageInput->value();
    }

    void setExcelPath(const char *excelPath)
    {
        excelInput->value(excelPath);
    }

    void setImagePath(const char *imagePath)
    {
        imageInput->value(imagePath);
    }

    bool alert(std::string title, std::string message)
    {
        fl_message_title(title.c_str());
        switch (fl_choice((message).c_str(), "Yes", "No", 0))
        {
        case 0:
            return true;
        case 1:
            return false;
        }
    }

    void error(std::string title, std::string message)
    {
        fl_message_title(title.c_str());
        fl_choice((message).c_str(), "OK", 0, 0);
    }
    bool checkFieldValidity()
    {
        if (excelInput == nullptr || imageInput == nullptr)
        {
            return false;
        }
        if (strcmp(excelInput->value(), "") == 0 && strcmp(imageInput->value(), "") == 0)
        {
            return false;
        }
        return true;
    }
    bool runFileChooser(int opt)
    {
        Fl_File_Chooser *chooser = new Fl_File_Chooser("", "", Fl_File_Chooser::SINGLE, "");

        switch (opt)
        {
        case EXCEL:
        {
            chooser->filter(*filter_excel);
            chooser->show();
            while (chooser->shown())
            {
                Fl::wait();
            }
            const char *path = chooser->value();
            if (path == NULL)
            {
                log_err(_TN_VIEW, "Excel not valid");
                return false;
            }
            excelInput->value(path);
            return true;
        }
        case IMAGE:
        {
            chooser->filter(*filter_image);
            chooser->show();
            while (chooser->shown())
            {
                Fl::wait();
            }
            const char *path = chooser->value();
            if (path == NULL)
            {
                log_err(_TN_VIEW, "Image not valid");
                return false;
            }
            imageInput->value(path);
            return true;
        }
        }
        return false;
    }

    View()
    {
        window = createWindow();
        window->add(createField(EXCEL));
        window->add(createField(IMAGE));
        window->add(createProcessButton());
    }

    Fl_Window *getWindow()
    {
        return window;
    }
    int run()
    {
        window->show();
        return Fl::run();
    }
};
}
#endif