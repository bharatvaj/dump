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
    public sealed partial class SideBarMenuButton : Button
    {
        public SideBarMenuButton()
        {
            this.InitializeComponent();
            Width = (Window.Current.Bounds.Width*2)/3;
            Height = Window.Current.Bounds.Height / 12;
        }
        public string ImageText
        {
            get { return TextImage.Text; }
            set { TextImage.Text = value; }
        }
        public string ContentText
        {
            get { return Text.Text; }
            set { Text.Text = value; }
        }
        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)LayoutGrid.Background; }
            set { LayoutGrid.Background = value; }
        }
    }
}
