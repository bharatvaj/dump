using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LearnTamil
{
    public sealed partial class DialogContainer : UserControl
    {

        StackPanel pronounciatonGrd;
        public DialogContainer()
        {
            this.InitializeComponent();
            Height = Window.Current.Bounds.Height * 0.75;
            SizeChanged += DialogContainer_SizeChanged;
        }

        private void DialogContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Height = Window.Current.Bounds.Height * 0.75;
        }

        public StackPanel ContentUI
        {
            get { return Content; }
            set { Content.Children.Add(value); }
        }

        public string Eparts
        {
            set
            {
                StackPanel stkPanel = new StackPanel();
                string[] eparts = new string[10];
                eparts = value.Split('\n');

                for (int i = 0; i < eparts.Count(); i++)
                {

                    if (String.IsNullOrEmpty(eparts[i]) || String.IsNullOrWhiteSpace(eparts[i])) continue;
                    if (i % 2 != 0)
                    {
                        TextBlock txt = new TextBlock();
                        txt.Foreground = new SolidColorBrush(Colors.Black);
                        txt.Text = eparts[i].Trim();
                        txt.FontSize = 18;
                        txt.Margin = new Thickness(5);
                        txt.HorizontalAlignment = HorizontalAlignment.Left;
                        txt.VerticalAlignment = VerticalAlignment.Center;
                        Border brd = new Border();
                        brd.HorizontalAlignment = HorizontalAlignment.Left;
                        brd.Margin = new Thickness(5);
                        brd.Background = new SolidColorBrush(Color.FromArgb(180, 255, 255, 255));
                        brd.CornerRadius = new CornerRadius(3);
                        brd.Child = txt;
                        stkPanel.Children.Add(brd);
                    }
                    else
                    {
                        TextBlock txt = new TextBlock();
                        txt.Foreground = new SolidColorBrush(Colors.Black);
                        txt.Text = eparts[i].Trim();
                        txt.FontSize = 18;
                        txt.Margin = new Thickness(5);
                        txt.HorizontalAlignment = HorizontalAlignment.Left;
                        txt.VerticalAlignment = VerticalAlignment.Center;
                        Border brd = new Border();
                        brd.HorizontalAlignment = HorizontalAlignment.Right;
                        brd.Margin = new Thickness(5);
                        brd.Background = new SolidColorBrush(Color.FromArgb(225, 255, 255, 255));
                        brd.CornerRadius = new CornerRadius(3);
                        brd.Child = txt;
                        stkPanel.Children.Add(brd);
                    }
                }
                Content.Children.Add(stkPanel);
            }
        }
        public string STparts
        {
            set
            {
                StackPanel stkPanel = new StackPanel();
                string[] eparts = new string[10];
                eparts = value.Split('\n');

                for (int i = 0; i < eparts.Count(); i++)
                {

                    if (String.IsNullOrEmpty(eparts[i]) || String.IsNullOrWhiteSpace(eparts[i])) continue;
                    if (i % 2 != 0)
                    {
                        TextBlock txt = new TextBlock();
                        txt.Foreground = new SolidColorBrush(Colors.Black);
                        txt.Text = eparts[i].Trim();
                        txt.FontSize = 18;
                        txt.Margin = new Thickness(5);
                        txt.HorizontalAlignment = HorizontalAlignment.Left;
                        txt.VerticalAlignment = VerticalAlignment.Center;
                        Border brd = new Border();
                        brd.HorizontalAlignment = HorizontalAlignment.Left;
                        brd.Margin = new Thickness(5);
                        brd.Background = new SolidColorBrush(Color.FromArgb(180, 255, 255, 255));
                        brd.CornerRadius = new CornerRadius(3);
                        brd.Child = txt;
                        stkPanel.Children.Add(brd);
                    }
                    else
                    {
                        TextBlock txt = new TextBlock();
                        txt.Foreground = new SolidColorBrush(Colors.Black);
                        txt.Text = eparts[i].Trim();
                        txt.FontSize = 18;
                        txt.Margin = new Thickness(5);
                        txt.HorizontalAlignment = HorizontalAlignment.Left;
                        txt.VerticalAlignment = VerticalAlignment.Center;
                        Border brd = new Border();
                        brd.HorizontalAlignment = HorizontalAlignment.Right;
                        brd.Margin = new Thickness(5);
                        brd.Background = new SolidColorBrush(Color.FromArgb(225, 255, 255, 255));
                        brd.CornerRadius = new CornerRadius(3);
                        brd.Child = txt;
                        stkPanel.Children.Add(brd);
                    }
                }
                Content.Children.Add(stkPanel);
            }
        }

        public string Cparts
        {
            set
            {
                StackPanel stkPanel = new StackPanel();
                string[] eparts = new string[10];
                eparts = value.Split('\n');

                for (int i = 0; i < eparts.Count(); i++)
                {

                    if (String.IsNullOrEmpty(eparts[i]) || String.IsNullOrWhiteSpace(eparts[i])) continue;
                    if (i % 2 != 0)
                    {
                        TextBlock txt = new TextBlock();
                        txt.Foreground = new SolidColorBrush(Colors.Black);
                        txt.Text = eparts[i].Trim();
                        txt.FontSize = 18;
                        txt.Margin = new Thickness(5);
                        txt.HorizontalAlignment = HorizontalAlignment.Left;
                        txt.VerticalAlignment = VerticalAlignment.Center;
                        Border brd = new Border();
                        brd.HorizontalAlignment = HorizontalAlignment.Left;
                        brd.Margin = new Thickness(5);
                        brd.Background = new SolidColorBrush(Color.FromArgb(180, 255, 255, 255));
                        brd.CornerRadius = new CornerRadius(3);
                        brd.Child = txt;
                        stkPanel.Children.Add(brd);
                    }
                    else
                    {
                        TextBlock txt = new TextBlock();
                        txt.Foreground = new SolidColorBrush(Colors.Black);
                        txt.Text = eparts[i].Trim();
                        txt.FontSize = 18;
                        txt.Margin = new Thickness(5);
                        txt.HorizontalAlignment = HorizontalAlignment.Left;
                        txt.VerticalAlignment = VerticalAlignment.Center;
                        Border brd = new Border();
                        brd.HorizontalAlignment = HorizontalAlignment.Right;
                        brd.Margin = new Thickness(5);
                        brd.Background = new SolidColorBrush(Color.FromArgb(225, 255, 255, 255));
                        brd.CornerRadius = new CornerRadius(3);
                        brd.Child = txt;
                        stkPanel.Children.Add(brd);
                    }
                }
                Content.Children.Add(stkPanel);
            }
        }
        public XmlReader Pparts
        {
            set
            {
                TextBlock txt = new TextBlock();
                txt.Margin = new Thickness(10);
                //txt.Text = value;
                txt.TextWrapping = TextWrapping.Wrap;
                txt.FontSize = 18;
                txt.Foreground = new SolidColorBrush(Colors.White);
                string elementName = String.Empty;
                XmlReader reader = value.ReadSubtree();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            elementName = reader.Name;
                            break;
                        case XmlNodeType.Text:
                            PronounciationContent.Children.Add(ReadAndReturnUIStack(reader, elementName));
                            break;
                    }
                }
                PronounciationContent.Children.Add(txt);
            }
        }


        public XmlReader Gparts
        {
            set
            {
                TextBlock txt = new TextBlock();
                txt.Margin = new Thickness(10);
                //txt.Text = value;
                txt.TextWrapping = TextWrapping.Wrap;
                txt.FontSize = 18;
                txt.Foreground = new SolidColorBrush(Colors.White);
                string elementName = String.Empty;
                XmlReader reader = value.ReadSubtree();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            elementName = reader.Name;
                            break;
                        case XmlNodeType.Text:
                            GrammarContent.Children.Add(ReadAndReturnUIStack(reader, elementName));
                            break;
                    }
                }
                GrammarContent.Children.Add(txt);
            }
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


        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            bt.Background = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255));
            bt.Foreground = new SolidColorBrush(Colors.Black);

            foreach (Button tempBt in ButtonContainer.Children)
            {
                if (tempBt == bt)
                {
                    continue;
                }
                tempBt.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                tempBt.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
            flipView.SelectedIndex = ButtonContainer.Children.IndexOf(bt);

        }


        private void flipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedItemIndex;
            try
            {
                selectedItemIndex = flipView.SelectedIndex;
            }
            catch (NullReferenceException)
            { return; }
            for (int i = 0; i < 4; i++)
            {
                if (i == selectedItemIndex)
                {
                    Button bt = ButtonContainer.Children.OfType<Button>().ElementAt(selectedItemIndex);
                    bt.Background = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255));
                    bt.Foreground = new SolidColorBrush(Colors.Black);
                }

                else
                {
                    Button bt = ButtonContainer.Children.OfType<Button>().ElementAt(i);
                    bt.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    bt.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                }
            }
        }
    }
}
