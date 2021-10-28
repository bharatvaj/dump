using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;

namespace File360
{
    public partial class DialogBox : UserControl
    {
        public DialogBox()
        {
            InitializeComponent();
        }
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
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
            Canvas can = (Canvas)DialogBoxControl.Parent;
            can.Children.Remove(DialogBoxControl);
        }

        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
