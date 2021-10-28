using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace File360
{
    class PictureList
    {

        public ImageBrush Preview { set; get; }
        public string PicName { set; get; }
        public DateTime PicDate { set; get; } //pic created date.
        public string Count { set; get; }
        public string FolderName { get; set; }

        public PictureList(string picName, DateTime picDate, ImageBrush preview)
        {
            this.PicName = picName;
            this.PicDate = picDate;
            this.Preview = preview;
        }

        public PictureList(string folderName, int count)
        {
            this.FolderName = folderName;
            this.Count = count.ToString();
        }
    }

}
