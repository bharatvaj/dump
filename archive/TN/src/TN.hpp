#ifndef _TN
#define _TN "TN"

#include <csignal>
#include <iostream>
#include <map>
#include <functional>

#include <clog/clog.h>
#include <em/EventManager.hpp>
#include <view/View.hpp>
#include <TNProcess.hpp>

namespace tn
{
enum class TNEvent
{
    Close
};
class TN : public em::EventManager<TNEvent, View *>
{
  private:
    View *view;
    TNProcess *process;

    void onProcess(View *v)
    {
        bool consent = v->alert("Start Process?", "The process might take some time to complete.\nAre you sure you want to continue?");
        if (consent)
        {
            process->start(std::string(view->getExcelPath()), std::string(view->getImagePath()));
        }
    }

    void onClose(View *v)
    {
        fireEvent(TNEvent::Close, view);
    }

    void onProcessInvalid(TNProcessEvent processEvent)
    {
        const char *process_invalid_title = "File Invalid";
        switch (processEvent)
        {
        case TNProcessEvent::InvalidExcel:
            view->error(process_invalid_title, "The provided excel file is invalid.\nMake sure the file is not corrupt or you have sufficient access");
            break;
        case TNProcessEvent::InvalidImage:
            view->error(process_invalid_title, "The provided image file is invalid.\nMake sure the file is not corrupt or you have sufficient access");
            break;
        }
    }

    void onProcessFail(TNProcessEvent processEvent)
    {
        const char *process_fail_title = "Process Fail";
        switch (processEvent)
        {
        case TNProcessEvent::FailConvert:
            view->error(process_fail_title, "Cannot convert files");
            break;
        case TNProcessEvent::FailExtract:
            view->error(process_fail_title, "Cannot extract from given files");
            break;
        case TNProcessEvent::FailMatch:
            view->error(process_fail_title, "Image and Excel do not match");
            break;
        case TNProcessEvent::FailDraw:
            view->error(process_fail_title, "Cannot draw to file");
            break;
        }
    }

  public:
    TN()
    {
        view = new View();
        process = new TNProcess();

        view->addEventHandler(ViewEvent::Process, this, &TN::onProcess);
        view->addEventHandler(ViewEvent::Close, this, &TN::onClose);

        view->setExcelPath("/home/laz3r/Desktop/sample.jpg");
        view->setImagePath("/home/laz3r/Desktop/sample.jpg");

        process->addEventHandler(TNProcessEvent::Fail, this, &TN::onProcessFail);
        process->addEventHandler(TNProcessEvent::Invalid, this, &TN::onProcessInvalid);
    }
    int run()
    {
        return view->run();
    }
};
}
#endif