using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using File360.Resources;
using System.Threading.Tasks;
using Microsoft.Phone.Storage;
using System.IO;
using System.Collections;
using System.Windows.Media;
using Windows.Phone.Storage.SharedAccess;
using System.Data.Linq;
using Windows.Storage;
using System.Text;
using Windows.ApplicationModel;
using Windows.Phone.Devices.Notification;
using Windows.Phone.UI.Input;
using System.ComponentModel;
using System.Resources;
using System.Windows.Input;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;



namespace File360
{
    public partial class MainPage : PhoneApplicationPage
    {
        List<sdlist> l = new List<sdlist>();
        List<musiclist> ml = new List<musiclist>();
        const string SETTINGS_PAGE_URI = "/SettingsPage.xaml";
        const string VAULT_PAGE_URI = "/VaultPage.xaml";
        public string btns;
        public static string Hbtns;
        ExternalStorageFolder temp;
        TextBox pb;
        IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        internal static BitmapImage bm = new BitmapImage(new Uri("pack://application:/Resources/Assets/Wallpaper/bg1.jpg", UriKind.Absolute));
        public MainPage()
        {
            InitializeComponent();
            VisualStateManager.GoToState(this, "Normal", false);
            #region InitialKeyUpdater
            if (appSettings.Count == 0)
            {
                appSettings.Add("Shaker","Off");
                appSettings.Save();
                appSettings.Add("Passer","Off");
                appSettings.Save();
                appSettings.Add("PasswordValue", "2580");
                appSettings.Save();
            }
            #endregion
            #region ThemeChecker
            //Visibility isVisible = (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"];
            //if (isVisible == System.Windows.Visibility.Visible)
            //{

            //}
            //else
            //{

            //}
            #endregion
            StateHelper();
            WallpaperChanger(bm);
            MusicStateCreator();
        }

        #region ContextMenu


        private void vault_button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Button b = (Button)sender;
            Hbtns = (b.Content).ToString();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Copied To Clipboard");
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ready to be Pasted");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("File Couldn't be deleted");
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            MenuItem m = (MenuItem)sender;
            sdlists.ItemsSource = l;
        }

        #endregion

        #region FileLister
        async public void StateHelper()
        {
            ExternalStorageDevice sdCard = (await ExternalStorage.GetExternalStorageDevicesAsync()).FirstOrDefault();
            if (sdCard != null)
            {
               ExternalStorageFolder sdrootFolder = sdCard.RootFolder;
               temp = sdrootFolder;
               var folder = await sdrootFolder.GetFoldersAsync();
               var files = await sdrootFolder.GetFilesAsync();
                foreach(ExternalStorageFolder currentChildFolder in folder)
                {
                    l.Add(new sdlist(currentChildFolder.Name, "/Resources/Assets/Images/folder.png"));
                }
                foreach (ExternalStorageFile currentChildFile in files)
                {
                    if (currentChildFile.Name.EndsWith(".pdf"))
                        l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/pdf.png"));
                    if (currentChildFile.Name.EndsWith(".svg"))
                        l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/svg.png"));
                    if (currentChildFile.Name.EndsWith(".mkv"))
                        l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/mkv.png"));
                    if (currentChildFile.Name.EndsWith(".rar"))
                        l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/rar.png"));
                    if (currentChildFile.Name.EndsWith(".csv"))
                        l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/csv.png"));
                    if (currentChildFile.Name.EndsWith(".avi"))
                        l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/avi.png"));
                    if (currentChildFile.Name.EndsWith(".7z"))
                        l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/7z.png"));

                }
            }

            else
            {
                nosd.Visibility = System.Windows.Visibility.Visible;
            }
            sdlists.ItemsSource = l;
            
        }
        async private void folderTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {

            StackPanel sp = (StackPanel)sender;
            TextBlock btn = (TextBlock)sp.FindName("folderTap");
            btns = btn.Text;
            if (btns.EndsWith(".7z") || btns.EndsWith(".avi") || btns.EndsWith(".mkv"))
            {
                if (btns.EndsWith(".7z"))
                {
                    MessageBox.Show("It's a 7-zip File");
                }
                if (btns.EndsWith(".avi"))
                {
                    MessageBox.Show("It's a avi File");
                }
                if (btns.EndsWith(".mkv"))
                {
                    MessageBox.Show("It's a mkv File");
                }
            }
            else
            {
                var temps = await temp.GetFoldersAsync();
                l.Clear();
                foreach (ExternalStorageFolder subTemp in temps)
                {
                    if (btns == subTemp.Name)
                    {
                        temp = subTemp;
                        textBlock.Text = subTemp.Path;
                        sd.Text = subTemp.Name;
                        var subtemps = await subTemp.GetFoldersAsync();
                        var files = await subTemp.GetFilesAsync();
                        l.Clear();
                        l.Add(new sdlist("back", "/Resources/Assets/Images/back.png"));
                        foreach (ExternalStorageFolder subsTemp in subtemps)
                        {
                            l.Add(new sdlist(subsTemp.Name, "/Resources/Assets/Images/folder.png"));
                        }
                        foreach (ExternalStorageFile currentChildFile in files)
                        {
                            if (currentChildFile.Name.EndsWith(".pdf"))
                                l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/pdf.png"));
                            if (currentChildFile.Name.EndsWith(".svg"))
                                l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/svg.png"));
                            if (currentChildFile.Name.EndsWith(".mkv"))
                                l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/mkv.png"));
                            if (currentChildFile.Name.EndsWith(".rar"))
                                l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/rar.png"));
                            if (currentChildFile.Name.EndsWith(".csv"))
                                l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/csv.png"));
                            if (currentChildFile.Name.EndsWith(".avi"))
                                l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/avi.png"));
                            if (currentChildFile.Name.EndsWith(".7z"))
                                l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/7z.png"));
                        }
                    }
                }
            }
            sdlists.ItemsSource = l;
        }

        #endregion

        #region AppBarButtons


        void NewFolder_Click(object sender, EventArgs e)
        {
            l.Add(new sdlist("New Folder", "/Resources/Assets/Images/folder.png"));
            sdlists.ItemsSource = l;
        }

        private void MultiSelect_Click(object sender, EventArgs e)
        {
            if (sdlists.IsSelectionEnabled == false)
            {
                sdlists.IsSelectionEnabled = true;
            }
            if (sdlists.IsSelectionEnabled == true)
            {
                sdlists.IsSelectionEnabled = false;
            }
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(SETTINGS_PAGE_URI, UriKind.Relative));
        }
        

        #endregion



        #region BackButtonControl

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (textBlock.Text == null)
            {
                e.Cancel = false;
            } 
            if (textBlock.Text != null)
            {
                l.Clear();
                StateHelper();
                btns = null;
                e.Cancel = true;
                sd.Text = "sdcard";
            }
            if (sdlists.IsSelectionEnabled == false)
            {
                e.Cancel = false;
            }
            if (sdlists.IsSelectionEnabled == true)
            {
                sdlists.IsSelectionEnabled = false;
                e.Cancel = true;
            }
        }

        #endregion


        #region HiddenVault
        private void vault_button_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if ((string)appSettings["Passer"] == "On")
            {
                pb = new TextBox();
                pb.Background = (SolidColorBrush)Resources["PhoneBackgroundBrush"];
                pb.Foreground = (SolidColorBrush)Resources["PhoneForegroundBrush"];
                pb.BorderBrush = (SolidColorBrush)Resources["PhoneBackgroundBrush"];
                pb.Height = 70;
                pb.Width = 356;
                pb.Margin = new Thickness(124, 0, 0, 625);
                pb.InputScope = new InputScope { Names = { new InputScopeName { NameValue = InputScopeNameValue.Number } } };
                pb.LostFocus += pb_LostFocus;
                pb.GotFocus += pb_GotFocus;
                LayoutRoot.Children.Add(pb);
                pb.Focus();
                pb.TextChanged += PChanged;
            }

            if ((string)appSettings["Passer"] == "Off")
            {
                NavigationService.Navigate(new Uri(VAULT_PAGE_URI, UriKind.Relative));
            }
        }

        private void PChanged(object sender, TextChangedEventArgs e)
        {
            if (pb.Text == (string)appSettings["PasswordValue"])
            {
                LayoutRoot.Children.Remove(pb);
                NavigationService.Navigate(new Uri(VAULT_PAGE_URI, UriKind.Relative));
            }
        }
        void pb_GotFocus(object sender, RoutedEventArgs e)
        {
            pb.Background = (SolidColorBrush)Resources["PhoneBackgroundBrush"];
            pb.Foreground = (SolidColorBrush)Resources["PhoneForegroundBrush"];
            pb.MaxLength = 5;
            pb.BorderBrush = (SolidColorBrush)Resources["PhoneBackgroundBrush"];
            pb.TextAlignment = TextAlignment.Center;
        }

        private void pb_LostFocus(object sender, RoutedEventArgs e)
        {
            LayoutRoot.Children.Remove(pb);
        }

        #endregion

        #region LateralMenu
        private void OpenClose_Left(object sender, RoutedEventArgs e)
        {
            var left = Canvas.GetLeft(CanvasRoot);
            if (left > -100)
            {
                ApplicationBar.IsVisible = true;
                MoveViewWindow(-320);
                Blurer.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                ApplicationBar.IsVisible = false;
                MoveViewWindow(0);
                Blurer.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void OpenClose_Right(object sender, RoutedEventArgs e)
        {
            var left = Canvas.GetLeft(CanvasRoot);
            if (left > -350)
            {
                ApplicationBar.IsVisible = false;
                MoveViewWindow(-680);
                Blurer.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ApplicationBar.IsVisible = true;
                MoveViewWindow(-320);
                Blurer.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        void MoveViewWindow(double left)
        {
            if (left == -320)
                ApplicationBar.IsVisible = true;
            else
                ApplicationBar.IsVisible = false;
            ((Storyboard)canvas.Resources["moveAnimation"]).SkipToFill();
            ((DoubleAnimation)((Storyboard)canvas.Resources["moveAnimation"]).Children[0]).To = left;
            ((Storyboard)canvas.Resources["moveAnimation"]).Begin();
        }

        private void canvas_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.DeltaManipulation.Translation.X != 0)
                Canvas.SetLeft(CanvasRoot, Math.Min(Math.Max(-680, Canvas.GetLeft(CanvasRoot) + e.DeltaManipulation.Translation.X), 0));
        }

        double initialPosition;
        bool _viewMoved = false;
        private void canvas_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _viewMoved = false;
            initialPosition = Canvas.GetLeft(CanvasRoot);
        }

        private void canvas_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            var left = Canvas.GetLeft(CanvasRoot);
            if (_viewMoved)
                return;
            if (Math.Abs(initialPosition - left) < 100)
            {
                //bouncing back
                MoveViewWindow(initialPosition);
                return;
            }
            //change of state
            if (initialPosition - left > 0)
            {
                //slide to the left
                if (initialPosition > -320)
                    MoveViewWindow(-320);
                else
                    MoveViewWindow(-680);
            }
            else
            {
                //slide to the right
                if (initialPosition < -320)
                    MoveViewWindow(-320);
                else
                    MoveViewWindow(0);
            }

        }

        #endregion

        private void downloads_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            sd.Text = "downloads";
            textBlock.Text = "downloads";
            sdcard_menu.Background = null;
            l.Clear();
            List<AlphaKeyGroup<sdlist>> DataSource = AlphaKeyGroup<sdlist>.CreateGroups(l, System.Threading.Thread.CurrentThread.CurrentUICulture, (sdlist s) => { return s.Name; }, true);
            sdlists.ItemsSource = DataSource;
            MoveViewWindow(-320);
            dl.Fill = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
            rect_above.Fill = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
            pl.Fill = null;
            Blurer.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void sdcard_menu_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            sd.Text = "sdcard";
            textBlock.Text = null;
            l.Clear();
            StateHelper();
            MoveViewWindow(-320);
            //HomeBrush.ImageSource = new BitmapImage(new Uri("pack://application:/Toolkit.Content/home.png", UriKind.Absolute));
            pl.Fill = null;
            dl.Fill = null;
            oneFill.Fill = null;
            sdcard_menu.Background = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
            rect_above.Fill = null;
            Blurer.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void WallpaperChanger(BitmapImage bitm)
        {
            ImageBrush im = new ImageBrush();
            im.ImageSource = bitm;
            ApplyGaussianFilter(im.ImageSource);
        }
        
        public void ApplyGaussianFilter(ImageSource image)
        {
            ImageApply(image);
        }

        public void ImageApply(ImageSource imt)
        {
            ImageBrush im = new ImageBrush();
            im.Stretch = Stretch.UniformToFill;
            im.ImageSource = imt;
            SideBar.Background = im;
        }
        #region Music

        public void MusicStateCreator()
        {
            int trackno = 0;
            try
            {
                l.Clear();
                ml.Clear();
                using (var library = new MediaLibrary())
                {
                    SongCollection songs = library.Songs;
                    foreach (Song song in songs)
                    {
                        ml.Add(new musiclist((song.Name).ToString(), (song.Album).ToString(), (song.Artist).ToString(), (trackno+1).ToString()));
                    }
                    musicLists.ItemsSource = ml;
                    sdlists.ItemsSource = ml;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("There are no songs in your phone...");
            }
            catch (InvalidOperationException iop)
            {
                MessageBox.Show(iop.ToString());
            }
        }

        private void Music_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int trackno = 0;
            //ImageBrush ib = new ImageBrush();
            //ib.ImageSource = new BitmapImage(new Uri("pack://application:/Resources/Assets/Images/MusicBar.png", UriKind.Absolute));
            //homeButton.OpacityMask = ib;
            sd.Text = "music";
            pl.Fill = null;
            dl.Fill = null;
            rect_above.Fill = null;
            oneFill.Fill = null;
            sdcard_menu.Background = null;
            Blurer.Visibility = System.Windows.Visibility.Collapsed;
            try
            {
                l.Clear();
                ml.Clear();
                using (var library = new MediaLibrary())
                {
                SongCollection songs = library.Songs;
                foreach (Song song in songs)
                {
                    ml.Add(new musiclist((song.Name).ToString(), (song.Artist).ToString(), (song.Album).ToString(), (trackno+1).ToString()));
                }
                musicLists.ItemsSource = ml;
                sdlists.ItemsSource = ml;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("There are no songs in your phone...");
            }
        }


        public void txtB_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock txt = (TextBlock)sender;
            string txts = (txt.Name).ToString();
            int index = int.Parse(txts);
            MainPage mp = new MainPage();
            using (var library = new MediaLibrary())
            {
                SongCollection songs = library.Songs;
                Song song = songs[index];
                try
                {
                    FrameworkDispatcher.Update();
                    MediaPlayer.Play(song);
                }
                catch (InvalidOperationException)
                {
                    MediaPlayer.Play(songs[2]);
                }
            }
        }

        #endregion
        async public void VideoLister(object sender, System.Windows.Input.GestureEventArgs e)
        {
            pl.Fill = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
            dl.Fill = null;
            rect_above.Fill = null;
            sdcard_menu.Background = null;
            oneFill.Fill = null;
            sd.Text = "videos";
            Blurer.Visibility = System.Windows.Visibility.Collapsed;
            //HomeBrush.ImageSource = new BitmapImage(new Uri("pack://application:/Resources/Assets/Images/Video.png", UriKind.Absolute));
            ExternalStorageDevice sdCard = (await ExternalStorage.GetExternalStorageDevicesAsync()).FirstOrDefault();
            if (sdCard != null)
            {
                ExternalStorageFolder sdrootFolder = sdCard.RootFolder;
                temp = sdrootFolder;
                var folder = await sdrootFolder.GetFoldersAsync();
                l.Clear();
                foreach (ExternalStorageFolder currentChildFolder in folder)
                    {
                        var files = await currentChildFolder.GetFilesAsync();
                        foreach (ExternalStorageFile currentChildFile in files)
                        {
                            if (currentChildFile.Name.EndsWith(".avi"))
                                    l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/avi.png"));
                            if (currentChildFile.Name.EndsWith(".mkv"))
                                l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/mkv.png"));
                        }
                        var folderss = await currentChildFolder.GetFoldersAsync();
                        foreach (ExternalStorageFolder currentChildFolder2 in folderss)
                        {
                            var filess = await currentChildFolder2.GetFilesAsync();
                            foreach (ExternalStorageFile currentChildFile in filess)
                            {
                                if (currentChildFile.Name.EndsWith(".avi"))
                                    l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/avi.png"));
                                if (currentChildFile.Name.EndsWith(".mkv"))
                                    l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/mkv.png"));
                            }
                        }

                    }
                
            }

            else
            {
                nosd.Visibility = System.Windows.Visibility.Visible;
            }
            sdlists.ItemsSource = l;
        }

        private void oneFill_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            oneFill.Fill = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
            pl.Fill = null;
            dl.Fill = null;
            rect_above.Fill = null;
            Blurer.Visibility = System.Windows.Visibility.Collapsed;
        }

        async private void SearchModule_Click(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text != null)
            {
                ExternalStorageDevice sdCard = (await ExternalStorage.GetExternalStorageDevicesAsync()).FirstOrDefault();
                ExternalStorageFolder sdrootFolder = sdCard.RootFolder;
                temp = sdrootFolder;
                var folder = await sdrootFolder.GetFoldersAsync();
                foreach (ExternalStorageFolder currentChildFolder in folder)
                {
                    var files = await currentChildFolder.GetFilesAsync();
                    foreach (ExternalStorageFile currentChildFile in files)
                    {
                        l.Clear();
                        if (currentChildFile.Name.EndsWith(searchBox.Text))
                            l.Add(new sdlist(searchBox.Text, "/Resources/Assets/Images/folder.png"));
                    }
                }
                sdlists.ItemsSource = l;
                
            }

        }
    }
}