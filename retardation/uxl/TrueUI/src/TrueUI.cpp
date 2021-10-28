#include <TrueUI/TrueUI.hpp>

#include <iostream>

TrueUI::TrueUI(/* args */)
{
}

TrueUI::~TrueUI()
{
}

void TrueUI::add(TrueUIFnCallback fn) {
    this->fns.push_back(fn);
}

int TrueUI::run(){
    for(auto fn: fns){
        Iui* uiSpec = fn(Iin());
        std::cout << uiSpec->value << std::endl;
    }
    return 0;
}