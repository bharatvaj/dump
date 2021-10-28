#ifndef _TN_PROCESS
#define _TN_PROCESS "TNProcess"
#include <em/EventManager.hpp>
#include <TNConvert.hpp>
#include <TNExtract.hpp>
#include <TNMatch.hpp>
#include <TNDraw.hpp>

namespace tn
{
enum class TNProcessEvent
{
    Invalid,
    InvalidExcel,
    InvalidImage,
    Fail,
    FailConvert,
    FailExtract,
    FailMatch,
    FailDraw
};
class TNProcess : public em::EventManager<TNProcessEvent, TNProcessEvent>
{
  private:
    TNConvert *convert;
    TNExtract *extract;
    TNMatch *match;
    TNDraw *draw;

  public:
    TNProcess()
    {
        convert = new TNConvert();
        extract = new TNExtract();
        match = new TNMatch();
        draw = new TNDraw();
    }
    bool openExcel(std::string excelFilePath)
    {
        struct stat buffer;
        if (stat(excelFilePath.c_str(), &buffer) != 0)
        {
            return false;
        }
        return true;
    }

    bool openImage(std::string imageFilePath)
    {
        struct stat buffer;
        if (stat(imageFilePath.c_str(), &buffer) != 0)
        {
            return false;
        }
        return true;
    }

    void start(std::string excelPath, std::string imagePath)
    {
        if (!openExcel(excelPath))
        {
            fireEvent(TNProcessEvent::Invalid, TNProcessEvent::InvalidExcel);
            return;
        }
        if (!openImage(imagePath))
        {
            fireEvent(TNProcessEvent::Invalid, TNProcessEvent::InvalidImage);
            return;
        }

        std::string convertedExcelPath = convert->startExcel(excelPath);
        std::string convertedImagePath = convert->startImage(imagePath);
        if (convertedExcelPath.empty() || convertedImagePath.empty())
        {
            fireEvent(TNProcessEvent::Fail, TNProcessEvent::FailConvert);
            return;
        }
        std::vector<TNExcel> extractedExcel = extract->getExcelInfo(convertedExcelPath);
        std::vector<TNImage> extractedImage = extract->getImageInfo(convertedImagePath);
        if (extractedExcel.size() == 0 || extractedImage.size() == 0)
        {
            fireEvent(TNProcessEvent::Fail, TNProcessEvent::FailExtract);
            return;
        }
        std::vector<TNImage> completeTNImage = match->start(extractedExcel, extractedImage);
        if (completeTNImage.size() == 0)
        {
            fireEvent(TNProcessEvent::Fail, TNProcessEvent::FailMatch);
            return;
        }
        std::string outputPath = draw->start(completeTNImage);
        if (outputPath.empty())
        {
            fireEvent(TNProcessEvent::Fail, TNProcessEvent::FailDraw);
            return;
        }
    }
};
}
#endif