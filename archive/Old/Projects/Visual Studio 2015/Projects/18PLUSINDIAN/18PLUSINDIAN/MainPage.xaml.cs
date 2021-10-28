using _18PLUSINDIAN.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace _18PLUSINDIAN
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        ObservableCollection<Entity> entity = new ObservableCollection<Entity>();
        public MainPage()
        {
            this.InitializeComponent();
            Frame mainFrame = Window.Current.Content as Frame;
            mainFrame.ContentTransitions = null;
            ContentThemeTransition ctt = new ContentThemeTransition();
            ctt.VerticalOffset = 0;
            ctt.HorizontalOffset = 0;
            TransitionCollection tc = new TransitionCollection();
            tc.Add(ctt);
            mainFrame.ContentTransitions = tc;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            frequentGridView.ItemsSource = entity;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Loaded += MainPage_Loaded;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            if (newCardFrame.Visibility == Visibility.Visible)
            {
                newCardFrame.Visibility = Visibility.Collapsed;
                blockRect.Visibility = Visibility.Collapsed;
                addButton.IsEnabled = true;
                //newCardFrame.Content = null;
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            entity.Clear();
            entity.Add(new Entity("AADHAR CARD", "", Colors.IndianRed));
            entity.Add(new Entity("RATION CARD", "", Colors.Aquamarine));
            entity.Add(new Entity("DRIVING LICENSE", "", Colors.Teal));
            entity.Add(new Entity("ABOUT", "", (SolidColorBrush)App.Current.Resources["PhoneAccentBrush"]));
            entity.Add(new Entity("ALL","",(SolidColorBrush)App.Current.Resources["PhoneAccentBrush"]));
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void frequentGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Entity data_entity = (Entity)e.ClickedItem;
            string name = data_entity.Name;
            if (name == "ALL")
            {
                Frame.Navigate(typeof(ListPage));
            }
        }

        private void AddCard(object sender, RoutedEventArgs e)
        {
            newCardFrame.Visibility = Visibility.Visible;
            blockRect.Visibility = Visibility;
            newCardFrame.Navigate(typeof(NewCard));
            addButton.IsEnabled = false;
            
        }

        private void blockRect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (newCardFrame.Visibility == Visibility.Visible)
            {
                newCardFrame.Visibility = Visibility.Collapsed;
                blockRect.Visibility = Visibility.Collapsed;
                addButton.IsEnabled = true;
                //newCardFrame.Content = null;
            }
        }
    }
}
