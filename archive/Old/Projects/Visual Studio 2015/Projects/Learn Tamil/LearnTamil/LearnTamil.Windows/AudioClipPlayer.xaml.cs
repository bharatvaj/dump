using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LearnTamil
{
    public sealed partial class AudioClipPlayer : UserControl
    {
        App app = (App)Application.Current;
        public AudioClipPlayer()
        {
            this.InitializeComponent();
        }

        private void Open_Music(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "s";
            OpenMusic.Begin();
        }
        public RoutedEventHandler ButtonClick
        {
            get { return null; }
            set { button.Checked += value; }
        }
        public RoutedEventHandler ButtonUncheck
        {
            get { return null; }
            set { button.Unchecked += value; }
        }

        public double MaximumValue
        {
            get { return progBar.Maximum; }
            set { progBar.Maximum += value; }
        }
        public double CurrentValue
        {
            get { return progBar.Value; }
            set { progBar.Value += value; }
        }
        private void Close_Music(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "p";
            OpenMusicRev.Begin();
        }
    }
}
