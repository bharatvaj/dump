using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace File360
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        const string about = "/AboutPage.xaml";
        const string security = "/SecurityPage.xaml";
        const string personalization = "/PersonalizationPage.xaml";
        string txts;
        List<settinglist> sl = new List<settinglist>();
        public SettingsPage()
        {
            InitializeComponent();
            sl.Add(new settinglist("ftp", "configure file transfer prtocol","/Resources/Assets/Settings/ftp.png"));
            sl.Add(new settinglist("bluetooth", "bluetooth settings","/Resources/Assets/Images/add.png"));
            sl.Add(new settinglist("nfc", "nfc configuration",""));
            sl.Add(new settinglist("personalization", "get a look",""));
            sl.Add(new settinglist("music", "change your music settings",""));
            sl.Add(new settinglist("security", "protect your files",""));
            sl.Add(new settinglist("about", "About Us!",""));
            SettingsList.ItemsSource = sl;

        }

        async private void txtB_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock txt = (TextBlock)sender;
            txts = (txt.Text).ToString();
            if (txts == "about")
            {
                NavigationService.Navigate(new Uri(about, UriKind.Relative));
            }

            if (txts == "security")
            {
                NavigationService.Navigate(new Uri(security, UriKind.Relative));
            }

            if (txts == "bluetooth")
            {
               await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
            }

            if (txts == "nfc")
            {
                MessageBox.Show("coming soon...");
            }

            if (txts == "personalization")
            {
                NavigationService.Navigate(new Uri(personalization, UriKind.Relative));
            }

            if (txts == "ftp")
            {
                MessageBox.Show("coming soon...");
            }
            if (txts == "music")
            {
                MessageBox.Show("coming soon...");
            }
        }
		
		public void SettingTap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
			{
				
			}


    }
}