using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            StateInitializer();
        }

        private void StateInitializer()
        {
            foreach (string s in Directory.GetLogicalDrives())
            {
                ImageBrush imgBrush = new ImageBrush();
                imgBrush.ImageSource = new BitmapImage(new Uri("\\Images\\harddrive.png", UriKind.Relative));
                WrapPanel grd = new WrapPanel();
                grd.Width = 100;
                grd.Height = 100;
                Rectangle rct = new Rectangle();
                rct.Fill = new SolidColorBrush(Color.FromRgb(118, 118, 118));
                rct.OpacityMask = imgBrush;
                rct.Stretch = Stretch.Uniform;
                rct.Width = 80;
                rct.Height = 50;
                rct.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                rct.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                TextBlock btn = new TextBlock();
                btn.Tag = s;
                btn.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                btn.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                btn.Text = s;
                btn.FontSize = 12;
                btn.Width = 85;
                btn.Height = 30;
                btn.MouseUp += btn_Click;
                grd.Children.Add(rct);
                grd.Children.Add(btn);
                folderPanel.Children.Add(grd);
            }
        }
        void StateHelper(string filePath)
        {
            try
            {
                foreach (string s in Directory.GetDirectories(filePath))
                {
                    WrapPanel grd = new WrapPanel();
                    grd.Width = 100;
                    grd.Height = 100;
                    Rectangle rct = new Rectangle();
                    rct.Fill = new SolidColorBrush(Color.FromRgb(238, 238, 30));
                    rct.Width = 80;
                    rct.Height = 50;
                    rct.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    rct.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    TextBlock btn = new TextBlock();
                    btn.Tag = s;
                    btn.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    btn.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    btn.Text = s.Substring(s.LastIndexOf("\\") + 1);
                    btn.FontSize = 12;
                    btn.Width = 85;
                    btn.Height = 30;
                    btn.MouseUp += btn_Click;
                    grd.Children.Add(rct);
                    grd.Children.Add(btn);
                    folderPanel.Children.Add(grd);
                }
            }
            catch(UnauthorizedAccessException)
            {
                MessageBox.Show("Current Directory is Unauthorized");
            }
            try
            {

                foreach (string s in Directory.GetFiles(filePath))
                {
                    WrapPanel grd = new WrapPanel();
                    grd.Width = 100;
                    grd.Height = 100;
                    Rectangle rct = new Rectangle();
                    rct.Fill = new SolidColorBrush(Color.FromRgb(38, 84, 91));
                    rct.Width = 80;
                    rct.Height = 50;
                    rct.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    rct.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    TextBlock btn = new TextBlock();
                    btn.Tag = s;
                    btn.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    btn.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    btn.Text = s.Substring(s.LastIndexOf("\\") + 1);
                    btn.FontSize = 12;
                    btn.Width = 85;
                    btn.Height = 30;
                    btn.MouseUp += btn_Click;
                    grd.Children.Add(rct);
                    grd.Children.Add(btn);
                    folderPanel.Children.Add(grd);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Current File is Unauthorized");
            }
        }
        void btn_Click(object sender, MouseButtonEventArgs e)
        {
            TextBlock btn = (TextBlock)sender;
            try
            {
                folderPanel.Children.Clear();
                StateHelper(@btn.Tag.ToString());
            }
            catch (IOException)
            {
                MessageBox.Show("IO Error");
            }
            catch (NullReferenceException nr)
            {
                MessageBox.Show("Invalid File at "+ nr.ToString());
            }
        }
    }
}
