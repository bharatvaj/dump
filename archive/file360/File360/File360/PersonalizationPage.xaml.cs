using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;

namespace File360
{
    public partial class PersonalizationPage : PhoneApplicationPage
    {
        public PersonalizationPage()
        {
            InitializeComponent();
        }

        private void SideMenuWallChanger_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            BitmapImage gh = MainPage.bm;
            gh = new BitmapImage(new Uri("pack://application:/Resources/Assets/Wallpaper/bg2.jpg", UriKind.Absolute));
            MainPage mp = new MainPage();
            mp.WallpaperChanger(gh);
        }
        
        private void Wallpaper_Checked(object sender, RoutedEventArgs e)
        {
            WallpaperTemp.Visibility = Visibility.Visible;
        }

        private void Wallpaper_Unchecked(object sender, RoutedEventArgs e)
        {
            WallpaperTemp.Visibility = Visibility.Collapsed;
        }
    }
}