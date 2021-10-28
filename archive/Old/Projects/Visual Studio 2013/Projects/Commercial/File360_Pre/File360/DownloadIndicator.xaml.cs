using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace File360
{
    public sealed partial class DownloadIndicator : Button
    {
        public DownloadIndicator()
        {
            this.InitializeComponent();
            //Width = (Window.Current.Bounds.Width * 2) / 3;
            //Height = Window.Current.Bounds.Height / 10;
        }
        
        public string ContentText
        {
            get { return FileName.Text; }
            set { FileName.Text = value; }
        }
        public double ProgressBarValue
        {
            get { return progBar.Value; }
            set { progBar.Value = value; }
        }
        public double DownSpeed
        {
            get { return progBar.Value; }
            set { progBar.Value = value; }
        }
        //public SolidColorBrush Status
        //{
        //    get { return (SolidColorBrush)LayoutGrid.Background; }
        //    set { LayoutGrid.Background = value; }
        //}
    }
}
