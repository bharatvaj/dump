using Microsoft.Phone;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Storage;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Live;
using Microsoft.Live.Controls;
using System.IO;
using File360;
using Microsoft.Phone.Shell;

namespace File360
{
    public partial class MainPage : PhoneApplicationPage
    {
        List<sdlist> l = new List<sdlist>();
        List<musiclist> ml = new List<musiclist>();
        ExternalStorageFolder tempFolder;
        IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
        public string btns;
        int winPosition = 1;
        public MediaLibrary lib = new MediaLibrary();
        public AlbumCollection albumslist;
        int varNo = 0;
        Album prevAlbum;
        private LiveConnectClient client;
        private TranslateTransform dT;
        public MainPage()
        {
            InitializeComponent();
            StateHelper();
            List<settinglist> sl = new List<settinglist>();
            #region InitialSettings
            sl.Add(new settinglist("ftp", "configure file transfer protocol", "/Resources/Assets/Settings/ftp.png"));
            sl.Add(new settinglist("bluetooth", "bluetooth settings", "/Resources/Assets/Settings/bluetooth.png"));
            sl.Add(new settinglist("personalization", "get a look", "/Resources/Assets/Settings/personalization.png"));
            sl.Add(new settinglist("security", "protect your files", "/Resources/Assets/Settings/security.png"));
            sl.Add(new settinglist("about", "About Us!", "/Resources/Assets/Settings/about.png"));
            SettingsList.ItemsSource = sl;
            #endregion
            VisualStateManager.GoToState(this, "Normal", false);
            #region AccelerometerReader
            if (!Microsoft.Devices.Sensors.Accelerometer.IsSupported)
            {
                shake.IsEnabled = false;
            }
            #endregion
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
            Settings_Checker();
            Microsoft.Xna.Framework.FrameworkDispatcher.Update();
            MusicDisplay();
            MusicLister();
            PictureGet();
            #region ThemeChecker
            Visibility themeHold = (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"];
            if (themeHold == System.Windows.Visibility.Visible)
            {
                //SystemTray.BackgroundColor = StringToColor("#FFDDDDDD");
                //SystemTray.ForegroundColor = StringToColor("FF000000");
            }
            else
            {
                //SystemTray.BackgroundColor = StringToColor("#FF1F1F1F");
                //SystemTray.ForegroundColor = StringToColor("#FFFFFFFF");
            }
            #endregion
            CreateMenuOne(folderim, "Folder");
            ml.Add(new musiclist("Some song", "some singer"));
            ml.Add(new musiclist("Some song", "some singer"));
            ml.Add(new musiclist("Some song", "some singer"));
            musicLists.ItemsSource = ml;
            settings.IsOpen = false;
            personalization.IsOpen = false;
            about.IsOpen = false;
            security.IsOpen = false;

        }

        #region UsefulTools
        protected static System.Windows.Media.Color StringToColor(string s)
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

            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }
        public Color GetPixel(WriteableBitmap wb,int pixelNo)
        {
            var c = wb.Pixels[pixelNo];
            var a = (byte)(c >> 24);


            // Prevent division by zero
            int ai = a;
            if (ai == 0)
            {
                ai = 1;
            }

            // Scale inverse alpha to use cheap integer mul bit shift
            ai = ((255 << 8) / ai);
            Color theColor = Color.FromArgb(a,
                             (byte)((((c >> 16) & 0xFF) * ai) >> 8),
                             (byte)((((c >> 8) & 0xFF) * ai) >> 8),
                             (byte)((((c & 0xFF) * ai) >> 8)));
            return theColor;
        }
        #endregion
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
                MoveViewWindow(-420);
                winPosition = 1;
                Blurer.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                MoveViewWindow(0);
                winPosition = 0;
                Blurer.Visibility = System.Windows.Visibility.Visible;
                blurer.Begin();
            }
        }
        private void OpenClose_Right(object sender, RoutedEventArgs e)
        {
            var left = Canvas.GetLeft(LayoutRoot);
            if (left > -520)
            {
                MoveViewWindow(-840);
                winPosition = 2;
                Blurer.Visibility = System.Windows.Visibility.Visible;
                blurer.Begin();
                slideRight.Begin();
            }
            else
            {
                MoveViewWindow(-420);
                winPosition = 1;
                Blurer.Visibility = System.Windows.Visibility.Collapsed;
                slideRight.Stop();
               
            }
        }

        void MoveViewWindow(double left)
        {
            _viewMoved = true;
            if (left == -420)
            {
                winPosition = 1;
            }
            else
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
                    slideRight.Stop();
                Blurer.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                //slide to the right
                if (initialPosition< -420)
                    MoveViewWindow(-420);
                slideRight.Stop();
                Blurer.Visibility = System.Windows.Visibility.Collapsed;
                
            }

        }
        #endregion
        #region FileLister
        async public void StateHelper()
        {
            ExternalStorageDevice sdCard = (await ExternalStorage.GetExternalStorageDevicesAsync()).FirstOrDefault();
            if (sdCard != null)
            {
                l.Clear();
                sdlists.ItemsSource = l;
                ExternalStorageFolder sdrootFolder = sdCard.RootFolder;
                tempFolder = sdrootFolder;
                var folder = await sdrootFolder.GetFoldersAsync();
                var files = await sdrootFolder.GetFilesAsync();
                fileHeader.Text = "sdroot";
                foreach (ExternalStorageFolder currentChildFolder in folder)
                {
                    l.Add(new sdlist(currentChildFolder.Name, "/Resources/Assets/Images/folder.png"));
                }
                foreach (ExternalStorageFile currentChildFile in files)
                {
                    if (currentChildFile.Name.EndsWith(".pdf"))
                        l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/pdf.png")); 
                    if (currentChildFile.Name.EndsWith(".mp4"))
                        l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/pdf.png"));
                    if (currentChildFile.Name.EndsWith(".svg"))
                        l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/svg.png"));
                    if (currentChildFile.Name.EndsWith(".mkv"))
                        l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/video.png"));
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
                        AddressHelper(subTemp.Name);
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
                                l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/video.png"));
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
            sdlists.ItemsSource = null;
            sdlists.ItemsSource = l;
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
            PopupPasswordKeyboard();
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
    private void searchBox_MouseEnter(object sender, MouseEventArgs e)
    {
        if (searchBox.IsEnabled == false)
        {
            searchBox.IsEnabled = true;
            expandAnimation.Begin();
            searchBox.Focus();
            searchBox.Text = "";
        }
    }
    private void downloads_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        fileHeader.Text = "downloads";
        AddressHelper("downloads");
        l.Clear();
        sdlists.ItemsSource = null;
        sdlists.ItemsSource = l;
        MoveViewWindow(-420);
        Blurer.Visibility = System.Windows.Visibility.Collapsed;
    }

    private void Settings_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        settings.IsOpen = true;
        settingPopup.Begin();
        PopupHelper.Visibility = System.Windows.Visibility.Visible;
    }

    private void sdcard_menu_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        fileHeader.Text = "sdroot";
        l.Clear();
        StateHelper();
        MoveViewWindow(-420);
        Blurer.Visibility = System.Windows.Visibility.Collapsed;
    }
        
    private void oneFill_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        oneFill.Fill = App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
        Blurer.Visibility = System.Windows.Visibility.Visible;
        MoveViewWindow(-420);
        OneDriveSignIn.IsOpen = true;
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
            sdlists.ItemsSource = null;
            sdlists.ItemsSource = l;
        }
    }
    private void searchButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        
    }

    private void searchBox_LostFocus(object sender, RoutedEventArgs e)
    {
        shrinkAnimation.Begin();
        searchBox.IsEnabled = false;
        searchBox.Text = "search...";
    }
        #endregion
        #region RightMenu
        public void MusicLister()
        {
            musicTileContainer.Children.Clear();
        for(int i=0; i<=5; i++)
        {
            Album album;
            ImageBrush ib = new ImageBrush();
            using (MediaLibrary library = new MediaLibrary())
            {
                if (library != null)
                {
                    try
                    {
                        AlbumCollection albumCollection = library.Albums;
                        album = albumCollection[1];
                        //if (album != null)
                        //{
                        //    while (album.HasArt == true)
                        //    {
                        //        album = albumCollection[i++];
                        //        MessageBox.Show(i.ToString());
                        //    }
                        //}
                        var b = album.GetAlbumArt();
                        WriteableBitmap wbimg = PictureDecoder.DecodeJpeg(b);
                        ib.ImageSource = wbimg;
                    }
                    catch 
                    {
                        MessageBox.Show("Music... Empty!");
                    }
                }
                else 
                {

                }
            }
            Grid grd = new Grid();
            grd.Background = ib;
            Rectangle rct = new Rectangle();
            Rectangle space = new Rectangle();
            space.Height = 140;
            space.Width = 15;
            ImageBrush im = new ImageBrush();
            im.ImageSource = new BitmapImage(new Uri("pack://application:/Resources/Assets/Images/transport.play.png", UriKind.Absolute));
            rct.Height = 65;
            rct.Width = 65;
            rct.OpacityMask = im;
            rct.Fill = (SolidColorBrush)Application.Current.Resources["PhoneForegroundBrush"];
            grd.Width = 100;
            grd.Height = 130;
            grd.ShowGridLines = true;
            grd.Children.Add(rct);
            grd.Tap += grdVideo_Tap;
            musicTileContainer.Children.Add(grd);
            musicTileContainer.Children.Add(space);
            if(i==5)
            {
                TextBlock txb = new TextBlock();
                txb.Text = "more";
                txb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                txb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                txb.Foreground = (SolidColorBrush)Application.Current.Resources["PhoneForegroundBrush"];
                grd.Children.Remove(rct);
                grd.Children.Add(txb);
            }
        }
        }

        void grdVideo_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //MessageBox.Show("Now, the pictures should be listed");
            ringRotate.Begin();
        }
        #endregion
        #region Music

    private void MusicDisplay()
    {
        try
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                var queue = MediaPlayer.Queue;
                AlbumArtCreator(queue.ActiveSong.Album);
                songName.Text = MediaPlayer.Queue.ActiveSong.Name;
                artistName.Text = (MediaPlayer.Queue.ActiveSong.Artist).ToString();
                prevAlbum = MediaPlayer.Queue.ActiveSong.Album;
                textBlock1.Text = "5";
            }
            if (MediaPlayer.State == MediaState.Paused)
            {
                var queue = MediaPlayer.Queue;
                AlbumArtCreator(queue.ActiveSong.Album);
                artistName.Text = (MediaPlayer.Queue.ActiveSong.Artist).ToString();
                prevAlbum = MediaPlayer.Queue.ActiveSong.Album;
                textBlock1.Text = "8";
            }
            if (MediaPlayer.State == MediaState.Stopped)
            {
                songName.Text = "No Media";
                artistName.Text = "tap to select album |>";
                textBlock1.Text = "2";
                ImageBrush im = new ImageBrush();
                im.ImageSource = new BitmapImage(new Uri("pack://application:/Resources/Assets/Images/coverArt.png", UriKind.Absolute));
                musicCoverArt.Fill = im;
            }
        }
        catch (Exception)
        {
            MessageBox.Show("sorry, an unexpected error ocurred");
        }
    }
    private void playState_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
        if (MediaPlayer.State == MediaState.Paused)
        {
            Microsoft.Xna.Framework.FrameworkDispatcher.Update();
            MediaPlayer.Resume();
            MusicDisplay();
        }

        if (MediaPlayer.State == MediaState.Stopped)
        {
            try
            {
                Microsoft.Xna.Framework.FrameworkDispatcher.Update();
                MediaLibrary mLibrary = new MediaLibrary();
                SongCollection songs = mLibrary.Songs;
                MediaPlayer.Play(songs);
                MusicDisplay();
            }
            catch (Exception)
            {
                MessageBox.Show("cannot find music in this phone :(");
            }
        }

        else
        {

            Microsoft.Xna.Framework.FrameworkDispatcher.Update();
            MediaPlayer.Pause();
            MusicDisplay();
        }
    }
    void Drag_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
    {
        if (e.DeltaManipulation.Translation.Y >= 55)
        {
            dragMusic.Begin();
            mlb.IsHitTestVisible = false;
        }
        if (e.DeltaManipulation.Translation.Y <= -25)
        {
            dragMusic.Stop();
            mlb.IsHitTestVisible = true;
        }
        if (e.DeltaManipulation.Translation.X > 0)
        {
            playPrevious.Width = 0;
            playForward.Width += 0.6 * (e.DeltaManipulation.Translation.X);
            if (playForward.Width > 280)
            {
                rectForward.Fill = new SolidColorBrush(StringToColor("#BF140415"));
            }
            else
            {
                rectForward.Fill = new SolidColorBrush(Colors.White);
            }
        }
        if (e.DeltaManipulation.Translation.X < 0)
        {
            playForward.Width = 0;
            playPrevious.Width += -1 * (0.6 * (e.DeltaManipulation.Translation.X));
            if (playPrevious.Width > 280)
            {
                rectPrevious.Fill = new SolidColorBrush(StringToColor("#BF140415"));
            }
            else
            {
                rectPrevious.Fill = new SolidColorBrush(Colors.White);
            }
        }
        
    }

    private void Drag_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
        LayoutRoot.IsHitTestVisible = true;
        if (playForward.Width > 280)
        {
            playForward.Width = 0;
            rectForward.Fill = new SolidColorBrush(Colors.White);
            Microsoft.Xna.Framework.FrameworkDispatcher.Update();
            MediaPlayer.MoveNext();
            MusicDisplay();
        }
            if (playPrevious.Width > 280)
        {
            playPrevious.Width = 0;
            rectPrevious.Fill = new SolidColorBrush(Colors.White);
            Microsoft.Xna.Framework.FrameworkDispatcher.Update();
            MediaPlayer.MovePrevious();
            MusicDisplay();
        }
        else
        {
            playForward.Width = 0;
            playPrevious.Width = 0;

        }
    }
    private void Drag_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
    {
        LayoutRoot.IsHitTestVisible = false;
    }

    private void AlbumArtCreator(Album album)
    {
            if (album.HasArt == true)
            {
                if (prevAlbum != MediaPlayer.Queue.ActiveSong.Album)
                {
                    var b = album.GetAlbumArt();
                    WriteableBitmap wbimg = PictureDecoder.DecodeJpeg(b);
                    RightMediaMenu.Background = new SolidColorBrush(GetPixel(wbimg, 255));
                    mediaBackgroundFade.Begin();
                    ImageBrush imb = new ImageBrush();
                    imb.ImageSource = wbimg;
                    musicCoverArt.Fill = imb;
                    prevAlbum = MediaPlayer.Queue.ActiveSong.Album;
                }
                else 
                {}
            }
            else
            {
                ImageBrush im = new ImageBrush();
                im.ImageSource = new BitmapImage(new Uri("pack://application:/Resources/Assets/Images/coverArt.png", UriKind.Absolute));
                musicCoverArt.Fill = im;
            }
    }
        #endregion
        #region Pictures

        private void PictureGet()
        {
            pac.Children.Clear();
            using (var library = new MediaLibrary())
            {
                PictureAlbumCollection allAlbums = library.RootPictureAlbum.Albums;
                foreach (PictureAlbum pa in allAlbums)
                {
                    ImageBrush im = new ImageBrush();
                    Grid grd = new Grid();
                    Rectangle rec = new Rectangle();
                    try
                    {
                        var b = pa.Pictures.FirstOrDefault().GetThumbnail();
                        WriteableBitmap wbimg = PictureDecoder.DecodeJpeg(b);
                        im.ImageSource = wbimg;
                        rec.Fill = im;
                    }
                    catch 
                    {
                         rec.Fill = (SolidColorBrush)Application.Current.Resources["PhoneChromeBrush"];
                    }
                    varNo++;
                    grd.Tap += grdPictures_Tap;
                    TextBlock txb = new TextBlock();
                    grd.Height = 150;
                    grd.Width = 120;
                    grd.Tag = varNo;
                    rec.Height = 120;
                    rec.Width = 120;
                    rec.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    txb.Text = pa.Name;
                    txb.Foreground = new SolidColorBrush(Colors.White);
                    txb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    txb.TextTrimming = TextTrimming.WordEllipsis;
                    txb.Width = 120;
                    txb.TextAlignment = TextAlignment.Center;
                    txb.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                    txb.FontSize = 18;
                    txb.Name = "txt"+varNo.ToString();
                    grd.Children.Add(rec);
                    grd.Children.Add(txb);
                    Rectangle space = new Rectangle();
                    space.Height = 155;
                    space.Width = 7;
                    pac.Children.Add(grd);
                    pac.Children.Add(space);
                    pac.Height += 30;
                }
            }
        }

        void grdPictures_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Grid ggd = (Grid)sender;
            TextBlock txt = (TextBlock)ggd.FindName("txt"+ggd.Tag.ToString());
            string currentAlbum = txt.Text;
            pictureAlbumName.Text = txt.Text;
            slideInPictureGrid.Begin();
            using (var library = new MediaLibrary())
            {
                PictureAlbumCollection allAlbums = library.RootPictureAlbum.Albums;
                //taking Camera Roll album separately from all album
                PictureAlbum cameraRoll = allAlbums.Where(album => album.Name == currentAlbum).FirstOrDefault();
                // here you will get camera roll picture list
                var CameraRollPictures = cameraRoll.Pictures;
                pictureSubText.Text = CameraRollPictures.Count.ToString();
                ptcf.Height = 0;
                foreach (Picture p in CameraRollPictures)
                {
                    var b = p.GetThumbnail();
                    WriteableBitmap wbimg = PictureDecoder.DecodeJpeg(b);
                    ImageBrush im = new ImageBrush();
                    im.ImageSource = wbimg;
                    Rectangle rct = new Rectangle();
                    rct.Height = 125;
                    rct.Width = 125;
                    rct.Fill = im;
                    Rectangle space = new Rectangle();
                    space.Height = 128;
                    space.Width = 5;
                    rct.Tag = p.Name;
                    rct.Tap += MainPicture_Tap;
                    ptcf.Children.Add(rct);
                    ptcf.Children.Add(space);
                    ptcf.Height += 41.6;
                }
                ptcf.Height += 125;
            }
        }

        void MainPicture_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;
            string currentAlbum = pictureAlbumName.Text;
            using (var library = new MediaLibrary())
            {
                PictureAlbumCollection allAlbums = library.RootPictureAlbum.Albums;
                //taking Camera Roll album separately from all album
                PictureAlbum cameraRoll = allAlbums.Where(album => album.Name == currentAlbum).FirstOrDefault();
                // here you will get camera roll picture list
                var CameraRollPictures = cameraRoll.Pictures;
                var p = CameraRollPictures.First();
                var b = p.GetImage();
                WriteableBitmap wbimg = PictureDecoder.DecodeJpeg(b);
                //ImageBrush im = new ImageBrush();
                //im.ImageSource = wbimg;
                Grid grid = new Grid();
                grid.Height = 768;
                grid.Width = 420;
                Image img = new Image();
                img.Height = 768;
                img.Width = 420;
                img.Source = wbimg;
                grid.Children.Add(img);
                ptc.Children.Add(grid);
            }
        }
        private void pictureBack_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            slideInPictureGrid.Stop();
            ptcf.Children.Clear();
        }
        #endregion
        #region BackButtonPress
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (PasswordAccepter.IsOpen == true)
            {
                e.Cancel = true;
                PasswordAccepter.IsOpen = false;
            }
            //if (fileUrl.Text != " " && winPosition == 1)
            //{
            //    sdlists.ItemsSource = null;
            //    e.Cancel = true;
            //    StateHelper();
            //}
            if (winPosition == 0 && settings.IsOpen == false && about.IsOpen == false && security.IsOpen == false && personalization.IsOpen == false)
            {
                e.Cancel = true;
                MoveViewWindow(-420);
                Blurer.Visibility = System.Windows.Visibility.Collapsed;
                
            }
            if (winPosition == 2)
            {
                e.Cancel = true;
                MoveViewWindow(-420);
                Blurer.Visibility = System.Windows.Visibility.Collapsed;
                slideRight.Stop();
            }
            if(settings.IsOpen == true)
            {
                e.Cancel = true;
                settings.IsOpen = false;
                PopupHelper.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (about.IsOpen == true)
            {
                e.Cancel = true;
                about.IsOpen = false;
                settings.IsOpen = true;
                PopupHelper.Visibility = System.Windows.Visibility.Visible;
                aboutAnimation.Stop();
            }
            if (personalization.IsOpen == true)
            {
                e.Cancel = true;
                personalization.IsOpen = false;
                settings.IsOpen = true;
                PopupHelper.Visibility = System.Windows.Visibility.Visible;
            }
            if (security.IsOpen == true)
            {
                e.Cancel = true;
                security.IsOpen = false;
                settings.IsOpen = true;
                PopupHelper.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                e.Cancel = true;
                DialogBox db = new DialogBox();
                if (canvas.Children.Count < 2)
                {
                    db.HeadingText = "Exit";
                    db.ContentText = "Are you sure you want quit this applicaton?";
                    db.LeftButtonContent = "Exit";
                    db.Height = 800;
                    db.Width = 480;
                    db.LeftButtonHandler += ExitApplication;
                    canvas.Children.Add(db);
                }

            }
        }
        #endregion
        #region Settings

        public async void SettingTap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Grid grd = (Grid)sender;
            TextBlock txt = (TextBlock)grd.FindName("txtB");
            string txts = (txt.Text).ToString();
            if (txts == "about")
            {
                settings.IsOpen = false;
                aboutAnimation.Begin();
                aboutPage.Begin();
            }

            if (txts == "security")
            {
                settings.IsOpen = false;
                securityPage.Begin();
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
                settings.IsOpen = false;
                personalizationPage.Begin();
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
        #region MainPage
        private void Grid_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
        }
        private void Home_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock txt = (TextBlock)sender;
            if (txt.Text == "sd")
            {
                l.Clear();
                sdlists.ItemsSource.Clear();
                StateHelper();
            }
        }
        int col = 0;
        private void header_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (header.Height >= 91 && header.Height <= 518)
                header.Height += e.DeltaManipulation.Translation.Y;
                col += (int)System.Math.Round(e.DeltaManipulation.Translation.Y);
                dragDown.Opacity = 0.002 * col;
        }

        private void header_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            LayoutRoot.IsHitTestVisible = true;
            if (header.Height >= 350)
            {
                headerExpandAnimation.Begin();
                dragDown.Opacity = 1.0;
                sdlists.IsHitTestVisible = false;
                col = 500;
            }
            if (header.Height <= 350)
            {
                headerShrinkAnimation.Begin();
                dragDown.Visibility = System.Windows.Visibility.Collapsed;
                dragDown.Opacity = 0;
                sdlists.IsHitTestVisible = true;
                col = 0;
            }
        }

        private void header_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            dragDown.Visibility = System.Windows.Visibility.Visible;
            LayoutRoot.IsHitTestVisible = false;
        }

        public void AddressHelper(string addressName)
        {
            Rectangle rct = new Rectangle(); 
            ImageBrush im = new ImageBrush();
            im.ImageSource = new BitmapImage(new Uri("pack://application:/Resources/Assets/Images/addressHelper.png", UriKind.Absolute));
            rct.OpacityMask = im;
            rct.Height = 44;
            rct.Width = 22;
            rct.Fill = (SolidColorBrush)App.Current.Resources["PhoneAccentBrush"];
            TextBlock txt = new TextBlock();
            txt.Foreground = (SolidColorBrush)App.Current.Resources["PhoneBackgroundBrush"];
            txt.FontFamily = new FontFamily("Segoe WP Light");
            txt.Height = 44;
            txt.FontSize = 28;
            txt.Text = addressName;
            txt.Tap += Address_Tap;
            addressCont.Children.Add(rct);
            addressCont.Children.Add(txt);
        }

        async void Address_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock txt = (TextBlock)sender;
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
                        AddressHelper(subTemp.Name);
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
                                l.Add(new sdlist(currentChildFile.Name, "/Resources/Assets/File_Types/video.png"));
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

        private void leftMenuItem_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Grid grd = (Grid)sender;
            grd.ManipulationDelta += leftMenuItem_Drag;
            grd.ManipulationCompleted += leftMenuItem_ManipulationCompleted;
            dT = new TranslateTransform();
            grd.RenderTransform = this.dT;
            grd.Background = new SolidColorBrush(StringToColor("#FF105278"));
            SideBar.IsHitTestVisible = false;
        }
        void leftMenuItem_Drag(object sender, ManipulationDeltaEventArgs e)
        {
            dT.Y += e.DeltaManipulation.Translation.Y;
        }

        private void leftMenuItem_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            Grid grd = (Grid)sender;
            grd.Background = new SolidColorBrush(Colors.Purple);
            SideBar.IsHitTestVisible = true;
            grd.ManipulationDelta += grd_ManipulationDelta;
        }
        void grd_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
        }
        private void sdlists_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.DeltaManipulation.Translation.X > 20)
            {
                MoveViewWindow(0);
                winPosition = 0;
                Blurer.Visibility = System.Windows.Visibility.Visible;
                blurer.Begin();
            }
            if (e.DeltaManipulation.Translation.X < -20)
                MoveViewWindow(-840);
            winPosition = 2;
            Blurer.Visibility = System.Windows.Visibility.Visible;
            blurer.Begin();
            slideRight.Begin();
        }
        
        #endregion
        #region Personalisation
        private void SideMenuWallChanger_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WallpaperChanger(new BitmapImage(new Uri("pack://application:/Resources/Assets/Wallpaper/bg1.jpg", UriKind.Absolute)));
        }

        private void Wallpaper_Checked(object sender, RoutedEventArgs e)
        {
            WallpaperTemp.Visibility = Visibility.Visible;
        }

        private void Wallpaper_Unchecked(object sender, RoutedEventArgs e)
        {
            WallpaperTemp.Visibility = Visibility.Collapsed;
            LayoutRoot.Background = new SolidColorBrush(StringToColor("#BF140415"));
        }
        #endregion
        #region Onedrive
        private void loginButton_SessionChanged(object sender, LiveConnectSessionChangedEventArgs e)
        {

            if (e != null && e.Status == LiveConnectSessionStatus.Connected)
            {
                //the session status is connected so we need to set this session status to client
                this.client = new LiveConnectClient(e.Session);
            }
            else
            {
                this.client = null;
            }

        } 
        #endregion
        #region PopupMenu
        private void folderTap_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            holdMenu.Begin();
        }

        private void holdMenuGrid_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            LayoutRoot.IsHitTestVisible = false;
        }

        private void holdMenuGrid_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            LayoutRoot.IsHitTestVisible = true;
        }

        
        #endregion
        #region Menu
        public void CreateMenuOne(ImageBrush image, string menuName)
        {
            StackPanel grd = new StackPanel();
            
            grd.Width = 420;
            grd.Height = 100;
            TextBlock txt = new TextBlock();
            txt.Width = 325;
            Thickness thicks = new Thickness(0, 0, 20, 0);
            txt.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            txt.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            txt.Margin = thicks;
            txt.Text = menuName;
            txt.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            txt.FontSize = 26;
            Rectangle rct = new Rectangle();
            rct.Fill = new SolidColorBrush(Colors.DarkGray);
            rct.OpacityMask = image;
            rct.Height = 100;
            rct.Width = 85;
            Rectangle space = new Rectangle();
            space.Width = 10;
            grd.Children.Add(rct);
            grd.Children.Add(space);
            grd.Children.Add(txt);
            SideBar.Children.Add(grd);
        }
        #endregion
        #region DialogBox
        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings.Save();
            Application.Current.Terminate();
        }
        #endregion

        
    }
}