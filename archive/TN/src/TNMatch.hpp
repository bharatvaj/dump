#ifndef _TN_MATCH
#define _TN_MATCH "TNMatch"
namespace tn
{
class TNMatch
{
public:
  /*
    * Returns a vector<TNImage> filled with extra information from paramm tnExcel
    */
  vector<TNImage> *getImageInfoExtra(vector<TNImage> *tnImages, vector<TNExcel> *tnExcels)
  {
    //copy values from tnImages
    vector<TNImage> *completeImage = new vector<TNImage>(*tnImages);
    for (const auto i : *completeImage)
    {
      //print(i);
    }
    //extract unique info from tnExcels

    return completeImage;
  }
  TNMatch()
  {
  }

  std::vector<TNImage> start(std::vector<TNExcel> extractedExcel, std::vector<TNImage> extractedImage)
  {
    std::vector<TNImage> filledImage = extractedImage;
    for (TNExcel excel : extractedExcel)
    {
      for (TNImage image : extractedImage)
      {
        if (excel.getCName() == image.getCName() && excel.getCType() == image.getCType())
        {
          image.setCNum(excel.getCNum());
        }
      }
    }
    return filledImage;
  }
};
}
#endif