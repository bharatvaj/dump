using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LearnTamil.DataModel
{
    public class Title
    {
        public string SubtitleName { set; get; }
        public string SubtitleText { set; get; }

        public StackPanel ChildStack { set; get; }

        public Title(StackPanel child)
        {
            ChildStack = child;
        }
        public Title(string subtitle,string content)
        {
            this.SubtitleName = subtitle;
            this.SubtitleText = content;
        }
        #region UIData
        private bool IsVertical()
        {
            if (Window.Current.Bounds.Height > Window.Current.Bounds.Width) return true;
            else return false;
        }
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
        #endregion
    }
}
