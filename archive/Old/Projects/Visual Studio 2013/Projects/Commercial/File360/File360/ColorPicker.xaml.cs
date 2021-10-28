using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace File360
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            #region blahblahblah
            //            string[] colorNames =
            //{
            //  "Yellow","BananaYellow","LaserLemon","Jasmine","Green","Emerald",
            //  "GreenYellow","Lime","Chartreuse","LimeGreen","SpringGreen","LightGreen",
            //  "MediumSeaGreen","MediumSpringGreen","Olive","SeaGreen","Red","OrangeRed",
            //  "DarkOrange","Orange","ImperialRed","Maroon","Brown","Chocolate",
            //  "Coral","Crimson","DarkSalmon","DeepPink","#Firebrick","HotPink",
            //  "IndianRed","LightCoral","LightPink","LightSalmon","Magenta","MediumVioletRed",
            //  "Orchid","PaleVioletRed","Salmon","SandyBrown","Navy","Indigo",
            //  "MidnightBlue","Blue","Purple","BlueViolet","CornflowerBlue","Cyan",
            //  "DarkCyan","DarkSlateBlue","DeepSkyBlue","DodgerBlue","LightBlue","LightSeaGreen",
            //  "LightSkyBlue","LightSteelBlue","Mauve","MediumSlateBlue","RoyalBlue","SlateBlue",
            //  "SlateGray","SteelBlue","Teal","Turquoise","DarkGrey","LightGray"
            //};
            #endregion

        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Height = 50;
        }

        private void ColorSetInitializer(string n)
        {
            if (n == "0")
            {
                string[] accentColors =
                {
            "#FFFFFF00","#FFFFE135","#FFFFFF66","#FFF8DE7E","#FF008000","#FF008A00",
            "#FFADFF2F","#FF00FF00","#FF7FFF00","#FF32CD32","#FF00FF7F","#FF90EE90",
            "#FF3CB371","#FF00FA9A","#FF808000","#FF2E8B57","#FFFF0000","#FFFF4500",
            "#FFFF8C00","#FFFFA500","#FFED2939","#FF800000","#FFA52A2A","#FFD2691E",
            "#FFFF7F50","#FFDC143C","#FFE9967A","#FFFF1493","#FFB22222","#FFFF69B4",
            "#FFCD5C5C","#FFF08080","#FFFFB6C1","#FFFFA07A","#FFFF00FF","#FFC71585",
            "#FFDA70D6","#FFDB7093","#FFFA8072","#FFF4A460","#FF000080","#FF4B0082",
            "#FF191970","#FF0000FF","#FF800080","#FF8A2BE2","#FF6495ED","#FF00FFFF",
            "#FF008B8B","#FF483D8B","#FF00BFFF","#FF1E90FF","#FFADD8E6","#FF20B2AA",
            "#FF87CEFA","#FFB0C4DE","#FF76608A","#FF7B68EE","#FF4169E1","#FF6A5ACD",
            "#FF708090","#FF4682B4","#FF008080","#FF40E0D0","#FFA9A9A9","#FFD3D3D3"
            };
                List<ColorList> accentItem = new List<ColorList>();
                for (int i = 0; i < 66; i++)
                {
                    accentItem.Add(new ColorList(accentColors[i]));
                }
                ColorContainer.ItemsSource = accentItem;
            }
            if (n == "1")
            {
                string[] themeColors =
                {
                "#FFFFFFFF","#FF000000"
            };
                List<ColorList> accentItem = new List<ColorList>();
                for (int i = 0; i < 2; i++)
                {
                    accentItem.Add(new ColorList(themeColors[i]));
                }
                ColorContainer.ItemsSource = accentItem;
            }
            if (n == "2")
            {
                string[] folderColors =
                {
            "#FFFFFF00","#FFFFE135","#FFFFFF66","#FFF8DE7E","#FF008000","#FF008A00",
            "#FFADFF2F","#FF00FF00","#FF7FFF00","#FF32CD32","#FF00FF7F","#FF90EE90",
            "#FF3CB371","#FF00FA9A","#FF808000","#FF2E8B57","#FFFF0000","#FFFF4500",
            "#FFFF8C00","#FFFFA500","#FFED2939","#FF800000","#FFA52A2A","#FFD2691E",
            "#FFFF7F50","#FFDC143C","#FFE9967A","#FFFF1493","#FFB22222","#FFFF69B4",
            "#FFCD5C5C","#FFF08080","#FFFFB6C1","#FFFFA07A","#FFFF00FF","#FFC71585",
            "#FFDA70D6","#FFDB7093","#FFFA8072","#FFF4A460","#FF000080","#FF4B0082",
            "#FF191970","#FF0000FF","#FF800080","#FF8A2BE2","#FF6495ED","#FF00FFFF",
            "#FF008B8B","#FF483D8B","#FF00BFFF","#FF1E90FF","#FFADD8E6","#FF20B2AA",
            "#FF87CEFA","#FFB0C4DE","#FF76608A","#FF7B68EE","#FF4169E1","#FF6A5ACD",
            "#FF708090","#FF4682B4","#FF008080","#FF40E0D0","#FFA9A9A9","#FFD3D3D3"
            };
                List<ColorList> accentItem = new List<ColorList>();
                for (int i = 0; i < 66; i++)
                {
                    accentItem.Add(new ColorList(folderColors[i]));
                }
                ColorContainer.ItemsSource = accentItem;
            }
        }

        public string ColorSet
        {
            set { ColorSetInitializer(value); }
        }

        public string OptionName
        {
            get { return ColorProperty.Text; }
            set { ColorProperty.Text = value; }
        }
        //public event EventArgs ColorSelected;
        //private void lstColorPalette_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (ColorSelected != null)
        //    {
        //        //SelectedColor = objColor.ColorBrush;
        //        //ColorSelectedEvent(this, EventArgs.Empty);
        //    }
        //}
        public SolidColorBrush SelectedColor
        {
            get { return new SolidColorBrush(StringToColor(CurrentColorItem.Color)); }
        }

        public ColorList CurrentColorItem { set; get; }

        private void ColorPickerPage_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private static Color StringToColor(string s)
        {
            // remove artifacts
            s = s.Trim().TrimStart('#');

            // only 8 (with alpha channel) or 6 symbols are allowed
            if (s.Length != 8 && s.Length != 6)
                throw new ArgumentException("Unknown string format!");

            int startParseIndex = 0;
            bool alphaChannelExists = s.Length == 8; // check if alpha canal exists            

            // read alpha channel value
            byte a = 255;
            if (alphaChannelExists)
            {
                a = System.Convert.ToByte(s.Substring(0, 2), 16);
                startParseIndex += 2;
            }

            // read r value
            byte r = System.Convert.ToByte(s.Substring(startParseIndex, 2), 16);
            startParseIndex += 2;
            // read g value
            byte g = System.Convert.ToByte(s.Substring(startParseIndex, 2), 16);
            startParseIndex += 2;
            // read b value
            byte b = System.Convert.ToByte(s.Substring(startParseIndex, 2), 16);

            return Color.FromArgb(a, r, g, b);
        }
        private void ColorContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                CurrentColorItem = ((ColorList)e.AddedItems[0]);
                RectangleFill.Fill = new SolidColorBrush(StringToColor(CurrentColorItem.Color));
            }
        }
        private void ButtonCheck(object sender, RoutedEventArgs e)
        {
            stackPanel.Height = 300;
        }
        private void ButtonUncheck(object sender, RoutedEventArgs e)
        {
            stackPanel.Height = 50;
        }
    }
}
