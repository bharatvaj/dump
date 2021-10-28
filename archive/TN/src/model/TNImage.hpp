#ifndef _TN_IMAGE
#define _TN_IMAGE

#include <model/TNExcel.hpp>
#include <model/Pos.hpp>

class TNImage : public TNExcel
{
private:
  Pos p;

public:
  TNImage(string cName, string cType, Pos p)
  {
    this->cNum = cNum;
    this->cName = cName;
    this->p = p;
  }

  void setPos(Pos p)
  {
    this->p = p;
  }

  Pos getPos()
  {
    return p;
  }
};
#endif