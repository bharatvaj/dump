using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LearnTamil
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ObservableCollection<DataModel.WordItems> wi = new ObservableCollection<DataModel.WordItems>();
        App app = (App)Application.Current;
        public MainPage()
        {
            this.InitializeComponent();
            gridView.ItemsSource = wi;
            this.NavigationCacheMode = NavigationCacheMode.Disabled;


            
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonsBackPressed;
            Loaded += MainPageLoaded;
        }

        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            ImageBrush im = new ImageBrush();
            im.Stretch = Stretch.UniformToFill;
            im.ImageSource = new BitmapImage(new Uri("ms-appx:///dCtaCBG.png"));
            wi.Add(new DataModel.WordItems("Chapter", "1", im, false));
            ImageBrush im1 = new ImageBrush();
            im1.Stretch = Stretch.UniformToFill;
            im.ImageSource = new BitmapImage(new Uri("ms-appx:///SpeakTamil4.jpg"));
            wi.Add(new DataModel.WordItems("Chapter", "2", im1, false));
            wi.Add(new DataModel.WordItems("Chapter", "3", null, true));
            wi.Add(new DataModel.WordItems("Chapter", "4", null, true));
            wi.Add(new DataModel.WordItems("Chapter", "5", null, true));
        }
        private void HardwareButtonsBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
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
        public void Status(string msg)
        {
            status.Text = msg;
        }
        private async void GridItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedItem = (DataModel.WordItems)e.ClickedItem;
            if (!selectedItem.Locked)
            {
                app.ChapterName = selectedItem.ChapterName;
                app.ChapterNo = selectedItem.ChapterNo;
                app.ChapterImage = selectedItem.ChapterImage;
                this.Frame.Navigate(typeof(HubPage));
            }
            else
            {
                try
                {
                    MessageDialog md = new MessageDialog("This content should be unlocked from store. Continue?");
                    await md.ShowAsync();
                }
                catch  { }
            }
        }
    }
}
