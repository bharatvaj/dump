#pragma once

#include <string>

class Iui {
private:
  /* data */
public:
  std::string value;
  Iui(/* args */);
  ~Iui();
  Iui* output(std::string);
};

class Iout : public Iui {
private:
  /* data */
public:
  Iout(std::string);
  Iout(/* args */);
  ~Iout();
};

class Iin : public Iui {
private:
  /* data */
public:
  Iin(/* args */);
  ~Iin();
};
