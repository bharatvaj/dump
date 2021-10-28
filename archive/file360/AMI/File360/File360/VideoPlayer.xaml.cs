using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace File360
{
    public partial class VideoPlayer : PhoneApplicationPage
    {
        double volume;
        public VideoPlayer()
        {
            InitializeComponent();
        }


        private void Reverse_Click(object sender, RoutedEventArgs e)
        {
            if (MediaPlayer.CanSeek)
            {
                int SliderValue = (int)timelineSlider.Value;
                timelineSlider.Value = SliderValue - 0.3;
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (MediaPlayer.CanPause)
            {
                MediaPlayer.Pause();
            }
            else
            {
                MediaPlayer.Play();
            }
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (MediaPlayer.CanSeek)
            {
                int SliderValue = (int)timelineSlider.Value;
                timelineSlider.Value = SliderValue + 0.3;
            }
        }

        void Media_MediaFailed(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void MediaPlayer_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ControlsContainer.Visibility == System.Windows.Visibility.Visible)
            {
                ControlsContainer.Visibility = System.Windows.Visibility.Collapsed;
            }
            else 
            {
                ControlsContainer.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void timelineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int SliderValue = (int)timelineSlider.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            MediaPlayer.Position = ts;
        }

        private void VideoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            timelineSlider.Maximum = MediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Stop();
        }
        
        private void MuteAudio_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (MediaPlayer.Volume == 0)
            {
                MediaPlayer.Volume = volume;
            }
            
            if(MediaPlayer.Volume != 0)
            {
                volume = MediaPlayer.Volume;
                MediaPlayer.Volume = 0;
            }
        }

    }
}