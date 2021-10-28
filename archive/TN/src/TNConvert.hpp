#ifndef _TN_CONVERT
#define _TN_CONVERT "TNConvert"
#include <iostream>
#include <fstream>
#include <em/EventManager.hpp>

//#include <CSV.hpp>
#include <model/TNExcel.hpp>
#include <model/TNImage.hpp>

using namespace std;
namespace tn
{
class TNConvert
{
  private:
    std::string excelPath;
    std::string imagePath;

  public:
    TNConvert()
    {
    }

    std::string startExcel(std::string excelPath)
    {
        return excelPath;
    }

    std::string startImage(std::string imagePath)
    {
        return imagePath;
    }
};
}
#endif