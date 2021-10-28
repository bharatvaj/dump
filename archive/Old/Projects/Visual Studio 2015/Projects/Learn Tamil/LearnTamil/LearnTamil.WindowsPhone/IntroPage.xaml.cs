using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LearnTamil
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class IntroPage : Page
    {
        private readonly PropertyPath _colorPath = new PropertyPath("(Grid.Background).(SolidColorBrush.Color)");
        List<Color> colors = new List<Color>();
        DispatcherTimer dt = new DispatcherTimer();
        private Windows.UI.Xaml.PropertyPath ColorPath
        {
            get
            {
                return _colorPath;
            }
        }

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public IntroPage()
        {
            this.InitializeComponent();
            dt.Interval = new TimeSpan(0, 0, 1);
            dt.Tick += (e, t) =>
            {
                flipView.IsEnabled = true;
            };
            dt.Start();
            colors.Add(Color.FromArgb(255, 51, 51, 51));
            colors.Add(Color.FromArgb(255, 34, 167, 240));
            colors.Add(Color.FromArgb(255, 244, 208, 63));
            colors.Add(Color.FromArgb(255, 151, 206, 104));
            colors.Add(Color.FromArgb(255, 51, 51, 51));
            flipView.SelectionChanged += FlipView_SelectionChanged;

            // Create a simple setting


        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Storyboard colorChange = new Storyboard();
            ColorAnimation colorAnimation = new ColorAnimation
            {

                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
                To = colors[flipView.SelectedIndex]
            };

            Storyboard.SetTarget(colorAnimation, grid);
            Storyboard.SetTargetProperty(colorAnimation, ColorPath.Path);
            colorChange.Children.Add(colorAnimation);
            colorChange.Begin();
        }

        private void LearnTamil(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(MainPage));
            localSettings.Values["AppHasStarted"] = "1";
        }
    }
}
