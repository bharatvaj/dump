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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace File360
{
    public sealed partial class MessageBox : UserControl
    {
        public MessageBox()
        {
            InitializeComponent();
            heading.FontSize = Window.Current.Bounds.Width / 20;
            dialogDetails.FontSize = Window.Current.Bounds.Width / 30;
            leftButton.FontSize = Window.Current.Bounds.Width / 35;
        }
        public string HeadingText
        {
            get { return heading.Text; }
            set { heading.Text = value; }
        }
        public string ContentText
        {
            get { return dialogDetails.Text; }
            set { dialogDetails.Text = value; }
        }
        public string LeftButtonContent
        {
            get { return leftButton.Content.ToString(); }
            set { leftButton.Content = value; }
        }
        public string RightButtonContent
        {
            get { return rightButton.Content.ToString(); }
            set { rightButton.Content = value; }
        }
        public RoutedEventHandler LeftButtonHandler
        {
            get { return leftButton_Click; }
            set { leftButton.Click += value; }
        }

        public RoutedEventHandler RightButtonHandler
        {
            get { return rightButton_Click; }
            set { rightButton.Click += value; }
        }


        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }
        private void rightButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public bool IsOpen
        {
            get
            {
                if (Height == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set
            {
                if (value == true)
                {
                    Height = Window.Current.Bounds.Height;
                }
                if (value == false)
                {
                    Height = 0;
                }
            }
        }
    }
}
