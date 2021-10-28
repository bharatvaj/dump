using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace File360
{
    class VideoList
    {

        public ImageBrush Preview { set; get; }
        public string VideoName { set; get; }
        public string VideoDuration { set; get; }
        public string FolderName { get; set; }
        public string Count { set; get; }

        public VideoList(string videoName, TimeSpan videoDuration,ImageBrush preview)
        {
            this.VideoName = videoName;
            this.VideoDuration = videoDuration.ToString();
            this.Preview = preview;
        }

        public VideoList(string folderName, int count)
        {
            this.FolderName = folderName;
            this.Count = count.ToString();
        }
    }

}
