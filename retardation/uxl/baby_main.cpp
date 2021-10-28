#include <TrueUI/TrueUI.hpp>

// std::string download_file(){
//     return "";
// }


Iui* help(Iin ui) {
    Iui* oui = ui.output("hello");
    return oui;
}

// Iui* download(Iin ui) {
//     return ui.output(download_file());
// }


int main() {
    TrueUI tui;
    TrueUIFnCallback fn = [=](Iin ui) -> Iui*{
        Iui* oui = help(ui);
        return oui;
    };
    tui.add(fn);
    return tui.run();
}
