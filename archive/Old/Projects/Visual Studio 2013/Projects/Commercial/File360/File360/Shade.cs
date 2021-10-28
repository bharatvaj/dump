using System;
using System.Linq;
using System.Windows;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace File360
{
    public class Shade : Grid
    {
        #region Globals and events

        private readonly PropertyPath _translatePath = new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)");
        private readonly PropertyPath _colorPath = new PropertyPath("(Grid.Background).(SolidColorBrush.Color)");

        private readonly TranslateTransform _listFragmentTransform = new TranslateTransform();
        private readonly TranslateTransform _deltaTransform = new TranslateTransform();
        private const int MaxAlpha = 190;

        public delegate void DrawerEventHandler(object sender);
        public event DrawerEventHandler DrawerOpened;
        public event DrawerEventHandler DrawerClosed;

        private Storyboard _fadeInStoryboard;
        private Storyboard _fadeOutStoryboard;
        private Grid _shadowFragment;
        private Grid _slideFragment;

        #endregion

        #region Properties

        public bool Shadowed { get; set; }
        private PropertyPath TranslatePath
        {
            get { return _translatePath; }
        }
        private PropertyPath ColorPath
        {
            get { return _colorPath; }
        }

        #endregion

        #region Methods

        public Shade()
        {
            Shadowed = false;
        }

        public void Shadow(Grid Element)
        {
            _slideFragment = (Grid)Element;
            // Create a shadow element
            _shadowFragment = new Grid
            {
                Name = "_shadowFragment",
                Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Visibility = Visibility.Collapsed
            };
            _shadowFragment.Tapped += shadowFragment_Tapped;
            _shadowFragment.IsHitTestVisible = false;

            // Set ZIndexes
            Canvas.SetZIndex(_shadowFragment, 50);
            Canvas.SetZIndex(_slideFragment, 51);
            Children.Add(_shadowFragment);

            // Create a new fadeIn animation storyboard
            _fadeInStoryboard = new Storyboard();

            // New double animation
            var doubleAnimation1 = new DoubleAnimation 
            { 
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)), 
                To = 0 
            };

            Storyboard.SetTarget(doubleAnimation1, _slideFragment);
            Storyboard.SetTargetProperty(doubleAnimation1, TranslatePath.Path);
            _fadeInStoryboard.Children.Add(doubleAnimation1);

            // New color animation for _shadowFragment
            var colorAnimation1 = new ColorAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
                To = Color.FromArgb(190, 0, 0, 0)
            };

            Storyboard.SetTarget(colorAnimation1, _shadowFragment);
            Storyboard.SetTargetProperty(colorAnimation1, ColorPath.Path);
            _fadeInStoryboard.Children.Add(colorAnimation1);

            // Create a new fadeOut animation storyboard
            _fadeOutStoryboard = new Storyboard();

            // New double animation
            var doubleAnimation2 = new DoubleAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
                To = -_slideFragment.Width - 200
            };

            Storyboard.SetTarget(doubleAnimation2, _slideFragment);
            Storyboard.SetTargetProperty(doubleAnimation2, TranslatePath.Path);
            _fadeOutStoryboard.Children.Add(doubleAnimation2);

            // New color animation for _shadowFragment
            var colorAnimation2 = new ColorAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
                To = Color.FromArgb(0, 0, 0, 0)
            };

            Storyboard.SetTarget(colorAnimation2, _shadowFragment);
            Storyboard.SetTargetProperty(colorAnimation2, ColorPath.Path);
            _fadeOutStoryboard.Children.Add(colorAnimation2);

        }

        public void UnShadow()
        {
            var shadow = new Storyboard();

            var doubleAnimation = new DoubleAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
                To = -_slideFragment.Width
            };

            Storyboard.SetTarget(doubleAnimation, _slideFragment);
            Storyboard.SetTargetProperty(doubleAnimation, TranslatePath.Path);
            shadow.Children.Add(doubleAnimation);

            var colorAnimation = new ColorAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
                To = Color.FromArgb(0, 0, 0, 0)
            };

            Storyboard.SetTarget(colorAnimation, _shadowFragment);
            Storyboard.SetTargetProperty(colorAnimation, ColorPath.Path);
            shadow.Children.Add(colorAnimation);

            shadow.Completed += shadow_Completed;
            shadow.Begin();
        }
        private void shadowFragment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var shadow = new Storyboard();

            var doubleAnimation = new DoubleAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
                To = -_slideFragment.Width
            };

            Storyboard.SetTarget(doubleAnimation, _slideFragment);
            Storyboard.SetTargetProperty(doubleAnimation, TranslatePath.Path);
            shadow.Children.Add(doubleAnimation);

            var colorAnimation = new ColorAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
                To = Color.FromArgb(0, 0, 0, 0)
            };

            Storyboard.SetTarget(colorAnimation, _shadowFragment);
            Storyboard.SetTargetProperty(colorAnimation, ColorPath.Path);
            shadow.Children.Add(colorAnimation);

            shadow.Completed += shadow_Completed;
            shadow.Begin();
        }
        private void shadow_Completed(object sender, object e)
        {
            _shadowFragment.IsHitTestVisible = false;
            _shadowFragment.Visibility = Visibility.Collapsed;
            Shadowed = false;

            // raise close event
            if (DrawerClosed != null) DrawerClosed(this);
        }
        private void fadeOutStoryboard_Completed(object sender, object e)
        {
            _shadowFragment.Visibility = Visibility.Collapsed;
            Shadowed = false;
            if (DrawerClosed != null) DrawerClosed(this);
        }
        #region MoveShadow
        private void MoveShadowFragment(double left)
        {
            // Show shadow fragment
            _shadowFragment.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            _shadowFragment.Visibility = Visibility.Visible;

            // Set bg color based on current _slideFragment position.
            var maxLeft = _slideFragment.ActualWidth;
            var currentLeft = maxLeft - left;

            var temp = Convert.ToInt32((currentLeft / maxLeft) * MaxAlpha);

            // Limit temp variable to 190 to avoid OverflowException
            if (temp > MaxAlpha) temp = MaxAlpha;

            byte alphaColorIndex;
            try
            {
                alphaColorIndex = Convert.ToByte(MaxAlpha - temp);
            }
            catch
            {
                alphaColorIndex = 0;
            }

            _shadowFragment.Background = new SolidColorBrush(Color.FromArgb(alphaColorIndex, 0, 0, 0));
        }
        #endregion
        #endregion
    }
}
