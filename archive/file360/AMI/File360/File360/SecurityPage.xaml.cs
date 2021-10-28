using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices.Sensors;
using File360.Resources;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using System.Windows.Input;

namespace File360
{
    public partial class SecurityPage : PhoneApplicationPage
    {
        IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        public SecurityPage()
        {
            InitializeComponent();
            if (!Accelerometer.IsSupported)
            {
                shake.IsEnabled = false;
            }
            Settings_Checker();
        }

        #region ToggleSwitcher

        private void shake_Checked(object sender, RoutedEventArgs e)
        {
            appSettings["Shaker"] = "On";
            appSettings.Save();
        }

        private void shake_Unchecked(object sender, RoutedEventArgs e)
        {
            appSettings["Shaker"] = "Off";
            appSettings.Save();
        }

        private void passw_Checked(object sender, RoutedEventArgs e)
        {
            appSettings["Passer"] = "On";
        }

        private void passw_Unchecked(object sender, RoutedEventArgs e)
        {
            appSettings["Passer"] = "Off";
        }


        private void PSetter(object sender, TextChangedEventArgs e)
        {

            appSettings["PasswordValue"] = pb.Text;
        }

        #endregion
        #region SettingsUpdater

        public void Settings_Checker()
        {
                if ((string)appSettings["Shaker"] == "On")
                {
                    shake.IsChecked = true;
                }

                if ((string)appSettings["Shaker"] == "Off")
                {
                    shake.IsChecked = false;
                }

                if ((string)appSettings["Passer"] == "On")
                {
                    passw.IsChecked = true;
                }

                if ((string)appSettings["Passer"] == "Off")
                {
                    passw.IsChecked = false;
                }
        }
        #endregion
    }
}
