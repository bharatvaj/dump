using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace File360
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            List<settinglist> sl = new List<settinglist>();
            #region InitialSettings
            sl.Add(new settinglist("GENERAL", "configure general settings", "g"));
            sl.Add(new settinglist("FTP", "configure file transfer protocol", "f"));
            sl.Add(new settinglist("BLUETOOTH", "bluetooth settings", "b"));
            sl.Add(new settinglist("PERSONALIZATION", "get a look", "p"));
            sl.Add(new settinglist("SECURITY", "protect your files", "s"));
            sl.Add(new settinglist("HELP", "check out this guide", "h"));
            sl.Add(new settinglist("ABOUT", "About Us!", "i"));
            SettingsList.ItemsSource = sl;
            #endregion
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            
        }

        private async void SettingsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SettingsList.SelectedIndex == 0)
            {
                if (!this.Frame.Navigate(typeof(GeneralSettings)))
                {
                    throw new Exception("Failed to create scenario list");
                }
            }
            if (SettingsList.SelectedIndex == 1)
            {
                if (!this.Frame.Navigate(typeof(PersonalizationPage)))
                {
                    throw new Exception("Failed to create scenario list");
                }
            }
            if (SettingsList.SelectedIndex == 2)
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
            }
            if (SettingsList.SelectedIndex == 3)
            {
                if (!this.Frame.Navigate(typeof(PersonalizationPage)))
                {
                    throw new Exception("Failed to create scenario list");
                }
            }
            if (SettingsList.SelectedIndex == 4)
            {
                if (!this.Frame.Navigate(typeof(PersonalizationPage)))
                {
                    throw new Exception("Failed to create scenario list");
                }
            }
            if (SettingsList.SelectedIndex == 5)
            {
                if (!this.Frame.Navigate(typeof(PersonalizationPage)))
                {
                    throw new Exception("Failed to create scenario list");
                }
            }
            if (SettingsList.SelectedIndex == 6)
            {
                if (!this.Frame.Navigate(typeof(AboutPage)))
                {
                    throw new Exception("Failed to create scenario list");
                }
            }
        }
    }
}
