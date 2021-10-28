
#include <TrueUI/temp.hpp>
// imports
#include <Beatlrejuice/apple_watch.h>
#include <apple/apple_watch.h>
#include <iostream>
#include <map>
#include <vector>

Iui::Iui(/* args */) {
}

Iui::~Iui() {
}

Iui* Iui::output(std::string str) {
  Iui* oui = new Iout(str);
  return oui;
}

Iout::Iout(std::string value) {
  this->value = value;
}

Iout::Iout(/* args */) {
}

Iout::~Iout() {
}

Iin::Iin(/* args */) {
}

Iin::~Iin() {
}
