using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace File360
{
    class MusicList
    {

        public double ImageHeight
        {
            get
            {
                if (IsVertical()) return Window.Current.Bounds.Height / 3.5;
                else return Window.Current.Bounds.Width / 3.5;
            }
        }
        public double ImageWidth
        {
            get
            {
                if (IsVertical()) return Window.Current.Bounds.Width / 2.2;
                else return Window.Current.Bounds.Height / 2.2;
            }
        }

        public double PicHeight
        {
            get
            {
                if (IsVertical()) return (Window.Current.Bounds.Height) / 2.5;
                else return (Window.Current.Bounds.Width) / 2.5;
            }
        }
        public double PicWidth
        {
            get
            {
                if (IsVertical()) return Window.Current.Bounds.Width / 2;
                else return Window.Current.Bounds.Height / 2;
            }
        }
        public ImageBrush Background { set; get; }
        public string SongName { set; get; }
        public string ArtistName { set; get; }
        public string AlbumName { set; get; }
        public string Count{ set; get; }
        public string FolderName { get; set; }

        public MusicList(string songName, string artistName, string albumName, ImageBrush background)
        {
            this.SongName = songName;
            this.ArtistName = artistName;
            this.AlbumName = albumName;
            this.Background = background;
        }

        public MusicList(string folderName, int count)
        {
            this.FolderName = folderName;
            this.Count = count.ToString();
        }


        private bool IsVertical()
        {
            if (Window.Current.Bounds.Height > Window.Current.Bounds.Width) return true;
            else return false;
        }
    }

}
