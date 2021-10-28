using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Expense_Manager__
{
    public class DrawerLayout : Grid
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
        private Grid _listFragment;
        private Grid _mainFragment;
        private Grid _touchFragment;
        private Grid _shadowFragment;

        public static double tempX;
        public static double OriginX;
        public static double delta;

        #endregion

        #region Properties

        public bool IsDrawerOpen { get; set; }
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

        public DrawerLayout()
        {
            IsDrawerOpen = false;
        }

        public void InitializeDrawerLayout()
        {
            if (Children == null) return;
            if (Children.Count < 2) return;

            try
            {
                _mainFragment = Children.OfType<Grid>().FirstOrDefault();
                _listFragment = Children.OfType<Grid>().ElementAt(1);
                _touchFragment = Children.OfType<Grid>().Last();
            }
            catch
            {
                return;
            }

            if (_mainFragment == null || _listFragment == null) return;

            _mainFragment.Name = "_mainFragment";
            _listFragment.Name = "_listFragment";
            _touchFragment.Name = "_touchFragment";
            // _mainFragment
            _mainFragment.HorizontalAlignment = HorizontalAlignment.Stretch;
            _mainFragment.VerticalAlignment = VerticalAlignment.Stretch;
            if (_mainFragment.Background == null) _mainFragment.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));

            // Render transform _listFragment
            _listFragment.HorizontalAlignment = HorizontalAlignment.Left;
            _listFragment.VerticalAlignment = VerticalAlignment.Stretch;
            _listFragment.Width = ActualWidth / 3;
            if (_listFragment.Background == null) _listFragment.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

            var animatedTranslateTransform = new TranslateTransform { X = -_listFragment.Width, Y = 0 };

            _listFragment.RenderTransform = animatedTranslateTransform;
            _listFragment.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);

            _listFragment.UpdateLayout();

            // Create a shadow element
            _shadowFragment = new Grid
            {
                Name = "_shadowFragment",
                Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Visibility = Visibility.Collapsed
            };
            _shadowFragment.MouseUp += shadowFragment_Tapped;
            _shadowFragment.IsHitTestVisible = false;

            // Set ZIndexes
            Canvas.SetZIndex(_shadowFragment, 50);
            Canvas.SetZIndex(_listFragment, 51);
            Children.Add(_shadowFragment);

            // Create a new fadeIn animation storyboard
            _fadeInStoryboard = new Storyboard();

            // New double animation
            var doubleAnimation1 = new DoubleAnimation { Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)), To = 0 };

            Storyboard.SetTarget(doubleAnimation1, _listFragment);
            Storyboard.SetTargetProperty(doubleAnimation1, TranslatePath);
            _fadeInStoryboard.Children.Add(doubleAnimation1);

            // New color animation for _shadowFragment
            var colorAnimation1 = new ColorAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 75)),
                To = Color.FromArgb(190, 0, 0, 0)
            };

            Storyboard.SetTarget(colorAnimation1, _shadowFragment);
            Storyboard.SetTargetProperty(colorAnimation1, ColorPath);
            _fadeInStoryboard.Children.Add(colorAnimation1);

            // Create a new fadeOut animation storyboard
            _fadeOutStoryboard = new Storyboard();

            // New double animation
            var doubleAnimation2 = new DoubleAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 75)),
                To = -_listFragment.Width
            };

            Storyboard.SetTarget(doubleAnimation2, _listFragment);
            Storyboard.SetTargetProperty(doubleAnimation2, TranslatePath);
            _fadeOutStoryboard.Children.Add(doubleAnimation2);

            // New color animation for _shadowFragment
            var colorAnimation2 = new ColorAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 75)),
                To = Color.FromArgb(0, 0, 0, 0)
            };

            Storyboard.SetTarget(colorAnimation2, _shadowFragment);
            Storyboard.SetTargetProperty(colorAnimation2, ColorPath);
            _fadeOutStoryboard.Children.Add(colorAnimation2);
            _listFragment.UpdateLayout();

        }
        public void OpenDrawer()
        {
            if (_fadeInStoryboard == null || _mainFragment == null || _listFragment == null) return;
            _shadowFragment.Visibility = Visibility.Visible;
            _shadowFragment.IsHitTestVisible = true;
            _fadeInStoryboard.Begin();
            IsDrawerOpen = true;

            if (DrawerOpened != null)
                DrawerOpened(this);
        }
        public void CloseDrawer()
        {
            if (_fadeOutStoryboard == null || _mainFragment == null || _listFragment == null) return;
            _fadeOutStoryboard.Begin();
            _fadeOutStoryboard.Completed += fadeOutStoryboard_Completed;
            IsDrawerOpen = false;

            if (DrawerClosed != null)
                DrawerClosed(this);
        }
        private void shadowFragment_Tapped(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CloseDrawer();
            //var shadow = new Storyboard();

            //var doubleAnimation = new DoubleAnimation
            //{
            //    Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
            //    To = -_listFragment.Width
            //};

            //Storyboard.SetTarget(doubleAnimation, _listFragment);
            //Storyboard.SetTargetProperty(doubleAnimation, TranslatePath);
            //shadow.Children.Add(doubleAnimation);

            //var colorAnimation = new ColorAnimation
            //{
            //    Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200)),
            //    To = Color.FromArgb(0, 0, 0, 0)
            //};

            //Storyboard.SetTarget(colorAnimation, _shadowFragment);
            //Storyboard.SetTargetProperty(colorAnimation, ColorPath);
            //shadow.Children.Add(colorAnimation);

            //shadow.Completed += shadow_Completed;
            //shadow.Begin();
        }
        private void shadow_Completed(object sender, object e)
        {
            _shadowFragment.IsHitTestVisible = false;
            _shadowFragment.Visibility = Visibility.Collapsed;
            //IsDrawerOpen = false;

            //// raise close event
            //if (DrawerClosed != null) DrawerClosed(this);
        }
        private void fadeOutStoryboard_Completed(object sender, object e)
        {
            _shadowFragment.Visibility = Visibility.Collapsed;
            IsDrawerOpen = false;
            if (DrawerClosed != null) DrawerClosed(this);
        }
        private void MoveListFragment(double left, Color color)
        {
            var s = new Storyboard();

            var doubleAnimation = new DoubleAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 75)),
                To = left
            };

            Storyboard.SetTarget(doubleAnimation, _listFragment);
            Storyboard.SetTargetProperty(doubleAnimation, TranslatePath);
            s.Children.Add(doubleAnimation);

            var colorAnimation = new ColorAnimation { Duration = new Duration(new TimeSpan(0, 0, 0, 0, 75)), To = color };

            Storyboard.SetTarget(colorAnimation, _shadowFragment);
            Storyboard.SetTargetProperty(colorAnimation, ColorPath);
            s.Children.Add(colorAnimation);

            s.Begin();
        }
        private void MoveShadowFragment(double left)
        {
            // Show shadow fragment
            _shadowFragment.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            _shadowFragment.Visibility = Visibility.Visible;

            // Set bg color based on current _listFragment position.
            var maxLeft = _listFragment.ActualWidth;
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
    }
}
