using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace File360
{
    class Addresser:StackPanel
    {
        //Button prev = new Button();
        public void InitializeComponent()
        {
            HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Orientation = Windows.UI.Xaml.Controls.Orientation.Horizontal;
            //StackPanel stk = new StackPanel();
            //stk.Orientation = Windows.UI.Xaml.Controls.Orientation.Horizontal;
            //TextBlock content = new TextBlock();
            //TextBlock txt = new TextBlock();
            //stk.Children.Add(content);
            //stk.Children.Add(txt);
            //prev.Content = stk;
        }

        public string Address
        {
            set
            {
                Button current = new Button();
                current.BorderThickness = new Thickness(0);
                StackPanel currentStk = new StackPanel();
                currentStk.Orientation = Windows.UI.Xaml.Controls.Orientation.Horizontal;
                TextBlock content = new TextBlock();
                TextBlock txtH = new TextBlock();
                current.Content = currentStk;
                txtH.FontFamily = new FontFamily("Assets/Font/iconFont.ttf#iconfont");
                txtH.Text = "r";
                txtH.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                txtH.FontSize = 25;
                txtH.Foreground = (SolidColorBrush)Application.Current.Resources["PhoneAccentBrush"];
                content.Text = value;
                currentStk.Children.Add(content);
                currentStk.Children.Add(txtH);
                current.Style = (Style)Application.Current.Resources["DefaultButton"];
                current.Background = null;
                current.Height = this.ActualHeight;
                current.MinWidth = 0;
                Children.Add(current);
            }
        }
        public void Reset()
        {
            Children.Clear();
        }
        public void SelectedFolder(int foldersNo)
        {
            for (int i = Children.Count-1; i < (Children.Count-foldersNo-1); i--)
            {
                Children.RemoveAt(i);
            }
        }
        
    }
}
