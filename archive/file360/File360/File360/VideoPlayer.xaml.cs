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
    public partial class secret : PhoneApplicationPage
    {
        int i;
        int j;
        double volume;
        public secret()
        {
            InitializeComponent();
        }


        private void Reverse_Click(object sender, RoutedEventArgs e)
        {
            if (VideoPlayer.CanSeek)
            {
                int SliderValue = (int)timelineSlider.Value;
                timelineSlider.Value = SliderValue - 0.3;
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            j++;
            if (i % 2 != 1)
            {
                VideoPlayer.Play();
            }

            if (i % 2 == 1)
            {
                if (VideoPlayer.CanPause)
                {
                    VideoPlayer.Pause();
                }
            }
            
            
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (VideoPlayer.CanSeek)
            {
                int SliderValue = (int)timelineSlider.Value;
                timelineSlider.Value = SliderValue + 0.3;
            }
        }

        void Media_MediaFailed(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void VideoPlayer_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            i++;
            if(i%2 != 1)
            {
            ControlsContainer.Visibility = System.Windows.Visibility.Collapsed;
            MuteAudio.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (i % 2 == 1)
            {
                ControlsContainer.Visibility = System.Windows.Visibility.Visible;
                MuteAudio.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void timelineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int SliderValue = (int)timelineSlider.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            VideoPlayer.Position = ts;
        }

        private void VideoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            timelineSlider.Maximum = VideoPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            VideoPlayer.Stop();
        }
        
        private void MuteAudio_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (VideoPlayer.Volume == 0)
            {
                VideoPlayer.Volume = volume;
            }
            
            if(VideoPlayer.Volume != 0)
            {
                volume = VideoPlayer.Volume;
                VideoPlayer.Volume = 0;
            }
        }

    }
}