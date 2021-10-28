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
using Microsoft.Xna.Framework;
using System.ComponentModel;
using File360.Resources;
using System.IO.IsolatedStorage;

namespace File360
{
    public partial class vault : PhoneApplicationPage
    {
        Accelerometer accelerometer;
        const string MAIN_PAGE_URI = "/MainPage.xaml";
        List<vaultlist> vl = new List<vaultlist>();
        public static string Vbtns;
        IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;

        int index;
        public vault()
        {
            InitializeComponent();
            #region AccelerometerChecker

            if ((string)appSettings["Shaker"] == "On")
            {
                if (accelerometer == null)
                {
                    accelerometer = new Accelerometer();
                    accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                    accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);
                    try
                    {
                        accelerometer.Start();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show("Initiating Accelerometer Failed!" + "\nAdditional Information:\n" + ex);
                    }
                }
            }

#endregion
            #region FilesLister
            if (MainPage.Hbtns != null)
            {
                vl.Add(new vaultlist(MainPage.Hbtns, "Folder"));
                List<AlphaKeyGroup<vaultlist>> DataSource = AlphaKeyGroup<vaultlist>.CreateGroups(vl, System.Threading.Thread.CurrentThread.CurrentUICulture, (vaultlist v) => { return v.VaultName; }, true);
                vaultLister.ItemsSource = DataSource;
            }
            #endregion
        }

        #region Accelerometer

        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            Dispatcher.BeginInvoke(() => UpdateUI(e.SensorReading));
        }

        private void UpdateUI(AccelerometerReading accelerometerReading)
        {

            Vector3 acceleration = accelerometerReading.Acceleration;

            if (acceleration.X >= 0.8 || acceleration.X <= -0.8)
            {
                if (accelerometer != null)
                {
                    accelerometer.Stop();
                }
                NavigationService.GoBack();
            }

        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (accelerometer != null)
            {
                accelerometer.Stop();
            }
        }

#endregion
        #region ContextMenu

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            
            vl.RemoveAt(index);
            List<AlphaKeyGroup<vaultlist>> DataSource = AlphaKeyGroup<vaultlist>.CreateGroups(vl, System.Threading.Thread.CurrentThread.CurrentUICulture, (vaultlist v) => { return v.VaultName; }, true);
            vaultLister.ItemsSource = DataSource;
        }

        #endregion

        private void folderTap_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Button b = (Button)sender;
            Vbtns = (b.Content).ToString();
            vaultlist dataObject = (vaultlist)b.DataContext;
            index = vl.IndexOf(dataObject);
        }

    }
}