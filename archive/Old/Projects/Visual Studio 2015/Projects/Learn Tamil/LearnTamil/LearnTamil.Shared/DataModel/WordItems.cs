using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace LearnTamil.DataModel
{
    public class WordItems
    {
        public string ChapterName { set; get; }
        public string ChapterNo { set; get; }
        public bool Locked { set; get; }
        public ImageBrush ChapterImage { set; get; }
        public double GridHeight
        {
            get
            {
                if (IsVertical()) return Window.Current.Bounds.Height / 2.5;
                else return Window.Current.Bounds.Width / 2.5;
            }
        }
        public double GridWidth
        {
            get
            {
                if (IsVertical()) return Window.Current.Bounds.Width;
                else return Window.Current.Bounds.Height;
            }
        }
        public double FullHeight
        {
            get
            {
                return Window.Current.Bounds.Height;
            }
        }
        public WordItems(string name, string no,ImageBrush im,bool locked)
        {
            this.ChapterName = name;
            this.ChapterNo = no;
            this.ChapterImage = im;
            this.Locked = locked;
        }
        private bool IsVertical()
        {
            if (Window.Current.Bounds.Height > Window.Current.Bounds.Width) return true;
            else return false;
        }
    }
}
