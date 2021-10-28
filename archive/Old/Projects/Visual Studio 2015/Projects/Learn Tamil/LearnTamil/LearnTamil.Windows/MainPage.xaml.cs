using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
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
            ImageBrush im = new ImageBrush();
            im.Stretch = Stretch.UniformToFill;
            im.ImageSource = new BitmapImage(new Uri("ms-appx:///dCtaCBG.png"));
            wi.Add(new DataModel.WordItems("வணக்கம் உங்கள் பெயர் என்ன ? - உங்க பேரு என்ன ?  Hello, What's your name?", "1", im, false));
            ImageBrush im1 = new ImageBrush();
            im1.Stretch = Stretch.UniformToFill;
            im.ImageSource = new BitmapImage(new Uri("ms-appx:///SpeakTamil4.jpg"));
            wi.Add(new DataModel.WordItems("Chapter", "2", im1, false));
            wi.Add(new DataModel.WordItems("Chapter", "3", null, true));
            wi.Add(new DataModel.WordItems("Chapter", "4", null, true));
            wi.Add(new DataModel.WordItems("Chapter", "5", null, true));
        }
        
        public void Status(string msg)
        {
            info.Text += msg;
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
                GridViewItem gvI = (GridViewItem)gridView.ContainerFromItem(e.ClickedItem);
                DependencyObject border = (DependencyObject)gvI.Content;
                info.Text = border.GetType().Name;
                //MessageDialog md = new MessageDialog("This content should be unlocked from store. Continue?"+bbb.GetType().Name);
                //await md.ShowAsync();
            }
        }

        private void HeaderTapped(object sender, TappedRoutedEventArgs e)
        {
            Border border = (Border)sender;
            int index = gridView.Items.IndexOf((DataModel.WordItems)(border.DataContext));
            var selectedItem = wi.ElementAt(index);
            if (!selectedItem.Locked)
            {
                app.ChapterName = selectedItem.ChapterName;
                app.ChapterNo = selectedItem.ChapterNo;
                app.ChapterImage = selectedItem.ChapterImage;
                border.RenderTransform = new CompositeTransform();
                Storyboard storyboard = new Storyboard();
                DoubleAnimation translateX = new DoubleAnimation
                {
                    To = -25,
                    Duration = new TimeSpan(0, 0, 0,0,300)
                };
                DoubleAnimation translateY = new DoubleAnimation
                {
                    To = -25,
                    Duration = new TimeSpan(0, 0, 0,0,300)
                };
                //DoubleAnimation opacityDown = new DoubleAnimation
                //{
                //    To = 0,
                //    Duration = new TimeSpan(0, 0, 1)
                //};
                Storyboard.SetTarget(translateX, border);
                Storyboard.SetTarget(translateY, border);
                //Storyboard.SetTarget(opacityDown, border);
                Storyboard.SetTargetProperty(translateX, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");
                Storyboard.SetTargetProperty(translateY, "(UIElement.RenderTransform).(CompositeTransform.ScaleY)");
                //Storyboard.SetTargetProperty(opacityDown, "(UIElement.Opacity)");
                storyboard.Children.Add(translateX);
                storyboard.Children.Add(translateY);
                //storyboard.Children.Add(opacityDown);
                storyboard.Begin();
                storyboard.Completed += delegate
                {
                    this.Frame.Navigate(typeof(HubPage));
                };
            }
            else
            {
                border.RenderTransform = new CompositeTransform();
                Storyboard storyboard = new Storyboard();
                DoubleAnimation translateX = new DoubleAnimation
                {
                    To = 25,
                    Duration = new TimeSpan(0, 0, 1)
                };
                //DoubleAnimation scaleUpY = new DoubleAnimation
                //{
                //    To = 25,
                //    Duration = new TimeSpan(0, 0, 1)
                //};
                //DoubleAnimation opacityDown = new DoubleAnimation
                //{
                //    To = 0,
                //    Duration = new TimeSpan(0, 0, 1)
                //};
                Storyboard.SetTarget(translateX, border);
                //Storyboard.SetTarget(scaleUpY, border);
                //Storyboard.SetTarget(opacityDown, border);
                Storyboard.SetTargetProperty(translateX, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");
                //Storyboard.SetTargetProperty(scaleUpY, "(UIElement.RenderTransform).(CompositeTransform.ScaleY)");
                //Storyboard.SetTargetProperty(opacityDown, "(UIElement.Opacity)");
                storyboard.Children.Add(translateX);
                //storyboard.Children.Add(scaleUpY);
                //storyboard.Children.Add(opacityDown);
                storyboard.Begin();
            }
        }

        private void HeaderPressed(object sender, PointerRoutedEventArgs e)
        {
            
        }
        //DependencyObject findElementInItemsControlItemAtIndex(ItemsControl itemsControl,
        //                                                        int itemOfIndexToFind)
        //{
        //    if (itemOfIndexToFind >= itemsControl.Items.Count) return null;

        //    DependencyObject depObj = null;
        //    object o = itemsControl.Items[itemOfIndexToFind];
        //    if (o != null)
        //    {
        //        var item = itemsControl.Conta
        //        info.Text = item.GetType().Name;
        //    }
        //    return null;
        //}


    }
}
