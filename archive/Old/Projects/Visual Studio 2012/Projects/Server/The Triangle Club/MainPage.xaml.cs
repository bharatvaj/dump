using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.IO;
using System.Globalization;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : MetroWindow
    {
        public MainPage()
        {
            InitializeComponent();
            Rectangle sp = new Rectangle();
            sp.Height = 6;
            lateralMenu.Children.Add(sp);
            ImageBrush im = new ImageBrush();
            MenuSet("Dashboard", im, Colors.LimeGreen);
            MenuSet("AP", im, Colors.Teal);
            MenuSet("Stations", im, Colors.LimeGreen);
            MenuSet("Packets", im, Colors.Teal);
            MenuSet("Scripting", im, Colors.LimeGreen);
        }
        #region LateralMenu
        public bool menuOpen = false;
        private void lateralMenu_Click(object sender, RoutedEventArgs e)
        {
            if (menuOpen == false)
            {
                Thickness thick = new Thickness(0, 42, 0, 0);
                lateralMenu.Margin = thick;
                menuOpen = true;
            }
            else
            {
                Thickness thick = new Thickness(-170, 42, 0, 0);
                lateralMenu.Margin = thick;
                menuOpen = false;
            }
        }
        #endregion
        #region Media
        public void MenuSet(string title, ImageBrush icon, Color color)
        {
            TextBlock txt = new TextBlock();
            Rectangle rct = new Rectangle();
            txt.Foreground = new SolidColorBrush(Colors.Black);
            txt.FontSize += 5;
            rct.Fill = new SolidColorBrush(color);
            rct.Height = 60;
            rct.Width = 60;
            Rectangle sp = new Rectangle();
            sp.Height = 4;
            rct.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            rct.OpacityMask = icon;
            txt.Text = title;
            txt.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            WrapPanel grd = new WrapPanel();
            grd.Height = 60;
            grd.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            grd.Width = 165;
            grd.Children.Add(rct);
            grd.Children.Add(txt);
            grd.Background = new SolidColorBrush(Colors.White);
            lateralMenu.Children.Add(sp);
            lateralMenu.Children.Add(grd);
        }
        #endregion

    }
}