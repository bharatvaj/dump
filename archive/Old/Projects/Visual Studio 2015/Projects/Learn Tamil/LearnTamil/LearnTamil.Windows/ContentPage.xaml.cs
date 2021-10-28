using LearnTamil.DataModel;
using MyToolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LearnTamil
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class ContentPage : Page
    {
        App app = (App)Application.Current;
        MediaElement mediaPlayer = new MediaElement();
        List<DataModel.Title> title = new List<DataModel.Title>();
        List<DataModel.GridTable> DataGrid = new List<DataModel.GridTable>();
        int Chapter = 0;
        int Title = 0;
        int Subtitle = 0;
        int AudioNo = 0;
        string CurrentContext
        {
            get { return Chapter.ToString()+"."+Title.ToString() +"." + Subtitle.ToString(); }
        }
        public ContentPage()
        {
            this.InitializeComponent();

            header.Text = "LESSON " + app.ChapterNo;
            headerText.Text = app.ChapterName;
            DrawerLayout.InitializeDrawerLayout();
            ReadXml();
        }

        private async void ReadXml()
        {
            StorageFile storageFile = null;
            StorageFolder storageFolder = Package.Current.InstalledLocation;
            storageFile = await storageFolder.GetFileAsync("lesson1.xml");
            string Name = storageFile.DisplayName;
            for (int i = 0; i < +25; i++)
            {
                if (Name.Contains(i.ToString()))
                {
                    Chapter = i;
                    break;
                }
            }
            string uri = await FileIO.ReadTextAsync(storageFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            XmlDocument xmlDoc = await XmlDocument.LoadFromFileAsync(storageFile);
            XDocument xDoc = XDocument.Parse(xmlDoc.GetXml());
            XmlReader reader = xDoc.CreateReader();
            string elementName = String.Empty;
            string titleText = String.Empty;
            string activityText = String.Empty;
            StackPanel stackPanel = null;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        elementName = reader.Name;
                        if (elementName == "section")
                        {
                            stackPanel = new StackPanel();
                            titleText = String.Empty;
                            activityText = String.Empty;
                            if (reader.HasAttributes)
                            {
                                try
                                {
                                    titleText = reader.GetAttribute("title");
                                }
                                catch { }
                                try
                                {
                                    activityText = reader.GetAttribute("activity");
                                }
                                catch { }
                                if (reader.GetAttribute("title") != null)
                                {
                                    Subtitle += 1;
                                    AudioNo = 0;
                                    ImageBrush im = new ImageBrush();
                                    BitmapImage bm = new BitmapImage();
                                    bm.UriSource = new Uri("ms-appx:///Bharat.png");
                                    im.ImageSource = bm;
                                    im.Stretch = Stretch.Uniform;
                                    TextBlock txt = new TextBlock();
                                    txt.FontSize = 25;
                                    txt.TextWrapping = TextWrapping.Wrap;
                                    txt.Foreground = new SolidColorBrush(Colors.White);
                                    txt.Text = titleText.ToUpper();
                                    txt.VerticalAlignment = VerticalAlignment.Center;
                                    txt.HorizontalAlignment = HorizontalAlignment.Left;
                                    txt.Margin = new Thickness(5);
                                    stackPanel.Children.Add(txt);


                                    StackPanel stkPanel = new StackPanel();
                                    stkPanel.Orientation = Orientation.Horizontal;
                                    TextBlock txtContext = new TextBlock();
                                    txtContext.Text = Subtitle.ToString();

                                    txtContext.Foreground = new SolidColorBrush(Colors.Black);
                                    TextBlock txtNew = new TextBlock();
                                    txtNew.Text = " " + titleText;
                                    txtNew.Foreground = new SolidColorBrush(Colors.Black);
                                    txtNew.VerticalAlignment = VerticalAlignment.Center;
                                    txtNew.HorizontalAlignment = HorizontalAlignment.Center;
                                    txtNew.TextLineBounds = TextLineBounds.Tight;
                                    Ellipse el = new Ellipse();
                                    el.Height = 30;
                                    el.Width = 30;
                                    el.Stroke = new SolidColorBrush(Color.FromArgb(255, 243, 156, 18));

                                    Grid grdContext = new Grid();
                                    grdContext.Margin = new Thickness(10, 0, 0, 0);
                                    txtContext.TextLineBounds = TextLineBounds.Tight;
                                    txtContext.VerticalAlignment = VerticalAlignment.Center;
                                    txtContext.HorizontalAlignment = HorizontalAlignment.Center;
                                    grdContext.Children.Add(txtContext);
                                    grdContext.Children.Add(el);

                                    stkPanel.Children.Add(grdContext);
                                    stkPanel.Children.Add(txtNew);
                                    listBox.Children.Add(stkPanel);
                                }
                                else if (reader.GetAttribute("header") != null)
                                {
                                    Subtitle = 0;
                                    AudioNo = 0;
                                    Title += 1;

                                    TextBlock txtHeader = new TextBlock();
                                    txtHeader.Text = "LESSON 1";
                                    txtHeader.FontSize = 25;
                                    //txtHeader.FontWeight = FontWeights.Bold;
                                    txtHeader.Foreground = new SolidColorBrush(Colors.White);

                                    TextBlock txt = new TextBlock();
                                    //txt.FontWeight = FontWeights.Bold;
                                    txt.FontSize = 30;
                                    txt.TextWrapping = TextWrapping.Wrap;
                                    txt.Foreground = new SolidColorBrush(Colors.White);
                                    txt.Text = reader.GetAttribute("header");

                                    TextBlock txtSummary = new TextBlock();
                                    txtSummary.Text = "\n" + reader.Value;
                                    txtSummary.FontSize = 20;
                                    txtSummary.TextWrapping = TextWrapping.Wrap;
                                    txtSummary.Foreground = new SolidColorBrush(Colors.White);
                                    StackPanel tempPanel = new StackPanel();

                                    tempPanel.Children.Add(txtHeader);
                                    tempPanel.Children.Add(txt);
                                    tempPanel.Children.Add(txtSummary);
                                    tempPanel.Margin = new Thickness(15, 50, 15, 50);
                                    Grid grd = new Grid();
                                    grd.Background = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
                                    stackPanel.Children.Add(tempPanel);
                                    //ImageBrush im = new ImageBrush();
                                    //im.ImageSource = app.ChapterImage.ImageSource;
                                    //im.Opacity = 0.5;
                                    //im.Stretch = Stretch.UniformToFill;
                                    //stackPanel.Background = im;

                                    StackPanel stkPanel = new StackPanel();
                                    stkPanel.Orientation = Orientation.Horizontal;
                                    TextBlock txtContext = new TextBlock();
                                    txtContext.Text = Title.ToString();

                                    txtContext.Foreground = new SolidColorBrush(Colors.Black);
                                    TextBlock txtNew = new TextBlock();
                                    txtNew.Foreground = new SolidColorBrush(Colors.Black);
                                    txtNew.Text = reader.GetAttribute("header");
                                    Ellipse el = new Ellipse();
                                    el.Height = 30;
                                    el.Width = 30;
                                    el.Stroke = new SolidColorBrush(Color.FromArgb(255, 243, 156, 18));

                                    Grid grdContext = new Grid();
                                    grdContext.Children.Add(txtContext);
                                    txtContext.TextLineBounds = TextLineBounds.Tight;
                                    txtContext.VerticalAlignment = VerticalAlignment.Center;
                                    txtContext.HorizontalAlignment = HorizontalAlignment.Center;

                                    txtNew.VerticalAlignment = VerticalAlignment.Center;
                                    txtNew.HorizontalAlignment = HorizontalAlignment.Center;
                                    txtNew.TextLineBounds = TextLineBounds.Tight;

                                    grdContext.Children.Add(el);
                                    stkPanel.Children.Add(grdContext);
                                    stkPanel.Children.Add(txtNew);
                                    listBox.Children.Add(stkPanel);
                                }
                                else if (reader.GetAttribute("activity") != null)
                                {
                                    if (reader.GetAttribute("activity") == "match")
                                    {
                                        Subtitle += 1;
                                        AudioNo = 0;
                                        int count = 0;
                                        var subNodesXmlReader = reader.ReadSubtree();

                                        StackPanel stkPanel = new StackPanel();
                                        stkPanel.Margin = new Thickness(25, 15, 25, 15);
                                        TextBlock txtA = new TextBlock();
                                        txtA.Text = "ACTIVITY";
                                        txtA.FontWeight = FontWeights.Bold;
                                        stkPanel.Children.Add(txtA);
                                        txtA.FontSize = 20;
                                        TextBlock txt = new TextBlock();
                                        txt.Text = "Match the words with similar meaning";
                                        txt.FontSize = 24;
                                        stkPanel.Children.Add(txt);

                                        WrapPanel wg = new WrapPanel();
                                        wg.VerticalAlignment = VerticalAlignment.Center;
                                        wg.Margin = new Thickness(0, 10, 0, 0);
                                        //for (int i = 0; i < 2; i++)
                                        //{
                                        //    //ColumnDefinition cd = new ColumnDefinition();
                                        //    //cd.Width = new GridLength(1, GridUnitType.Star);
                                        //    //wg.ColumnDefinitions.Add(cd);
                                        //}
                                        string subElementName = String.Empty;
                                        while (subNodesXmlReader.Read())
                                        {
                                            switch (subNodesXmlReader.NodeType)
                                            {
                                                case XmlNodeType.Element:
                                                    subElementName = subNodesXmlReader.Name;
                                                    break;
                                                case XmlNodeType.Text:
                                                    if (subElementName == "t")
                                                    {
                                                        //RowDefinition rd = new RowDefinition();
                                                        //rd.Height = new GridLength(1, GridUnitType.Star);
                                                        //wg.RowDefinitions.Add(rd);
                                                        ToggleButton mat = new ToggleButton();
                                                        mat.Foreground = new SolidColorBrush(Colors.White);
                                                        mat.Content = subNodesXmlReader.Value;
                                                        mat.FontSize = 30;
                                                        mat.HorizontalAlignment = HorizontalAlignment.Center;
                                                        mat.VerticalAlignment = VerticalAlignment.Center;

                                                        wg.Children.Add(mat);
                                                    }
                                                    else
                                                    {
                                                        ToggleButton mat = new ToggleButton();
                                                        mat.Foreground = new SolidColorBrush(Colors.White);
                                                        mat.Content = subNodesXmlReader.Value;
                                                        mat.FontSize = 30;
                                                        mat.HorizontalAlignment = HorizontalAlignment.Center;
                                                        mat.VerticalAlignment = VerticalAlignment.Center;
                                                        Rectangle rect = new Rectangle();
                                                        ImageBrush im = new ImageBrush();
                                                        BitmapImage bm = new BitmapImage();
                                                        bm.UriSource = new Uri("ms-appx:///dCtaCBG.png", UriKind.Absolute);
                                                        im.ImageSource = bm;
                                                        rect.Fill = im;
                                                        rect.Height = 150;
                                                        rect.Width = 150;
                                                        //wg.Children.Add(mat);
                                                        wg.Children.Add(rect);
                                                        count += 1;
                                                    }
                                                    break;
                                            }
                                        }
                                        stkPanel.Children.Add(wg);
                                        stackPanel.Children.Add(stkPanel);
                                    }
                                }
                                else if (reader.GetAttribute("dialog") != null)
                                {
                                    Subtitle += 1;
                                    AudioNo = 0;
                                    DialogContainer dc = new DialogContainer();
                                    var subNodesXmlReader = reader.ReadSubtree();
                                    string subElementName = String.Empty;
                                    while (subNodesXmlReader.Read())
                                    {
                                        switch (subNodesXmlReader.NodeType)
                                        {
                                            case XmlNodeType.Element:
                                                subElementName = subNodesXmlReader.Name;

                                                if (subElementName == "grammar")
                                                {
                                                    dc.Gparts = subNodesXmlReader;
                                                }
                                                else if (subElementName == "pronounciationtips")
                                                {
                                                    dc.Pparts = subNodesXmlReader;
                                                }
                                                else if (subElementName == "st")
                                                {

                                                }
                                                else
                                                {

                                                }
                                                break;
                                            case XmlNodeType.Text:
                                                if (subElementName == "e")
                                                {
                                                    dc.Eparts = subNodesXmlReader.Value;
                                                }
                                                else if (subElementName == "st")
                                                {
                                                    dc.STparts = subNodesXmlReader.Value;
                                                }

                                                else
                                                {

                                                }
                                                break;
                                        }
                                    }

                                    //dc.ContentUI = stkPanel;
                                    stackPanel.Children.Add(dc);
                                }
                            }
                            else
                            {
                                title.Add(new Title(stackPanel));
                            }
                            title.Add(new DataModel.Title(stackPanel));
                        }
                        else if (elementName == "rec")
                        {
                            AudioNo += 1;
                            try
                            {
                                AudioClipPlayer bt = new AudioClipPlayer();
                                bt.Margin = new Thickness(10);
                                bt.ButtonClick += PlayAudio;
                                bt.ButtonUncheck += StopAudio;
                                bt.Tag = "ms-appx:///rec/rec" + Chapter.ToString() + "." + Title.ToString() + "." + Subtitle.ToString() + "." + AudioNo.ToString() + ".mp3";
                                stackPanel.Children.Add(bt);
                            }
                            catch { }
                        }
                        break;

                    case XmlNodeType.Text: //Display the text in each element.
                        //try
                        //{
                            stackPanel.Children.Add(ReadAndReturnUIStack(reader, elementName));
                        //}
                        //catch { }

                        break;
                }
            }
            foreach (Title sectionContext in title)
            {
                Border hsec = new Border();
                if (title.IndexOf(sectionContext) == 0)
                {
                    //hsec.Background = new SolidColorBrush(Color.FromArgb(255, 44, 62, 80));
                }
                else if (title.IndexOf(sectionContext) == 1)
                {
                    //hsec.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                }
                else if (title.IndexOf(sectionContext) == title.Count-1)
                {
                    hsec.Background = new SolidColorBrush(Color.FromArgb(255, 44, 62, 80));
                }
                else
                {
                    //BitmapImage bm = new BitmapImage();
                    //bm.UriSource = new Uri("ms-appx:///mainDropShadow.png");
                    //ImageBrush im = new ImageBrush();
                    //im.ImageSource = bm;
                    //im.Stretch = Stretch.Fill;
                    //hsec.Background = new SolidColorBrush(Color.FromArgb(255, 52, 73, 94));
                    //sectionContext.ChildStack.Margin = new Thickness(40);
                }
                hsec.DataContext = sectionContext;
                hsec.Width = 550;
                //hsec.Foreground = new SolidColorBrush(Colors.Black);
                sectionContext.ChildStack.Margin = new Thickness(40);
                hsec.Child = sectionContext.ChildStack;// (DataTemplate)Resources["SectionTemplate"];
                hub.Children.Add(hsec);
            }
            //XElement root = xDoc.Root;
            ////XNamespace ad = "http://www.w3.org/2005/Atom";
            //IEnumerable<string> title = from abc in root.Descendants("title") select (string)abc;
            //foreach (var str in title)
            //    Status(str);
        }

        private UIElement ReadAndReturnUIStack(XmlReader reader, string elementName)
        {
            if (elementName == "dialog")
            {
                TextBlock txt = new TextBlock();
                txt.FontSize += 3;
                txt.Style = (Style)App.Current.Resources["BodyTextBlockStyle"];
                txt.Text = reader.Value;
                return txt;
            }
            else if (elementName == "summary")
            {
                TextBlock txt = new TextBlock();
                txt.FontSize += 5;
                txt.Style = (Style)App.Current.Resources["BodyTextBlockStyle"];
                txt.Foreground = new SolidColorBrush(Colors.White);
                txt.Text = reader.Value;
                return txt;
            }
            else if (elementName == "culturalnote")
            {
                StackPanel stkPanel = new StackPanel();
                stkPanel.Background = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
                stkPanel.Margin = new Thickness(0, 10, 0, 0);
                TextBlock txtI = new TextBlock();
                txtI.Margin = new Thickness(10);
                txtI.Foreground = new SolidColorBrush(Colors.White);
                txtI.FontSize += 30;
                txtI.FontFamily = new FontFamily("languageapp.ttf#languageapp");
                txtI.Text = "c";
                TextBlock txt = new TextBlock();
                txt.FontSize += 3;
                txt.Style = (Style)App.Current.Resources["BodyTextBlockStyle"];
                txt.Foreground = new SolidColorBrush(Colors.White);
                txt.FontWeight = FontWeights.SemiBold;
                txt.Text = Environment.NewLine + "CULTURAL NOTE:" + reader.Value;
                txt.FontSize += 1;
                txt.Margin = new Thickness(10);
                txt.TextWrapping = TextWrapping.Wrap;
                stkPanel.Children.Add(txtI);
                stkPanel.Children.Add(txt);
                return stkPanel;
            }
            else if (elementName == "table")
            {
                Subtitle += 1;
                AudioNo = 0;
                Grid grd = new Grid();
                for (int i = 0; i < 3; i++)
                {
                    ColumnDefinition cd = new ColumnDefinition();
                    cd.Width = new GridLength(1, GridUnitType.Star);
                    grd.ColumnDefinitions.Add(cd);
                }
                string[] parts = reader.Value.Split('|');
                foreach (string part in parts)
                {
                    part.Trim();
                }
                int count = 0;
                int rowCount = 0;
                for (int i = 0; i < parts.Count() / 3; i++)
                {
                    RowDefinition rd = new RowDefinition();
                    rd.Height = new GridLength(1, GridUnitType.Star);
                    grd.RowDefinitions.Add(rd);
                }
                for (int i = 0; i < parts.Count(); i++)
                {

                    string part = parts[i].Trim();
                    TextBlock txt = new TextBlock();
                    txt.Foreground = new SolidColorBrush(Colors.Black);
                    txt.Text = part;
                    txt.Margin = new Thickness(10);
                    txt.Foreground = new SolidColorBrush(Colors.White);
                    txt.FontSize = 18;
                    txt.HorizontalAlignment = HorizontalAlignment.Center;
                    txt.VerticalAlignment = VerticalAlignment.Center;

                    Grid.SetRow(txt, rowCount);
                    Grid.SetColumn(txt, count);

                    grd.Children.Add(txt);
                    count += 1;
                    if (count > 2)
                    {
                        rowCount += 1;
                        count = 0;
                    }
                }
                return grd;
            }
            else return new Grid();
        }

        private async void StopAudio(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => mediaPlayer.Stop());
        }

        private async void PlayAudio(object sender, RoutedEventArgs e)
        {
            ToggleButton bt = (ToggleButton)sender;
            Grid grd = (Grid)bt.Parent;
            AudioClipPlayer acp = (AudioClipPlayer)grd.Parent;
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(acp.Tag.ToString()));
            //mediaPlayer.AutoPlay = false;
            mediaPlayer.SetSource(await (file.OpenAsync(FileAccessMode.Read)), "mp3");
            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () => { mediaPlayer.Play(); });
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void IncrementFontHeight(object sender, RoutedEventArgs e)
        { 
            var children = stackView.Children.OfType<TextBlock>();
            foreach (var child in children)
            {
                child.FontSize += 1;
            }
        }

        private void DecrementFontHeight(object sender, RoutedEventArgs e)
        {
            var children = stackView.Children.OfType<TextBlock>();
            foreach (var child in children)
            {
                child.FontSize -= 1;
            }
        }

        private void InvertColor(object sender, RoutedEventArgs e)
        {
            header.Foreground = new SolidColorBrush(Colors.Black);
            Background = new SolidColorBrush(Colors.White);
            var children = stackView.Children.OfType<TextBlock>();
            foreach (var child in children)
            {
                child.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        //private async void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    StackPanel item = (StackPanel)listBox.SelectedItem;
        //    //TextBlock txt = (TextBlock)item.Children.First();
        //    StackPanel stkPanel = (StackPanel)scrollViewer.Content;
        //    var children = stkPanel.Children.OfType<TextBlock>();
        //    foreach (var child in children)
        //    {
        //        //if (child.Text.Contains(txt.Text))
        //        //{
        //        //    int stkPan = stkPanel.Children.IndexOf(item);
        //        //    scrollViewer.ChangeView(0, stkPan, 1);
        //        //}
        //    }
        //}

        private void ActivityClose(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ActivityGrid.Visibility = Visibility.Collapsed;
        }
    }
}
