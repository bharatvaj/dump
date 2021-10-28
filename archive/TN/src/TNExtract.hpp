#ifndef _TN_EXTRACT
#define _TN_EXTRACT "TNExtract"
#include <iostream>
#include <vector>
#include <fstream>
#include <opencv2/opencv.hpp>
#include <opencv2/text/ocr.hpp>

#include <em/EventManager.hpp>
#include <clog/clog.h>
namespace tn
{
class TNExtract
{
public:
  TNExtract()
  {
  }

  void print(TNImage i)
  {
    log_inf(_TN_CONVERT, "(%s, %s) : %s", i.getCName().c_str(), i.getCType().c_str(), i.getCNum().c_str());
  }

  std::vector<TNExcel> getExcelInfo(std::string excelPath)
  {
    return vector<TNExcel>();
  }

  std::vector<TNImage> getImageInfo(std::string imagePath)
  {
    using namespace cv;
    std::vector<TNImage> tnImages;
    Mat image;
    image = imread(imagePath.c_str(), IMREAD_COLOR);
    if (!image.data) // Check for invalid input
    {
      log_inf(_TN_EXTRACT, "Could not open or find the image");
      return vector<TNImage>();
    }
    //Ptr<cv::text::OCRTesseract> ocr =
    //cv::text::OCRTesseract::create(NULL /*datapath*/, "eng" /*lang*/, "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890" /*whitelist*/, ocr_engine_mode::OEM_TESSERACT_ONLY, 10 /*psmode*/);

    string output;
    vector<Rect> boxes;
    vector<string> words;
    vector<float> confidences;
    //ocr->run(image, output, &boxes, &words, &confidences, cv::text::OCR_LEVEL_WORD);
    for (auto word : words)
    {
      tnImages.push_back(TNImage(word, word, Pos(0, 0)));
      log_inf(_TN_EXTRACT, word.c_str());
    }
    log_inf(_TN_EXTRACT, output.c_str());
    return vector<TNImage>();
  }
};
}
#endif