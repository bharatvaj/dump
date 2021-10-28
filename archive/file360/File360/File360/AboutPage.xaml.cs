using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace File360
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
            SolidColorBrush red = new SolidColorBrush();
            SolidColorBrush green = new SolidColorBrush();
        }

        private void blue_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SolidColorBrush bluec = new SolidColorBrush();
            bluec.Color = Colors.Blue;
            bgChanger.Fill = bluec;
        }

        private void red_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void green_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }
    }
}