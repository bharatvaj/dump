#ifndef _TN_EXCEL
#define _TN_EXCEL
#include <iostream>
using namespace std;
class TNExcel
{
  protected:
    string cNum;
    string cName;
    string cType;

  public:
    TNExcel()
    {
    }
    TNExcel(string cNum, string cName, string cType)
    {
        this->cNum = cNum;
        this->cName = cName;
        this->cType = cType;
    }
    string getCNum()
    {
        return cNum;
    }
    string getCName()
    {
        return cName;
    }
    string getCType()
    {
        return cType;
    }
    void setCNum(string cNum)
    {
        this->cNum = cNum;
    }
    void setCName(string cName)
    {
        this->cName = cName;
    }
    void setCType(string cType)
    {
        this->cType = cType;
    }
};
#endif