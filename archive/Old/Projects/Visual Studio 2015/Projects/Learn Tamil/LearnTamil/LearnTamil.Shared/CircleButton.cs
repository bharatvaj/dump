using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace LearnTamil
{
   public class CircleButton :Button
    {
        Color color = new Color();
        Ellipse el = new Ellipse();
        public CircleButton(ImageBrush im,Color c)
        {
            
            BorderBrush = null;
            color = c;
            Click += CircleButton_Click;
            PointerPressed += CircleButton_PointerPressed;
            
            el.Height = 75;
            el.Width = 75;
            el.Fill = new SolidColorBrush(c);
            Rectangle rect = new Rectangle();
            rect.Height = 50;
            rect.Width = 50;
            rect.Fill = im;
            rect.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            rect.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            el.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            el.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Grid grd = new Grid();
            grd.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            grd.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            grd.Children.Add(el);
            grd.Children.Add(rect);
            Content = grd;
        }

        private void CircleButton_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            color.A = 150;
            el.Fill = new SolidColorBrush(color);
            Background = null;
        }

        private void CircleButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
        }
    }
}
