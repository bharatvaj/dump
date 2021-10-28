using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LearnTamil
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlphabetPage : Page
    {
        string[,] tamilChars = new string[20, 20];
        string[] proNo = new string[20];
        int proNoi = 0;
        public AlphabetPage()
        {
            this.InitializeComponent();
            Loaded += AlphabetPage_Loaded;
            ReadXml();
            
        }

        private void AlphabetPage_Loaded(object sender, RoutedEventArgs e)
        {
            life.SelectionChanged += Life_SelectionChanged;
            truth.SelectionChanged += Truth_SelectionChanged;
            lifetruth.SelectionChanged += Lifetruth_SelectionChanged;
        }

        private void Life_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lifetruth.SelectedIndex = life.SelectedIndex + (12 * truth.SelectedIndex);
                lifeDetails.Text = proNo[life.SelectedIndex];
            }
            catch (Exception)
            {

            }
        }

        private void Truth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lifetruth.SelectedIndex = life.SelectedIndex + (12 * truth.SelectedIndex);
            }
            catch (Exception)
            {

            }
        }


        private void Lifetruth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int lifeNo = life.SelectedIndex;
                int truthNo = truth.SelectedIndex;
                truth.SelectedIndex = (lifetruth.SelectedIndex - lifeNo) / 12;
                life.SelectedIndex = lifetruth.SelectedIndex - (12 * truthNo);
            }
            catch (Exception)
            {
            }
        }



        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        #region XmlReader

        private async void ReadXml()
        {
            StorageFile storageFile = null;
            StorageFolder storageFolder = Package.Current.InstalledLocation;
            storageFile = await storageFolder.GetFileAsync("tamil_characters.xml");
            string uri = await FileIO.ReadTextAsync(storageFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            XmlDocument xmlDoc = await XmlDocument.LoadFromFileAsync(storageFile);
            XDocument xDoc = XDocument.Parse(xmlDoc.GetXml());
            XmlReader reader = xDoc.CreateReader();
            string elementName = String.Empty;
            int i = 0;
            int j = 0;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        elementName = reader.Name;
                        if (elementName == "row")
                        {

                        }
                        break;

                    case XmlNodeType.EndElement:
                        if (reader.Name == "row")
                        {
                            i++;
                            j = 0;
                        }
                        //if (i == 0 && j > 0) life.IsEnabled = true;
                        break;

                    case XmlNodeType.Text: //Display the text in each element.
                        if (elementName == "cell")
                        {
                            tamilChars[i, j] = reader.Value;
                            if (i == 0 && j == 0)
                            {

                            }
                            else if (i == 0 && j > 0)
                            {
                                AddLetter(tamilChars[i, j], life);
                            }
                            else if (i > 0 && j == 0)
                            {
                                AddLetter(tamilChars[i, j], truth);
                            }
                            else
                            {
                                AddLetter(tamilChars[i, j], lifetruth);
                            }

                            j++;
                        }
                        else if (elementName == "p")
                        {
                            proNo[proNoi] = reader.Value;
                            proNoi++;
                        }
                        break;
                }
            }
        }
        #endregion


        public void AddLetter(string character, FlipView to)
        {
            FlipViewItem flipViewItem = new FlipViewItem();
            TextBlock txt = new TextBlock();
            txt.Text = character;
            txt.Style = (Style)App.Current.Resources["HeaderTextBlockStyle"];
            txt.HorizontalAlignment = HorizontalAlignment.Center;
            txt.VerticalAlignment = VerticalAlignment.Center;
            flipViewItem.Content = txt;
            to.Items.Add(flipViewItem);
        }
    }
}
