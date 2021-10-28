using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using File360;
using Windows.UI.Xaml.Controls.Primitives;

namespace File360
{
    public sealed partial class DialogBox : UserControl
    {
        public DialogBox()
        {
            InitializeComponent();
            Loaded += DialogBox_Loaded;
            heading.FontSize = Window.Current.Bounds.Width / 20;
            dialogDetails.FontSize = Window.Current.Bounds.Width / 30;
            leftButton.FontSize = Window.Current.Bounds.Width / 35;
            rightButton.FontSize = Window.Current.Bounds.Width / 35;
        }

        void DialogBox_Loaded(object sender, RoutedEventArgs e)
        {

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

        private void rightButton_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
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
