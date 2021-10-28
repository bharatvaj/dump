using LearnTamil.DataModel;
using MyToolkit.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Hub Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=321224

namespace LearnTamil
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>

    public sealed partial class HubPage : Page
    {
        private readonly PropertyPath _colorPath = new PropertyPath("(Grid.Background).(SolidColorBrush.Color)");
        List<Color> colors = new List<Color>();
        App app = (App)Application.Current;
        List<Title> title = new List<Title>();
        
        public PropertyPath ColorPath
        {
            get
            {
                return _colorPath;
            }
        }

        public HubPage()
        {
            this.InitializeComponent();
            Loaded += HubPage_Loaded;
            colors.Add(Color.FromArgb(255, 51, 51, 51));
            colors.Add(Color.FromArgb(255, 34, 167, 240));
            colors.Add(Color.FromArgb(255, 244, 208, 63));
            colors.Add(Color.FromArgb(255, 151, 206, 104));
            colors.Add(Color.FromArgb(255, 51, 51, 51));
            //chapterImage.Background = app.ChapterImage;
            //flipView.DataContext = title;
            //chapterName.Text = app.ChapterName;
        }


        private void HubPage_Loaded(object sender, RoutedEventArgs e)
        {
            ReadXml();
            flipView.SelectionChanged += FlipView_SelectionChanged;
            flipView.IsEnabled = true;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        
        #region MayDelete
        private async void ReadXml()
        {
            StorageFile storageFile = null;
            StorageFolder storageFolder = Package.Current.InstalledLocation;
            storageFile = await (await storageFolder.GetFolderAsync("lesson_contents")).GetFileAsync("lesson0.xml");
            string uri = await FileIO.ReadTextAsync(storageFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            XmlDocument xmlDoc = await XmlDocument.LoadFromFileAsync(storageFile);
            XDocument xDoc = XDocument.Parse(xmlDoc.GetXml());
            XmlReader reader = xDoc.CreateReader();
            string elementName = String.Empty;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        elementName = reader.Name;
                        break;

                    case XmlNodeType.Text: //Display the text in each element.

                        if (elementName == "name")
                        {
                            title.Add(new Title(reader.Value.Trim(), String.Empty));
                        }
                        else if (elementName == "subdetail")
                        {
                            title.Last().SubtitleText = reader.Value.Trim();
                        }
                        break;
                }
            }

            foreach (var item in title)
            {
                FlipViewItem flipViewItem = new FlipViewItem();

                Button bt = new Button();
                bt.Style = (Style)App.Current.Resources["DefaultButtonP"];
                bt.VerticalAlignment = VerticalAlignment.Center;
                bt.HorizontalAlignment = HorizontalAlignment.Center;
                bt.BorderBrush = null;
                bt.Click += Bt_Click;

                StackPanel stkPanel = new StackPanel();

                Border brd = new Border();
                brd.CornerRadius = new CornerRadius(500);
                brd.Background = new SolidColorBrush(Colors.White);
                brd.VerticalAlignment = VerticalAlignment.Center;
                brd.HorizontalAlignment = HorizontalAlignment.Center;

                Image im = new Image();
                im.Source = new BitmapImage(new Uri("ms-appx:///logo/0/0.png"));
                im.Stretch = Stretch.Uniform;
                im.Margin = new Thickness(25);
                im.Height = 100;
                im.Width = 100;

                brd.Child = im;

                TextBlock txt = new TextBlock();
                txt.Text = item.SubtitleName;
                txt.Style = (Style)App.Current.Resources["MessageDialogTitleStyle"];
                txt.Margin = new Thickness(20);
                txt.FontWeight = FontWeights.Bold;
                txt.Foreground = new SolidColorBrush(Color.FromArgb(192, 0, 0, 0));
                txt.HorizontalAlignment = HorizontalAlignment.Center;

                TextBlock txt1 = new TextBlock();
                txt1.Text = item.SubtitleText;
                txt1.Style = (Style)App.Current.Resources["MessageDialogContentStyle"];
                //txt1.Margin = new Thickness(10);
                txt1.FontWeight = FontWeights.Bold;
                txt1.Foreground = new SolidColorBrush(Color.FromArgb(192, 0, 0, 0));
                txt1.HorizontalAlignment = HorizontalAlignment.Center;

                stkPanel.Children.Add(brd);
                stkPanel.Children.Add(txt);
                stkPanel.Children.Add(txt1);

                bt.Content = stkPanel;

                flipViewItem.Content = bt;

                flipView.Items.Add(flipViewItem);
            }
        }

        private void Bt_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AlphabetPage));
        }
        #endregion

        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void GoToContent(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ContentPage));
        }

private void HardwareButtonsBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            if (this.Frame.CanGoBack)
            {
                this.Frame.Navigate(typeof(ContentPage));
            }
        }
        
    }
}
