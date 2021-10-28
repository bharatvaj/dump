using _18PLUSINDIAN.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace _18PLUSINDIAN
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListPage : Page
    {


        ObservableCollection<Entity> entity = new ObservableCollection<Entity>();
        public ListPage()
        {
            this.InitializeComponent();
            listView.ItemsSource = entity;
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Loaded += ListPage_Loaded;
        }

        private void ListPage_Loaded(object sender, RoutedEventArgs e)
        {
            entity.Add(new Entity("Name", "", null));
            entity.Add(new Entity("Name", "", null));
            entity.Add(new Entity("Name", "", null));
            entity.Add(new Entity("Name", "", null));
            entity.Add(new Entity("Name", "", null));
            entity.Add(new Entity("Name", "", null));
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            this.Frame.GoBack();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
