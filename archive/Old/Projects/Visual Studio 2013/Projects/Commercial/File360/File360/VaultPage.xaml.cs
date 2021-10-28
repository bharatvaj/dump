using File360.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace File360
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VaultPage : Page
    {

        App app = (App)Application.Current;
        string PASS_KEY = "2580";
        public VaultPage()
        {
            this.InitializeComponent();
            Loaded += VaultPage_Loaded;
        }

        private void VaultPage_Loaded(object sender, RoutedEventArgs e)
        {
            passwordBox.Focus(FocusState.Pointer);
            //int btNo = 0;
            //for (int i = 0; i <= 3; i++)
            //{
            //    for (int j = 0; j < 3; j++)
            //    {
            //        btNo++;
            //        Button bt = new Button();
            //        bt.Content = btNo;
            //        bt.HorizontalAlignment = HorizontalAlignment.Stretch;
            //        bt.VerticalAlignment = VerticalAlignment.Stretch;
            //        bt.Click += (o, t) =>
            //        {
            //            passwordBox.Text += bt.Content;
            //        };
            //        Grid.SetRow(bt, i);
            //        Grid.SetColumn(bt, j);
            //        passwordGrid.Children.Add(bt);
            //    }
            //}
        }

        private void passwordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (passwordBox.Text.Count() == 4 && passwordBox.Text != PASS_KEY)
            {
                AuthenticationFailed.Begin();
                passwordBox.Text = "";
            }
            
            ((Grid)(((Grid)this.Frame.Parent).Parent)).Visibility = Visibility.Collapsed;
            this.Frame.Content = null;

        }
        

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            passwordBox.Focus(FocusState.Pointer);
        }

        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ((Grid)(((Grid)this.Frame.Parent).Parent)).Visibility = Visibility.Collapsed;
            this.Frame.Content = null;

        }
    }
}
