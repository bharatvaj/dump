using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Storage;
using System.Windows.Media;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using Microsoft.Devices.Sensors;

namespace File360
{
    public partial class MainPage : PhoneApplicationPage
    {
        List<sdlist> l = new List<sdlist>();
        ExternalStorageFolder tempFolder;
        IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        public string btns;
        string txts;
        List<settinglist> sl = new List<settinglist>();
        public MainPage()
        {
            InitializeComponent();
            StateHelper();
            VisualStateManager.GoToState(this, "Normal", false);
            #region InitialKeyUpdater
            if (appSettings.Count == 0)
            {
                appSettings.Add("Shaker", "Off");
                appSettings.Save();
                appSettings.Add("Passer", "Off");
                appSettings.Save();
                appSettings.Add("PasswordValue", "2580");
                appSettings.Save();
                appSettings.Add("WallPaper", "/Resources/Assets/Wallpaper/bg1.jpg");
                appSettings.Save();
            }
            #endregion
            PopupPasswordKeyboard();
            #region InitialSettings
            sl.Add(new settinglist("ftp", "configure file transfer protocol", "/Resources/Assets/Settings/ftp.png"));
            sl.Add(new settinglist("bluetooth", "bluetooth settings", "/Resources/Assets/Settings/bluetooth.png"));
            sl.Add(new settinglist("nfc", "nfc configuration", "/Resources/Assets/Settings/nfc.png"));
            sl.Add(new settinglist("personalization", "get a look", "/Resources/Assets/Settings/personalization.png"));
            sl.Add(new settinglist("music", "change your music settings", "/Resources/Assets/Settings/music.png"));
            sl.Add(new settinglist("security", "protect your files", "/Resources/Assets/Settings/security.png"));
            sl.Add(new settinglist("about", "About Us!", "/Resources/Assets/Settings/about.png"));
            SettingsList.ItemsSource = sl;
    #endregion
            MusicDisplay();
            #region ThemeChecker
            Visibility themeHold = (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"];
            if (themeHold == System.Windows.Visibility.Visible)
            {
                ApplicationBar.BackgroundColor = Colors.White;
                ApplicationBar.ForegroundColor = Colors.Black;
                LeftGrid.Background = App.Current.Resources["PhoneChromeBrush"] as SolidColorBrush;
            }
            #endregion
            #region AccelerometerReader
            if (!Accelerometer.IsSupported)
            {
                shake.IsEnabled = false;
            }
            Settings_Checker();
            #endregion
        }
        
        #region Wallpaper
        public void WallpaperChanger(BitmapImage bitm)
        {
            ApplyGaussianFilter(bitm);
        }
        public void ApplyGaussianFilter(BitmapImage image)
        {
            //do some fancy work
            //then send to ImageApply to apply the Image.
            ImageApply(image);
        }

        public void ImageApply(BitmapImage imt)
        {
            ImageBrush im = new ImageBrush();
            im.ImageSource = imt;
            LayoutRoot.Background = im;
        }
        #endregion
        #region LateralMenu
        private void OpenClose_Left(object sender, RoutedEventArgs e)
        {
            var left = Canvas.GetLeft(LayoutRoot);
            if (left > -100)
            {
                ApplicationBar.IsVisible = true;
                MoveViewWindow(-420);
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
            var left = Canvas.GetLeft(LayoutRoot);
            if (left > -520)
            {
                ApplicationBar.IsVisible = false;
                MoveViewWindow(-840);
                Blurer.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ApplicationBar.IsVisible = true;
                MoveViewWindow(-420);
                Blurer.Visibility = System.Windows.Visibility.Collapsed;
               
            }
        }

        void MoveViewWindow(double left)
        {
            _viewMoved = true;
            if (left==-420)
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
                Canvas.SetLeft(LayoutRoot, Math.Min(Math.Max(-840, Canvas.GetLeft(LayoutRoot) + e.DeltaManipulation.Translation.X), 0));
        }

        double initialPosition;
        bool _viewMoved = false;
        private void canvas_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _viewMoved = false;
            initialPosition = Canvas.GetLeft(LayoutRoot);
        }

        private void canvas_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            var left = Canvas.GetLeft(LayoutRoot);
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
                if (initialPosition > -420)
                {
                     MoveViewWindow(-420);
                Blurer.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                //slide to the right
                if (initialPosition< -420)
                      MoveViewWindow(-420);
                Blurer.Visibility = System.Windows.Visibility.Collapsed;
                
            }

        }
        #endregion
        #region AppBar
        private void MultiSelect_Click(object sender, RoutedEventArgs e)
        {

        }
        private void NewFolder_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Settings_Click(object sender, EventArgs e)
        {
            settings.IsOpen = true;
            ApplicationBar.IsVisible = false;
        }
        #endregion
        #region FileLister
        async public void StateHelper()
        {
            ExternalStorageDevice sdCard = (await ExternalStorage.GetExternalStorageDevicesAsync()).FirstOrDefault();
            if (sdCard != null)
            {
                ExternalStorageFolder sdrootFolder = sdCard.RootFolder;
                tempFolder = sdrootFolder;
                var folder = await sdrootFolder.GetFoldersAsync();
                var files = await sdrootFolder.GetFilesAsync();
                fileHeader.Text = "sdroot";
                fileUrl.Text = " ";
                foreach (ExternalStorageFolder currentChildFolder in folder)
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
                sdlists.ItemsSource = null;
                sdlists.ItemsSource = l;
            }

            else
            {
                nosd.Visibility = System.Windows.Visibility.Visible;
            }
        }
        async private void folderTap_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Grid grd = (Grid)sender;
            TextBlock txt = (TextBlock)grd.FindName("folderTap");
            btns = txt.Text;
            fileHeader.Text = btns;
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
                var tempFolders = await tempFolder.GetFoldersAsync();
                foreach (ExternalStorageFolder subTemp in tempFolders)
                {
                    if (btns == subTemp.Name)
                    {
                        tempFolder = subTemp;
                        var subTemps = await subTemp.GetFoldersAsync();
                        var files = await subTemp.GetFilesAsync();
                        l.Clear();
                        fileUrl.Text = subTemp.Path;
                        //l.Add(new sdlist("back", "/Resources/Assets/Images/back.png"));
                        foreach (ExternalStorageFolder folder in subTemps)
                        {
                            l.Add(new sdlist(folder.Name, "/Resources/Assets/Images/folder.png"));
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
            sdlists.ItemsSource = null;
            sdlists.ItemsSource = l;
        }
	#endregion
        #region Video
        async public void VideoLister(object sender, System.Windows.Input.GestureEventArgs e)
        {
            sdcard_menu.Background = null;
            oneFill.Fill = null;
            fileHeader.Text = "videos";
            Blurer.Visibility = System.Windows.Visibility.Collapsed;
            //HomeBrush.ImageSource = new BitmapImage(new Uri("pack://application:/Resources/Assets/Images/Video.png", UriKind.Absolute));
            ExternalStorageDevice sdCard = (await ExternalStorage.GetExternalStorageDevicesAsync()).FirstOrDefault();
            if (sdCard != null)
            {
                ExternalStorageFolder sdrootFolder = sdCard.RootFolder;
                tempFolder = sdrootFolder;
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
            List<AlphaKeyGroup<sdlist>> DataSource = AlphaKeyGroup<sdlist>.CreateGroups(l, System.Threading.Thread.CurrentThread.CurrentUICulture, (sdlist s) => { return s.Name; }, true);
            sdlists.ItemsSource = DataSource;
        }
        #endregion
        #region PopupPassword
	public void PopupPasswordKeyboard()
    {
	for (int i = 1; i < 10; i++)
	{
	    Button btn = new Button();
	    btn.Content = i.ToString();
        btn.Width = 153.3;
        btn.Height = 102.5;
        btn.BorderBrush = null;
        btn.FontSize = 35;
        btn.Click += new RoutedEventHandler(Numbers_Click);
        wrapPanel.Children.Add(btn);
	}
    }

    private void Numbers_Click(object sender, RoutedEventArgs e)
    {
        Button btn = (Button)sender;
        if (passwordDisplay.Text.Length < 4)
        {
            string i = btn.Content.ToString();
            passwordDisplay.Text += i;
            if(passwordDisplay.Text == (string)appSettings["PasswordValue"])
            {
                passwordDisplay.Text = null;
                PasswordAccepter.IsOpen = false; 
                l.Add(new sdlist("Hidden Folder", "/Resources/Assets/Images/hiddenFolder.png"));
                sdlists.ItemsSource = null;
                sdlists.ItemsSource = l;
            }

        }
        else
        {
            passwordDisplay.Text = null;
            PasswordAccepter.IsOpen = false;
        }
    }
        
    private void Vault_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        if ((string)appSettings["Passer"] == "On")
        {
            PasswordAccepter.IsOpen = true;
            ApplicationBar.IsVisible = false;
        }
        if ((string)appSettings["Passer"] == "Off")
        {
            l.Add(new sdlist("Hidden Folder", "/Resources/Assets/Images/hiddenFolder.png"));
            sdlists.ItemsSource = null;
            sdlists.ItemsSource = l;
        }
    }
    private void PopupCancel(object sender, RoutedEventArgs e)
    {
        PasswordAccepter.IsOpen = false;
    }
    private void PasswordBackspace(object sender, RoutedEventArgs e)
    {
        passwordDisplay.Text = null;
    }
	#endregion
        #region LeftSidemenu
    private void downloads_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        fileHeader.Text = "downloads";
        fileUrl.Text = "downloads";
        sdcard_menu.Background = null;
        l.Clear();
        List<AlphaKeyGroup<sdlist>> DataSource = AlphaKeyGroup<sdlist>.CreateGroups(l, System.Threading.Thread.CurrentThread.CurrentUICulture, (sdlist s) => { return s.Name; }, true);
        sdlists.ItemsSource = DataSource;
        MoveViewWindow(-420);
        Blurer.Visibility = System.Windows.Visibility.Collapsed;
    }

    private void sdcard_menu_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        fileHeader.Text = "sdroot";
        fileUrl.Text = null;
        l.Clear();
        StateHelper();
        MoveViewWindow(-420);
        sdText.Foreground = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
        sdImage.Fill = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
        sdcard_menu.Background = App.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush;
        Blurer.Visibility = System.Windows.Visibility.Collapsed;
    }

    
    //    SideBar.Background = im;
    //}
        
    private void oneFill_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        oneFill.Fill = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
        Blurer.Visibility = System.Windows.Visibility.Collapsed;
    }

    async private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (searchBox.Text != null)
        {
            ExternalStorageDevice sdCard = (await ExternalStorage.GetExternalStorageDevicesAsync()).FirstOrDefault();
            ExternalStorageFolder sdrootFolder = sdCard.RootFolder;
            tempFolder = sdrootFolder;
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
    
    private void lil_search_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        searchBox.Focus();
    }
        #endregion
        #region RightMenu
        #endregion
        #region Music

    private void MusicDisplay()
    {
        if (MediaPlayer.State == MediaState.Playing)
        {
            songName.Text = MediaPlayer.Queue.ActiveSong.Name;
            artistName.Text = (MediaPlayer.Queue.ActiveSong.Artist).ToString();
            ImageBrush im = new ImageBrush();
            im.ImageSource = new BitmapImage(new Uri("pack://application:/Resources/Assets/Images/transport.pause.png", UriKind.Absolute));
            playState.OpacityMask = im;
        }
        if (MediaPlayer.State == MediaState.Paused)
        {
            songName.Text = MediaPlayer.Queue.ActiveSong.Name;
            artistName.Text = (MediaPlayer.Queue.ActiveSong.Artist).ToString();
            ImageBrush im = new ImageBrush();
            im.ImageSource = new BitmapImage(new Uri("pack://application:/Resources/Assets/Images/transport.play.png", UriKind.Absolute));
            playState.OpacityMask = im;
        }
        if (MediaPlayer.State == MediaState.Stopped)
        {
            songName.Text = "No Media";
            artistName.Text = null;
            ImageBrush im = new ImageBrush();
            im.ImageSource = new BitmapImage(new Uri("pack://application:/Resources/Assets/Images/transport.play.png", UriKind.Absolute));
            playState.OpacityMask = im;
        }
    }
    private void playState_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        if (MediaPlayer.State == MediaState.Playing)
        {

            FrameworkDispatcher.Update();
            MediaPlayer.Pause();
            MusicDisplay();
        }
        else
        {
            FrameworkDispatcher.Update();
            MediaPlayer.Resume();
            MusicDisplay();
        }
        //if (MediaPlayer.State == MediaState.Stopped)
        //{
        //    FrameworkDispatcher.Update();
        //    MediaLibrary mLibrary = new MediaLibrary();
        //    SongCollection songs = mLibrary.Songs;
        //    MediaPlayer.Play(songs);
        //    MusicDisplay();
        //}
    }

        #endregion
        #region Personalisation
    private void SideMenuWallChanger_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        WallpaperChanger(new BitmapImage(new Uri("pack://application:/Resources/Assets/Wallpaper/bg2.jpg", UriKind.Absolute)));
    }

    private void Wallpaper_Checked(object sender, RoutedEventArgs e)
    {
        WallpaperTemp.Visibility = Visibility.Visible;
    }

    private void Wallpaper_Unchecked(object sender, RoutedEventArgs e)
    {
        WallpaperTemp.Visibility = Visibility.Collapsed;
    }
    #endregion
        #region Settings
        
        public async void SettingTap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Grid grd = (Grid)sender;
            TextBlock txt = (TextBlock)grd.FindName("txtB");
            txts = (txt.Text).ToString();
            if (txts == "about")
            {
                settings.IsOpen = false;
                about.IsOpen = true;
            }

            if (txts == "security")
            {
                settings.IsOpen = false;
                security.IsOpen = true;
            }

            if (txts == "bluetooth")
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
            }

            if (txts == "nfc")
            {
                MessageBox.Show("coming soon...");
            }

            if (txts == "personalization")
            {
                //settings.IsOpen = false;
                personalization.IsOpen = true;
            }

            if (txts == "ftp")
            {
                NavigationService.Navigate(new Uri("/FTPServerPage.xaml", UriKind.Relative));
            }
            if (txts == "music")
            {
                MessageBox.Show("coming soon...");
            }
        }
        #endregion
        #region Security

        #region ToggleSwitcher

        private void shake_Checked(object sender, RoutedEventArgs e)
        {
            appSettings["Shaker"] = "On";
            appSettings.Save();
        }

        private void shake_Unchecked(object sender, RoutedEventArgs e)
        {
            appSettings["Shaker"] = "Off";
            appSettings.Save();
        }

        private void passw_Checked(object sender, RoutedEventArgs e)
        {
            appSettings["Passer"] = "On";
        }

        private void passw_Unchecked(object sender, RoutedEventArgs e)
        {
            appSettings["Passer"] = "Off";
        }


        private void PSetter(object sender, TextChangedEventArgs e)
        {

            appSettings["PasswordValue"] = pb.Text;
        }

        #endregion
        #region SettingsUpdater

        public void Settings_Checker()
        {
            if ((string)appSettings["Shaker"] == "On")
            {
                shake.IsChecked = true;
            }

            if ((string)appSettings["Shaker"] == "Off")
            {
                shake.IsChecked = false;
            }

            if ((string)appSettings["Passer"] == "On")
            {
                passw.IsChecked = true;
            }

            if ((string)appSettings["Passer"] == "Off")
            {
                passw.IsChecked = false;
            }
        }
        #endregion
        #endregion
        #region BackButtonPress
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (PasswordAccepter.IsOpen == true)
            {
                e.Cancel = true;
                PasswordAccepter.IsOpen = false;
                ApplicationBar.IsVisible = true;
            }
            if (fileUrl.Text != " ")
            {
                e.Cancel = true;
                StateHelper();
            }
            if (settings.IsOpen == true)
            {
                e.Cancel = true;
                settings.IsOpen = false;
                ApplicationBar.IsVisible = true;
            }
            if (about.IsOpen == true)
            {
                e.Cancel = true;
                about.IsOpen = false;
                settings.IsOpen = true;
            }
            if (personalization.IsOpen == true)
            {
                e.Cancel = true;
                personalization.IsOpen = false;
                settings.IsOpen = true;
            }
            if (security.IsOpen == true)
            {
                e.Cancel = true;
                security.IsOpen = false;
                settings.IsOpen = true;
            }
            else
            {}
        }
        #endregion
    }
}