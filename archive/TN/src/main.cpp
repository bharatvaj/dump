#include <iostream>
#include <csignal>
#include <view/View.hpp>
#include <TN.hpp>
#include <em/EventManager.hpp>

#define _MAIN "MAIN"

using namespace tn;

void exit_handler(int sig)
{
    if (sig == SIGINT)
    {
        exit(EXIT_SUCCESS);
    }
}

void onClose(View *v)
{
    bool consent = v->alert("Exit?", "Are you sure you want to exit");
    if (consent)
    {
        log_inf(_MAIN, "Closing TN");
        exit_handler(SIGINT);
    }
}

int main(int argc, char *argv[])
{
    signal(SIGINT, exit_handler);
    TN *tn = new TN();
    tn->addEventHandler(TNEvent::Close, onClose);
    return tn->run();
}