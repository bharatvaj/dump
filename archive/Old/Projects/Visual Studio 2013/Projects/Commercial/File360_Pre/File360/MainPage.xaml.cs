// Copyright (c) Microsoft. All rights reserved.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Media.Animation;
using System.Collections.ObjectModel;
using Windows.UI.ViewManagement;
using Windows.Storage;
using Windows.Media.Playback;
using System.Threading.Tasks;
using TagLib;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;
using System.IO.Compression;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using System.Linq;
using Windows.Storage.FileProperties;
using Coding4Fun.Toolkit.Controls;
using Windows.ApplicationModel.Activation;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using Windows.Storage.AccessCache;
using Windows.Phone.Devices.Notification;
using System.Runtime.InteropServices;
using Windows.Media;
using Windows.Graphics.Display;

namespace File360
{
    public sealed partial class MainPage : Page, IFolderPickerContinuable
    {



        //[DllImport("KernelBase.dll")]
        //public static extern void KernelDll();
        #region Variables

        public bool GridType = true;
        public ItemsControl musicItemsControlGrid = null;

        #region SettingsVaribles
        ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        StorageFolder folder;
        IReadOnlyList<IStorageItem> Items;

        ObservableCollection<sdlist> sd = new ObservableCollection<sdlist>();

        public IStorageFolder CurrentFolder = null;
        bool InZipRoot = false;

        List<IStorageItem> clipboard = new List<IStorageItem>();

        //bool singleTap = true;

        App app = (App)Application.Current;
        private bool IsMediaOpen = false;

        #region FileOperationVariables
        bool IS_FILE_MOVE = false;
        #endregion

        #region MusicVariables
        SystemMediaTransportControls systemControls;
        DispatcherTimer musicDispatcher = new DispatcherTimer();
        string title = String.Empty;
        string artist = String.Empty;
        #endregion


        #endregion

        public MainPage()
        {
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            this.InitializeComponent();
            app.mainPage = this;
            SDGridView.ItemsSource = sd;
            SideMenu.InitializeDrawerLayout();
            Addresser.InitializeComponent();
            #region MusicPlayerInitialization


            // Hook up app to system transport controls.

            systemControls = SystemMediaTransportControls.GetForCurrentView();

            systemControls.ButtonPressed += async (sender, args) =>
            {
                switch (args.Button)

                {

                    case SystemMediaTransportControlsButton.Play:

                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>

                        {
                            mediaPlayer.Play();

                        });

                        break;

                    case SystemMediaTransportControlsButton.Pause:
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>

   {

       mediaPlayer.Pause();

   });

                        break;

                    default:

                        break;

                }
            };

            // Register to handle the following system transpot control buttons.

            systemControls.IsPlayEnabled = true;

            systemControls.IsPauseEnabled = true;
            #endregion
            #region SettingsSerializer
            if (settings.Values.Count == 0)
            {
                settings.Values["userName"] = "User";
                app.UserName = (string)settings.Values["userName"];
                settings.Values["itemType"] = 1;
                GridType = (int)settings.Values["itemType"] == 1;

                settings.Values["musicPlayer"] = "1";
                settings.Values["picturesPlayer"] = "1";
                settings.Values["videoPlayer"] = "1";
                settings.Values["ebookViewer"] = "1";
                settings.Values["musThumbnail"] = "0";
                settings.Values["vidThumbnail"] = "0";
                settings.Values["picThumbnail"] = "0";

                settings.Values["passkey"] = String.Empty;
            }
            else
            {
                app.UserName = (string)settings.Values["userName"];
                GridType = (int)settings.Values["itemType"] == 1;
            }
            ChangeTemplate();
            #endregion
            #region CriticalStartup
            HideStatusBar();
            Loaded += MainPageLoaded;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            Window.Current.SizeChanged += Current_SizeChanged;
            #endregion
        }


        #region FileOperationsNotifier

        private void PasteBarSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PasteFlipView.SelectedIndex == 0)
                IS_FILE_MOVE = false;
            else IS_FILE_MOVE = true;
        }
        #endregion

        #region MainEvents
        private void MainPageLoaded(object sender, RoutedEventArgs e)
        {
            GetSDRoot();
            LoadMemoryData();
            //SettingsTab.Height = Window.Current.Bounds.Width * 2 / 3;
            PasteFlipView.SelectionChanged += PasteBarSelectionChanged;
            SubFrame.ContentTransitions = new TransitionCollection { new PaneThemeTransition { Edge = EdgeTransitionLocation.Bottom } };
            #region MediaPlayerDispatcherInitialization
            musicDispatcher.Interval = TimeSpan.FromMilliseconds(1000);

            mediaPlayer.MediaFailed += async (s, t) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
                {
                    UpdateMusic("no media", "", TimeSpan.FromSeconds(0));
                    musicSlider.Value = 0;
                    playPause.Content = "5";
                    currentDuration.Text = "--:--";
                    mediaDuration.Text = "--:--";
                    if (musicDispatcher.IsEnabled)
                        musicDispatcher.Stop();
                });
            };

            mediaPlayer.MediaEnded += async (s, t) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
                {
                    UpdateMusic("no media", "", mediaPlayer.NaturalDuration.TimeSpan);
                    musicSlider.Value = 0;
                    playPause.Content = "5";
                    currentDuration.Text = "--:--";
                    mediaDuration.Text = "--:--";
                    musicDispatcher.Stop();
                });
            };
            musicDispatcher.Tick += (s, t) =>
            {
                musicSlider.Value = mediaPlayer.Position.TotalSeconds;
                currentDuration.Text = mediaPlayer.Position.Minutes + ":" + mediaPlayer.Position.Seconds;

            };
            #endregion

        }
        private async void HideStatusBar()
        {
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();

            // Hide the status bar
            await statusBar.HideAsync();
        }
        public void OpenDialog()
        {
            if (!this.DialogBox.IsOpen)
            {
                this.DialogBox.IsOpen = true;
                this.DialogBox.LeftButtonHandler += ExitApplicaton;
            }
        }
        #region Navigaion Methods
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }
        #endregion

        #region BackButtonHandler
        public async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            if (PictureDisplayer.Visibility == Visibility.Visible)
            {
                PictureFlipView.Items.Clear();
                PictureDisplayer.Visibility = Visibility.Collapsed;
            }
            else if (FileMoveDialog.Visibility == Visibility.Visible)
            {
                CloseDialog(2);
            }
            else if (SubFrameGrid.Visibility == Visibility.Visible)
            {
                CloseDialog(0);
            }
            else if (IsMediaOpen)
            {
                MediaOpenRev.Begin();
                IsMediaOpen = false;
            }

            else if (SortMenu.Visibility == Visibility.Visible)
            {
                SortMenu.Visibility = Visibility.Collapsed;
            }
            else if (this.SideMenu.IsDrawerOpen == true)
            {
                this.SideMenu.CloseDrawer();
            }
            else if (SDGridView.SelectionMode == ListViewSelectionMode.Multiple)
            {
                MultipleSelection(false);
            }
            else if (InZipRoot)
            {
                GetFilesAndFolder(CurrentFolder);
                InZipRoot = false;
            }
            else if (!this.Addresser.Root)
            {
                try
                {
                    int curFoldCharNm = CurrentFolder.Path.LastIndexOf(CurrentFolder.Name);
                    StorageFolder sf = await StorageFolder.GetFolderFromPathAsync(CurrentFolder.Path.Remove(curFoldCharNm));
                    this.Addresser.RemoveLast();
                    GetFilesAndFolder(sf);
                }
                catch { }
            }
            else
            {
                OpenDialog();
            }
        }
        #endregion

        #endregion
        #region Music
        #region MusicEvents
        //private void MusicSegmentChanged(object sender, RangeBaseValueChangedEventArgs e)
        //{
        //    //    if (!(mediaPlayer.CurrentState == MediaPlayerState.Closed))
        //    //    {
        //    //        if (mediaPlayer.CanSeek)
        //    //        {
        //    //            mediaPlayer.Position = TimeSpan.FromSeconds(e.NewValue);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        musicSlider.Value = 0;
        //    //    }
        //}
        internal async void PlayMedia(IStorageFile selectedItem)
        {

            mediaPlayer.Source = new Uri(selectedItem.Path);

            mediaPlayer.MediaOpened += async (s, t) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, delegate
                {
                    musicSlider.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    playPause.Content = "8";
                    UpdateMusic(title, artist, mediaPlayer.NaturalDuration.TimeSpan);
                    musicDispatcher.Start();
                });
            };
            mediaPlayer.Play();

            #region TagLib AlbumArt
            Tag tags = null;
            var fileStream = await selectedItem.OpenStreamForReadAsync();
            try
            {
                var tagFile = TagLib.File.Create(new StreamFileAbstraction(selectedItem.Name, fileStream, fileStream));
                if (selectedItem.FileType == ".mp3" || selectedItem.FileType == ".wma")
                    tags = tagFile.GetTag(TagTypes.Id3v2);
                else if (selectedItem.FileType == ".m4a")
                    tags = tagFile.GetTag(TagLib.TagTypes.MovieId);
                try
                {
                    title = tags.Title;
                }
                catch (NullReferenceException)
                {
                    title = selectedItem.Name;
                }
                try
                {
                    artist = String.Concat(tags.Performers);
                }
                catch (NullReferenceException)
                {
                    artist = String.Empty;
                }
                if (tags.Pictures != null)
                {
                    try
                    {
                        MemoryStream ms = new MemoryStream(tags.Pictures[0].Data.Data);
                        WriteableBitmap wm = null;
                        var wwmV = wm.FromStream(ms, Windows.Graphics.Imaging.BitmapPixelFormat.Unknown);
                        await wwmV.ContinueWith(delegate
                        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                            {
                                ms.Dispose();
                                ImageBrush im = new ImageBrush();
                                im.ImageSource = wwmV.Result;
                                im.Stretch = Stretch.UniformToFill;
                                musicArt.Visibility = Visibility.Collapsed;
                                imageContainerMedia.Background = im;
                            });
                        });
                    }

                    catch (IndexOutOfRangeException)
                    {
                        imageContainerMedia.Background = (SolidColorBrush)App.Current.Resources["PhoneChromeBrush"];
                        musicArt.Visibility = Visibility.Visible;
                    }
                    catch (UnauthorizedAccessException)
                    {
                    }
                    #endregion
                }
                else
                {
                    imageContainerMedia.Background = (SolidColorBrush)App.Current.Resources["PhoneChromeBrush"];
                    musicArt.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                imageContainerMedia.Background = (SolidColorBrush)App.Current.Resources["PhoneChromeBrush"];
                musicArt.Visibility = Visibility.Visible;
            }
        }

        private void UpdateMusic(string songName, string songArtist, TimeSpan duration)
        {
            try
            {
                SongName.Text = songName;
                SongArtist.Text = songArtist;
                mediaDuration.Text = duration.Minutes + ":" + duration.Seconds;
            }
            catch
            {
                SongName.Text = "no media";
                SongArtist.Text = "";
                mediaDuration.Text = "--:--";
            }
        }
        #endregion
        #region MusicMenu
        public void MusicMenuClick()
        {
            if (IsMediaOpen)
            {
                MediaOpenRev.Begin();
                IsMediaOpen = false;
                //OpenMusic.Content = "u";
                return;
            }
            else if (!IsMediaOpen)
            {
                //BitmapImage bm = new BitmapImage(new Uri("ms-appx:///SampleData/SampleDataSource1/SampleDataSource1_Files/image01.png", UriKind.RelativeOrAbsolute));
                //var wb = await WinRTXamlToolkit.Imaging.WriteableBitmapFromBitmapImageExtension.FromBitmapImage(bm);
                //ColorSampler cs = new ColorSampler();
                //WriteableBitmap wd = wb;
                //MusicDock.Background = new SolidColorBrush(cs.GetPixel(wd));
                MediaOpen.Begin();
                IsMediaOpen = true;
                //OpenMusic.Content = "d";
                return;
            }
        }

        private void PlayCurrent(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.CurrentState == MediaElementState.Playing)
            {
                mediaPlayer.Pause();
                playPause.Content = "5";
                DurationBlink.Begin();
            }
            else if (mediaPlayer.CurrentState == MediaElementState.Paused)
            {
                mediaPlayer.Play();
                playPause.Content = "8";
                DurationBlink.Stop();
            }

            else if (mediaPlayer.CurrentState == MediaElementState.Stopped)
            {
                KnownFolders.MusicLibrary.GetFilesAsync().Completed += (es, st) =>
                {
                    mediaPlayer.Source = new Uri(es.GetResults().First().Path);
                    mediaPlayer.Play();
                    playPause.Content = "8";
                };

            }

        }
        #endregion
        #endregion

        #region MediaMaipulations
        private void mediaFragManipDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.X > 0)
                MediaFragment.RenderTransform = new CompositeTransform { TranslateX = e.Cumulative.Translation.X };
            else if (e.Delta.Translation.X < -25)
            {
                MediaOpen.Begin();
                IsMediaOpen = true;
            }
        }

        private void mediaFragManipCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var transform = (CompositeTransform)MediaFragment.RenderTransform;
            if (transform == null) return;
            var up = transform.TranslateX;

            var snapLimit = ActualHeight / 1.1;

            // Get init position of _listFragment
            var initialPosition = ActualHeight;

            // If current left coordinate is smaller than snap limit, close drawer
            if (Math.Abs(initialPosition - up) < snapLimit)
            {
                MediaOpenRev.Begin();
                IsMediaOpen = false;
            }
            else
            {
                MediaOpen.Begin();
                IsMediaOpen = true;
            }
        }
        #endregion

        #region NotifyHelpers
        public void NotifyUser(string info)
        {
            statusMsg.Text = info;
        }

        public void AI(string header, string msg)
        {
            aiText.Text = header;
            aiSubText.Text = msg;
            aiBorder.Visibility = Visibility.Visible;
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 4);
            dt.Tick += delegate
            {
                aiBorder.Visibility = Visibility.Collapsed;
                dt.Stop();
            };
            dt.Start();
        }
        #endregion
        #region Pictures


        public void DisplayPhoto(IStorageFile selectedItem)
        {
            FlipViewItem fvi = new FlipViewItem();
            ScrollViewer scv = new ScrollViewer();
            Image im = new Image();
            BitmapImage bi = new BitmapImage(new Uri(selectedItem.Path));
            im.Source = bi;
            scv.Content = im;
            fvi.Content = scv;
            PictureFlipView.Items.Add(fvi);
            PictureDisplayer.Visibility = Visibility.Visible;
        }

        #endregion


        #region MainGrid
        public void CloseDrawer()
        {
            SideMenu.CloseDrawer();
        }



        private void ChangeTemplate()
        {
            if (!GridType)
            {
                SDGridView.ItemsPanel = (ItemsPanelTemplate)Resources["ListViewItemsPanel"];
                SDGridView.ItemTemplate = (DataTemplate)Resources["ListFoldersView"];
                gridList.ContentText = "list";
                gridList.ImageText = "b";
                settings.Values["itemType"] = 0;
                GridType = false;
            }
            else
            {
                SDGridView.ItemsPanel = (ItemsPanelTemplate)Resources["GridViewItemsPanel"];
                SDGridView.ItemTemplate = (DataTemplate)Resources["GridFoldersView"];
                gridList.ContentText = "grid";
                gridList.ImageText = "N";
                settings.Values["itemType"] = 1;
                GridType = true;
            }
        }

        private async void GetSDRoot()
        {

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("FolderTokenSettingsKey"))
            {
                string token = (string)ApplicationData.Current.LocalSettings.Values["FolderTokenSettingsKey"];
                // if we do, use it to get the StorageFolder instance
                folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
                this.Addresser.Reset();
                try
                {
                    GetFilesAndFolder(folder);
                    RootFolder.Text = "3";
                }
                catch (Exception ex)
                {
                    ShowStatus(ex.Message);
                    //AI("no sdcard is detected", 2);
                }
            }
        }


        #region UserFolderCommunication


        private void Reset()
        {
            sd.Clear();
        }


        #endregion
        #region OrientationHelpers
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            //var b = Window.Current.Bounds;
            VisualStateManager.GoToState(this, "Portrait", false);
        }
        public bool IsVertical()
        {
            if (Window.Current.Bounds.Height > Window.Current.Bounds.Width) return true;
            else return false;
        }
        #endregion
        #endregion



        #region ExitApplication

        private void ExitApplicaton(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
        #endregion
        #region FileLister
        public async void GetFilesAndFolder(IStorageFolder anyFolder)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async delegate
            {
                ShowStatus(anyFolder.Path);
                if (anyFolder != null)
                {
                    sd.Clear();
                    CurrentFolder = anyFolder;
                    Items = await anyFolder.GetItemsAsync();
                    if (Items.Count == 0)
                    {
                        CurrentFolder = anyFolder;
                        headerText.Text = anyFolder.Name;
                        switch (anyFolder.Path)
                        {
                            case @"D:\Documents":
                                //display documents missing sad face
                                break;
                            case @"D:\Music":
                                //display music missing sad face
                                break;
                            case @"D:\Videos":
                                //display videos missing sad face
                                break;
                            case @"D:\Pictures":
                                //display pictures missing sad face
                                break;
                            default:
                                EmptyFolderStack.Visibility = Visibility.Visible;
                                //default options for other empty folders
                                break;

                        }
                        return;
                    }
                    else
                    {
                        EmptyFolderStack.Visibility = Visibility.Collapsed;
                        foreach (IStorageItem Data in Items)
                        {
                            if (Data.IsOfType(StorageItemTypes.Folder))
                            {
                                new sdlist("Folder", "f");
                                IStorageFolder Folder;
                                Folder = (IStorageFolder)Data;
                                headerText.Text = anyFolder.Name;
                                if (GridType)
                                    SDGridView.ItemsPanel = (ItemsPanelTemplate)Resources["GridViewItemsPanel"];
                                else SDGridView.ItemsPanel = (ItemsPanelTemplate)Resources["ListViewItemsPanel"];
                                if (CurrentFolder.Path == @"D:\" || CurrentFolder.Path == @"C:\")
                                {
                                    if (Folder.Name == "Videos") sd.Add(new sdlist(Folder.Name, "o", ""));
                                    else if (Folder.Name == "Pictures") sd.Add(new sdlist(Folder.Name, "f", ""));
                                    else if (Folder.Name == "Music") sd.Add(new sdlist(Folder.Name, "g", ""));
                                    else if (Folder.Name == "Downloads") sd.Add(new sdlist(Folder.Name, "e", ""));
                                    else if (Folder.Name == "Documents") sd.Add(new sdlist(Folder.Name, "n", ""));
                                    else sd.Add(new sdlist(Folder.Name, "h", ""));
                                }
                                else sd.Add(new sdlist(Folder.Name, "h", ""));
                            }
                            if (Data.IsOfType(StorageItemTypes.File))
                            {
                                IStorageFile File = (IStorageFile)Data;
                                string fn = File.Name;
                                string fi = File.FileType;
                                #region FileTypes
                                if (fi == ".mp3")
                                {
                                    if (((string)settings.Values["musThumbnail"] == "1"))
                                    {

                                        #region Mp3

                                        //var fileStream = await File.OpenStreamForReadAsync();
                                        //var tagFile = TagLib.File.Create(new StreamFileAbstraction(File.Name, fileStream, fileStream));
                                        //var tags = tagFile.GetTag(TagTypes.Id3v2);
                                        //try
                                        //{
                                        //    if (tags.Pictures != null)
                                        //    {
                                        //        MemoryStream ms = new MemoryStream(tags.Pictures[0].Data.Data);
                                        //        WriteableBitmap wm = null;
                                        //        WriteableBitmap wwm = await wm.FromStream(ms, Windows.Graphics.Imaging.BitmapPixelFormat.Unknown);

                                        //        ms.Dispose();
                                        //        ImageBrush im = new ImageBrush();
                                        //        im.ImageSource = wwm;
                                        //        im.Stretch = Stretch.UniformToFill;
                                        //        sd.Add(new sdlist(fn, im, ""));
                                        //        #endregion
                                        //    }
                                        //}
                                        //catch (IndexOutOfRangeException ex)
                                        //{
                                        //    BitmapImage bm = new BitmapImage(new Uri("ms-appx:///Assets/IMG-20150528-WA0003.jpg", UriKind.Absolute));
                                        //    ImageBrush im = new ImageBrush();
                                        //    im.ImageSource = bm;
                                        //    sd.Add(new sdlist(fn, im, ""));
                                        //}
                                        //catch (Exception ex)
                                        //{
                                        //    AI(ex.Message, 2);
                                        //}
                                        #endregion
                                    }
                                    else sd.Add(new sdlist(fn, "m"));

                                }
                                else if (fi == ".wma")
                                {
                                    if (((string)settings.Values["musThumbnail"] == "1"))
                                    { }
                                    #region wma-work in progress

                                    //var fileStream = await File.OpenStreamForReadAsync();
                                    //var tagFile = TagLib.File.Create(new StreamFileAbstraction(File.Name, fileStream, fileStream));
                                    //var tags = tagFile.GetTag(TagTypes.Id3v2);
                                    //try
                                    //{
                                    //    if (tags.Pictures != null)
                                    //    {
                                    //        MemoryStream ms = new MemoryStream(tags.Pictures[0].Data.Data);
                                    //        WriteableBitmap wm = null;
                                    //        WriteableBitmap wwm = await wm.FromStream(ms, Windows.Graphics.Imaging.BitmapPixelFormat.Unknown);

                                    //        ms.Dispose();
                                    //        ImageBrush im = new ImageBrush();
                                    //        im.ImageSource = wwm;
                                    //        im.Stretch = Stretch.UniformToFill;
                                    //        sd.Add(new sdlist(fn, im, ""));
                                    //        #endregion
                                    //    }
                                    //}
                                    //catch (IndexOutOfRangeException ex)
                                    //{
                                    //    BitmapImage bm = new BitmapImage(new Uri("ms-appx:///Assets/IMG-20150528-WA0003.jpg", UriKind.Absolute));
                                    //    ImageBrush im = new ImageBrush();
                                    //    im.ImageSource = bm;
                                    //    sd.Add(new sdlist(fn, im, ""));
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    AI(ex.Message, 2);
                                    //}
                                    #endregion
                                    else sd.Add(new sdlist(fn, "m"));

                                }
                                else if (fi == ".docx")
                                {
                                    sd.Add(new sdlist(fn, "k"));

                                }
                                else if (fi == ".png")
                                {
                                    if (((string)settings.Values["picThumbnail"] == "1"))
                                        await GetThumbnailImageAsync((StorageFile)File, ThumbnailMode.ListView);
                                    else sd.Add(new sdlist(fn, "p"));
                                }
                                else if (fi == ".jpg")
                                {
                                    if (((string)settings.Values["picThumbnail"] == "1"))
                                        await GetThumbnailImageAsync((StorageFile)File, ThumbnailMode.ListView);
                                    else sd.Add(new sdlist(fn, "p"));
                                }
                                else if (fi == ".mp4")
                                {
                                    if (((string)settings.Values["vidThumbnail"] == "1"))
                                    {
                                        try
                                        {
                                            await GetThumbnailImageAsync((StorageFile)File, ThumbnailMode.ListView);
                                        }
                                        catch
                                        {
                                            sd.Add(new sdlist(fn, "v"));
                                        }
                                    }
                                    else sd.Add(new sdlist(fn, "v"));
                                }
                                else if (fi == ".mov")
                                {
                                    if (((string)settings.Values["vidThumbnail"] == "1"))
                                    {
                                        try
                                        {
                                            await GetThumbnailImageAsync((StorageFile)File, ThumbnailMode.ListView);
                                        }
                                        catch
                                        {
                                            sd.Add(new sdlist(fn, "v"));
                                        }
                                    }
                                    else sd.Add(new sdlist(fn, "v"));
                                }
                                else if (fi == ".zip")
                                {
                                    sd.Add(new sdlist(fn, "z"));

                                }
                                else if (fi == ".cs")
                                {
                                    sd.Add(new sdlist(fn, "c"));
                                }
                                else if (fi == ".pdf")
                                {
                                    sd.Add(new sdlist(fn, "b"));
                                }
                                else if (fi == ".vcf")
                                {
                                    sd.Add(new sdlist(fn, "l"));
                                }
                                else if (fi == ".doc")
                                {
                                    sd.Add(new sdlist(fn, "k"));
                                }
                                else if (fi == ".xlx")
                                {
                                    sd.Add(new sdlist(fn, "j"));
                                }
                                else if (fi == ".xlsx")
                                {
                                    sd.Add(new sdlist(fn, "j"));
                                }
                                else if (fi == ".7z")
                                {
                                    sd.Add(new sdlist(fn, "7"));
                                }
                                else if (fi == ".xml")
                                {
                                    sd.Add(new sdlist(fn, "x"));
                                }
                                else if (fi == ".txt")
                                {
                                    sd.Add(new sdlist(fn, "a"));
                                }
                                else if (fi == ".rar")
                                {
                                    sd.Add(new sdlist(fn, "r"));
                                }
                                else
                                {
                                    sd.Add(new sdlist(fn, "i"));

                                }
                                #endregion
                            }
                        }
                    }
                }
                //else AI("This folder contains no items", 2);
            });
        }

        #endregion
        #region LateralMenu
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (SideMenu.IsDrawerOpen == false)
            {
                SideMenu.OpenDrawer();
            }
            else
            {
                SideMenu.CloseDrawer();
            }
        }
        #endregion
        #region MenuHandlers
        #region DeviceStorageEnumerators
        private async void SDCardEnumerator(object sender, RoutedEventArgs e)
        {
            StorageFolder folder;
            // check if we already have a token
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("SDCardKey"))
            {
                string token = (string)ApplicationData.Current.LocalSettings.Values["SDCardKey"];
                // if we do, use it to get the StorageFolder instance
                folder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
                this.Addresser.Reset();
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, delegate
                {
                    headerText.Text = "SD Card";
                    RootFolder.Text = "3";
                    SideMenu.CloseDrawer();
                });
                GetFilesAndFolder(folder);
            }
            else
            {
                // if we don't, this is the first time the code is being executed; user has to give
                // us a consent to use the folder
                FolderPicker folderPicker = new FolderPicker();
                folderPicker.FileTypeFilter.Add("*");
                folderPicker.PickFolderAndContinue();
            }
        }


        private void PhoneEnumerator(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.PickFolderAndContinue();
        }
        #endregion



        #endregion
        #region ImageHelpers
        public async Task GetThumbnailImageAsync(StorageFolder item, ThumbnailMode mode)
        {
            if (item == null)
                return;
            using (var thumbnail = await item.GetThumbnailAsync(mode))
            {
                if (thumbnail != null /*&& thumbnail.Type == ThumbnailType.Image && thumbnail.Type == ThumbnailType.Icon*/)
                {
                    sd.Add(new sdlist(item.DisplayName, thumbnail, "2"));
                    return;
                }
            }
        }

        private async Task GetThumbnailImageAsync(StorageFile item, ThumbnailMode mode)
        {
            if (item == null)
                return;
            using (var thumbnail = await item.GetThumbnailAsync(mode))
            {
                if (thumbnail != null)
                {
                    sd.Add(new sdlist(item.DisplayName, thumbnail, ""));
                    return;
                }
            }
        }
        #endregion


        #region FolderPicker


        public void ContinueFolderPicker(FolderPickerContinuationEventArgs args)
        {
            folder = args.Folder;
            if (folder != null)
            {
                try
                {
                    var id = folder.FolderRelativeId;
                    if (folder != null)
                    {
                        if (folder.IsOfType(StorageItemTypes.Folder))
                        {
                            this.Addresser.Reset();
                        }
                        // after that, we can store the folder for future reuse
                        string pickedFolderToken = StorageApplicationPermissions.FutureAccessList.Add(folder);
                        ApplicationData.Current.LocalSettings.Values.Add("FolderTokenSettingsKey", pickedFolderToken);
                        StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                        string mruToken = Windows.Storage.AccessCache.StorageApplicationPermissions.MostRecentlyUsedList.Add(folder);
                        headerText.Text = "SD Card";
                        RootFolder.Text = "3";
                        if (GridType)
                        {
                            SDGridView.ItemsPanel = (ItemsPanelTemplate)Resources["GridViewItemsPanel"];
                            SDGridView.ItemTemplate = (DataTemplate)Resources["GridFoldersView"];
                        }
                        else
                        {
                            SDGridView.ItemsPanel = (ItemsPanelTemplate)Resources["ListViewItemsPanel"];
                            SDGridView.ItemTemplate = (DataTemplate)Resources["ListFoldersView"];
                        }
                        GetFilesAndFolder(folder);
                    }
                    else
                    {
                        return;
                    }
                }
                catch
                { return; }
            }
        }
        #endregion
        #region FileExecution
        private async void ExecuteFile(IStorageFile selectedItem)
        {

            if (selectedItem.FileType == ".zip")
            {
                OpenZip(selectedItem);
            }
            else if (selectedItem.FileType == ".mp3")
            {
                if ((string)settings.Values["musicPlayer"] == "1")
                    PlayMedia(selectedItem);
                else await Windows.System.Launcher.LaunchFileAsync(selectedItem);
            }
            else if (selectedItem.FileType == ".wma")
            {
                if ((string)settings.Values["musicPlayer"] == "1")
                    PlayMedia(selectedItem);
                else await Windows.System.Launcher.LaunchFileAsync(selectedItem);
            }
            else if (selectedItem.FileType == ".m4a")
            {
                if ((string)settings.Values["musicPlayer"] == "1")
                    PlayMedia(selectedItem);
                else await Windows.System.Launcher.LaunchFileAsync(selectedItem);
            }
            else if (selectedItem.FileType == ".mp4")
            {
                if ((string)settings.Values["videosPlayer"] == "1")
                {
                    PlayVideo(selectedItem);
                }
                else await Windows.System.Launcher.LaunchFileAsync(selectedItem);
            }
            else if (selectedItem.FileType == ".mov")
            {
                if ((string)settings.Values["videosPlayer"] == "1")
                {
                    PlayVideo(selectedItem);
                }
                else await Windows.System.Launcher.LaunchFileAsync(selectedItem);
            }
            else if (selectedItem.FileType == ".jpg")
            {
                if ((string)settings.Values["picturesPlayer"] == "1")
                {
                    DisplayPhoto(selectedItem);
                }
                else await Windows.System.Launcher.LaunchFileAsync(selectedItem);
            }
            else
            {
                await Windows.System.Launcher.LaunchFileAsync(selectedItem);
            }
        }

        private void PlayVideo(IStorageFile selectedItem)
        {
            this.Frame.Navigate(typeof(VideoPage), selectedItem);
        }

        #endregion
        #region ZipFileHelpers
        public async void OpenZip(IStorageFile compressedFile)
        {
            using (ZipArchive archive = new ZipArchive(await compressedFile.OpenStreamForReadAsync()))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    //if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    entry.ExtractToFile(Path.Combine(extractPath, entry.FullName));
                    //}
                    if (entry.Name.Contains('.'))
                        sd.Add(new sdlist(entry.Name, "Q"));
                    else sd.Add(new sdlist(entry.Name, "f"));
                }
            }



            //try
            //{
            //    using (MemoryStream zipMemoryStream = new MemoryStream(WindowsRuntimeBufferExtensions.ToArray(await FileIO.ReadBufferAsync(compressedFile))))
            //    {
            //        using (ZipArchive zipArchive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Read))
            //        {
            //            var files = zipArchive.Entries;
            //            foreach (var file in files)
            //            {
            //                try
            //                {
            //                    var uncompressedFol = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(file.Name, CreationCollisionOption.ReplaceExisting);
            //                }
            //                catch
            //                {
            //                    var uncompressedFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(file.Name, CreationCollisionOption.ReplaceExisting);
            //                }

            //                return ApplicationData.Current.TemporaryFolder;
            //            }
            //            return null;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ShowStatus(ex.Message);
            //    return null;
            //}
        }


        public async Task<StorageFile> CreateZip(StorageFile file)
        {
            if (CurrentFolder != null)
            {
                try
                {
                    using (MemoryStream zipMemoryStream = new MemoryStream())
                    {
                        using (ZipArchive zipArchive = new System.IO.Compression.ZipArchive(zipMemoryStream, ZipArchiveMode.Create))
                        {
                            try
                            {
                                byte[] buffer = WindowsRuntimeBufferExtensions.ToArray(await FileIO.ReadBufferAsync(file));
                                ZipArchiveEntry entry = zipArchive.CreateEntry(file.Name);
                                using (Stream entryStream = entry.Open())
                                {
                                    await entryStream.WriteAsync(buffer, 0, buffer.Length);
                                }
                                GetFilesAndFolder(CurrentFolder);
                            }
                            catch (Exception ex)
                            {
                                ShowStatus(ex.Message);
                            }
                        }

                        // Created new file to store compressed files
                        var compressedFileName = file.Name + ".zip";
                        StorageFile zipFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(compressedFileName, CreationCollisionOption.GenerateUniqueName);
                        using (IRandomAccessStream zipStream = await zipFile.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            // Write compressed data from memory to file
                            using (Stream outstream = zipStream.AsStreamForWrite())
                            {
                                byte[] buffer = zipMemoryStream.ToArray();
                                outstream.Write(buffer, 0, buffer.Length);
                                outstream.Flush();
                                return zipFile;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowStatus(ex.Message);
                    return null;
                }
            }
            else
            {
                ShowStatus("Error! This is not a valid folder");
                return null;
            }
        }
        #endregion
        #region BottomBarmenuEventHandlers
        private void GridListChange(object sender, RoutedEventArgs e)
        {
            if (GridType)
            {
                SDGridView.ItemsPanel = (ItemsPanelTemplate)Resources["ListViewItemsPanel"];
                SDGridView.ItemTemplate = (DataTemplate)Resources["ListFoldersView"];
                gridList.ContentText = "list";
                gridList.ImageText = "b";
                GridType = false;
            }
            else
            {
                SDGridView.ItemsPanel = (ItemsPanelTemplate)Resources["GridViewItemsPanel"];
                SDGridView.ItemTemplate = (DataTemplate)Resources["GridFoldersView"];
                gridList.ContentText = "grid";
                gridList.ImageText = "N";
                GridType = true;
            }
        }
        private void SelectAll(object sender, RoutedEventArgs e)
        {
            MultipleSelection(true);
            SDGridView.SelectAll();
            //folderHold.Begin();
        }
        private void selectBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MultipleSelection(false);
        }
        #endregion



        #region FileSortingMethods

        private void OpenSortMenu(object sender, RoutedEventArgs e)
        {
            OpenSortMenuAnim.Begin();
        }
        private void SortMenuItemClick(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            string buttonName = bt.Content.ToString();
            if (buttonName == "Name")
            {
                Sort(0);
            }
            else if (buttonName == "Type")
            {
                Sort(1);
            }
            else if (buttonName == "Size")
            {
                Sort(2);
            }
            else if (buttonName == "Date")
            {
                Sort(3);
            }
            SortMenu.Visibility = Visibility.Collapsed;
        }

        private void ToggleMenu(Panel element)
        {
            if (element.Visibility == Visibility.Visible)
                element.Visibility = Visibility.Collapsed;
            else element.Visibility = Visibility.Visible;
        }


        public void Sort(int sortType)
        {
            try
            {
                switch (sortType)
                {
                    case 0:
                        sd.OrderBy(o => o.Name);
                        break;
                    case 1:
                        sd.OrderBy(o => o.Image);
                        break;
                    case 2:
                        sd.OrderBy(o => o.Name);
                        break;
                    case 3:
                        sd.OrderBy(o => o.Name);
                        break;
                }
            }
            catch (ArgumentNullException)
            {

            }
        }

        #endregion

        private Task LoadMemoryData()
        {
            IStorageItem sf = ApplicationData.Current.LocalFolder;

            var properties = sf.GetBasicPropertiesAsync().Completed += (es, t) =>
            {
                es.GetResults().RetrievePropertiesAsync(new[] { "System.FreeSpace" }).Completed += async (ex, tx) =>
                {
                    var freeSpace = ex.GetResults()["System.FreeSpace"];
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        float freeSpaceGB = (float)Double.Parse((freeSpace.ToString())) / 1000000000;
                        FreeSpace.Text = Math.Round(freeSpaceGB, 2).ToString();
                        sdProgbar.Maximum = 16.00;
                        sdProgbar.Value = 16.00 - freeSpaceGB;
                    });
                };
            };

            return null;
        }

        private void AddFile(object sender, RoutedEventArgs e)
        {
            CurrentFolder.CreateFileAsync("New File", CreationCollisionOption.GenerateUniqueName).Completed += async (es, t) =>
               {
                   await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
                   {
                       sd.Add(new sdlist("New File", "Q"));
                   });
               };
        }
        private void AddZip(object sender, RoutedEventArgs e)
        {
            InputPrompt ip = new InputPrompt();
            ip.Completed += (async (ex, t) =>
            {
                StorageFile sf = await CurrentFolder.CreateFileAsync(t.Result + ".zip", CreationCollisionOption.GenerateUniqueName);
                if (sf != null && CreateZip(sf).IsCompleted)
                {

                    GetFilesAndFolder(CurrentFolder);
                }
            });
            ip.Title = "Enter file name";
            ip.Show();
        }

        private async void AddFolder(object sender, RoutedEventArgs e)
        {
            sd.Add(new sdlist("New Folder", "f"));
            StorageFolder sf = await CurrentFolder.CreateFolderAsync("New Folder", CreationCollisionOption.GenerateUniqueName);
        }
        private async void DeleteItem(object sender, RoutedEventArgs e)
        {


            foreach (var item in SDGridView.SelectedItems)
            {
                try
                {

                    sdlist sdli = (sdlist)item;
                    IStorageItem selectedFile = Items.ElementAt(sd.IndexOf(sdli));
                    await selectedFile.DeleteAsync();
                    sd.Remove(sdli);
                }
                catch (Exception ex)
                {
                    AI("Cannot Delete File", "Cannot delete file because the file may be locked");
                    ShowStatus(ex.Message);
                }
            }
            //GetFilesAndFolder(CurrentFolder);
        }

        private async void RenameItem(object sender, RoutedEventArgs e)
        {

            foreach (var item in SDGridView.SelectedItems)
            {
                try
                {
                    sdlist sdli = (sdlist)item;
                    IStorageItem selectedFile = Items.ElementAt(sd.IndexOf(sdli));
                    await selectedFile.RenameAsync("Renamed Folder", NameCollisionOption.GenerateUniqueName);
                    sdli.Name = "New Folder";
                }
                catch (Exception ex)
                {
                    AI("Cannot Rename File", "Cannot rename file because the file may be locked");
                    ShowStatus(ex.Message);
                }
            }

            //WinRTXamlToolkit.Controls.InputDialog id = new WinRTXamlToolkit.Controls.InputDialog();

            //id.GotFocus += (s, ex) =>
            //{
            //    try
            //    {
            //        id.Select(0, 3).Text.LastIndexOf('.'));
            //    }
            //    catch
            //    {
            //        txtBoxRe.SelectAll();
            //    }
            //};
            //id.AcceptButton = "Rename";
            //id.CancelButton = "Cancel";

            //id.KeyDown += async (s, ex) =>
            //{
            //    if (ex.Key == Windows.System.VirtualKey.Enter)
            //    {
            //        if (txtBoxRe.Text == String.Empty)
            //        {
            //            txtBoxRe.Visibility = Visibility.Collapsed;
            //            txtBlockRe.Visibility = Visibility.Visible;
            //        }
            //        else if (txtBoxRe.Text.Contains("/") || txtBoxRe.Text.Contains(@"\"))
            //        {
            //            AI("Cannot rename File or Folder", @"A file or folder can't contain any of the following characters: \ / : * ? " + "\"" + " < > |");
            //        }
            //        else
            //        {
            //            try
            //            {
            //                txtBlockRe.Text = txtBoxRe.Text;
            //                txtBoxRe.Visibility = Visibility.Collapsed;
            //                txtBlockRe.Visibility = Visibility.Visible;

            //            }
            //            catch
            //            {
            //                AI("Rename operation failed", "Cannot rename this file at the moment");
            //            }
            //            GetFilesAndFolder(CurrentFolder);
            //        }
            //    }
            //};

            //var stk = itemControl.GetLogicalChildrenByType<TextBlock>(true);
            //var textBlock = stk.ElementAt(1);
            //var sdl = await CurrentFolder.GetItemsAsync();
            //int index = SDGridView.Items.IndexOf(sdli);

        }

        private void CutItems(object sender, RoutedEventArgs e)
        {
            clipboard.Clear();
            var sdl = SDGridView.SelectedItems;
            foreach (var sdli in sdl)
            {
                try
                {
                    var si = (sdlist)sdli;
                    IStorageItem selectedItem = Items.ElementAt(sd.IndexOf(si));
                    clipboard.Add(selectedItem);
                }
                catch (Exception ex)
                {
                    ShowStatus(ex.Message);
                    AI("Cannot add to clipboard", ex.Message);
                }

            }
        }

        #region ShowStatus
        public void ShowStatus(string info)
        {
            NotifyUser(info);
        }
        #endregion

        #region Paste
        private void PasteItem(object sender, RoutedEventArgs e)
        {
            OpenDialog(2);
        }
        private void OverwriteContents(object sender, RoutedEventArgs e)
        {
            FileMoveDialog.Visibility = Visibility.Collapsed;
            ProgInfoView.Visibility = Visibility.Visible;
            int clipCount = 0;
            if ((clipCount = clipboard.Count) != 0)
            {
                progFileDest.Text = CurrentFolder.Name;
                progTotalSize.Text = clipCount.ToString();
                if (IS_FILE_MOVE)//move
                {
                    cmstatus.Text = "moving";
                    try
                    {
                        foreach (var item in clipboard)
                        {
                            progFileSource.Text = item.Name;
                            progCurrentSize.Text = clipboard.IndexOf(item).ToString();
                            if (item.IsOfType(StorageItemTypes.File))
                            {
                                var tb = ((IStorageFile)item).MoveAsync(CurrentFolder, item.Name, NameCollisionOption.ReplaceExisting);
                                tb.Completed += async (i, s) =>
                                {
                                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
                                    {
                                        ProgInfoView.Visibility = Visibility.Collapsed;
                                        ProgressDialog.Visibility = Visibility.Collapsed;
                                    });
                                };
                            }
                        }
                        GetFilesAndFolder(CurrentFolder);
                    }
                    catch (Exception ex)
                    {
                        AI("Error copying file", ex.Message);
                    }
                }
                else//copy
                {
                    cmstatus.Text = "copying";
                    try
                    {
                        foreach (var item in clipboard)
                        {
                            progFileSource.Text = item.Name;
                            progCurrentSize.Text = clipboard.IndexOf(item).ToString();
                            if (item.IsOfType(StorageItemTypes.File))
                            {
                                var tb = ((IStorageFile)item).CopyAsync(CurrentFolder, item.Name, NameCollisionOption.ReplaceExisting);
                                tb.Completed += async (i, s) =>
                                {
                                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
                                    {
                                        ProgInfoView.Visibility = Visibility.Collapsed;
                                        ProgressDialog.Visibility = Visibility.Collapsed;
                                    });
                                };
                            }
                        }
                        GetFilesAndFolder(CurrentFolder);
                    }
                    catch (Exception ex)
                    {
                        AI("Error copying file", ex.Message);
                    }
                }
            }
        }

        private void KeepBothFiles(object sender, RoutedEventArgs e)
        {
            FileMoveDialog.Visibility = Visibility.Collapsed;
            ProgInfoView.Visibility = Visibility.Visible;
            int clipCount = 0;
            if ((clipCount = clipboard.Count) != 0)
            {
                progFileDest.Text = CurrentFolder.Name;
                progTotalSize.Text = clipCount.ToString();
                if (IS_FILE_MOVE)//move
                {
                    cmstatus.Text = "moving";
                    try
                    {
                        foreach (var item in clipboard)
                        {
                            progFileSource.Text = item.Name;
                            progCurrentSize.Text = clipboard.IndexOf(item).ToString();
                            if (item.IsOfType(StorageItemTypes.File))
                            {
                                var tb = ((IStorageFile)item).MoveAsync(CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                                tb.Completed += async (i, s) =>
                                {
                                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
                                    {
                                        ProgInfoView.Visibility = Visibility.Collapsed;
                                        ProgressDialog.Visibility = Visibility.Collapsed;
                                    });
                                };
                            }
                        }
                        GetFilesAndFolder(CurrentFolder);
                    }
                    catch (Exception ex)
                    {
                        AI("Error copying file", ex.Message);
                    }
                }
                else//copy
                {
                    cmstatus.Text = "copying";
                    try
                    {
                        foreach (var item in clipboard)
                        {
                            progFileSource.Text = item.Name;
                            progCurrentSize.Text = clipboard.IndexOf(item).ToString();
                            if (item.IsOfType(StorageItemTypes.File))
                            {
                                var tb = ((IStorageFile)item).CopyAsync(CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                                tb.Completed += async (i, s) =>
                                {
                                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, delegate
                                    {
                                        ProgInfoView.Visibility = Visibility.Collapsed;
                                        ProgressDialog.Visibility = Visibility.Collapsed;
                                    });
                                };
                            }
                        }
                        GetFilesAndFolder(CurrentFolder);
                    }
                    catch (Exception ex)
                    {
                        AI("Error copying file", ex.Message);
                    }
                }
            }
        }
        #endregion
        #region FileShare
        private void ShareContent(object sender, RoutedEventArgs e)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(ShareStorageItemsHandler);
            // If the user clicks the share button, invoke the share flow programatically.
            DataTransferManager.ShowShareUI();
        }

        private void ShareStorageItemsHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = "File360";
            request.Data.Properties.Description = "files & folders";

            // Because we are making async calls in the DataRequested event handler,
            // we need to get the deferral first.
            DataRequestDeferral deferral = request.GetDeferral();
            List<IStorageItem> selectedFiles = null;

            // Make sure we always call Complete on the deferral.
            try
            {


                var sdl = SDGridView.SelectedItems;
                foreach (var sdli in sdl)
                {
                    try
                    {
                        var si = (sdlist)sdli;
                        selectedFiles.Add(Items.ElementAt(sd.IndexOf(si)));
                    }
                    catch (Exception ex)
                    {
                        ShowStatus(ex.Message);
                    }
                }

                request.Data.SetStorageItems(selectedFiles);
            }
            finally
            {
                deferral.Complete();
            }
        }
        #endregion

        private async void DeleteThisFolder(object sender, RoutedEventArgs e)
        {
            //int index = CurrentFolder.Path.LastIndexOf(CurrentFolder.Name);
            var sf = await StorageFolder.GetFolderFromPathAsync(CurrentFolder.Path);
            var td = CurrentFolder.DeleteAsync();
            td.Completed += (es,t)=>
            {
                GetFilesAndFolder(sf);
            };
        }

        private void PersonalStorageEnumerator(object sender, RoutedEventArgs e)
        {
            PersonalStorageEnumerator();
        }

        public async void PersonalStorageEnumerator()
        {
            var HomeFolderP = ApplicationData.Current.LocalFolder;
            var HomeFolder = await HomeFolderP.CreateFolderAsync("Home", CreationCollisionOption.OpenIfExists);
            GetFilesAndFolder(HomeFolder);
            this.Addresser.Reset();
            headerText.Text = "Personal Storage";
            RootFolder.Text = "|";
            SideMenu.CloseDrawer();
        }

        private void NowPlaying(object sender, RoutedEventArgs e)
        {
            MusicMenuClick();
        }

        private async void PreviousFolder(object sender, RoutedEventArgs e)
        {
            if (!this.Addresser.Root)
            {
                try
                {
                    int curFoldCharNm = CurrentFolder.Path.LastIndexOf(CurrentFolder.Name);
                    StorageFolder sf = await StorageFolder.GetFolderFromPathAsync(CurrentFolder.Path.Remove(curFoldCharNm));
                    this.Addresser.RemoveLast();
                    GetFilesAndFolder(sf);
                }
                catch { }
            }
            else
            {
                SideMenu.OpenDrawer();
            }
        }

        #endregion
        


        //#region Bluetooth
        //private async void OpenBluetoothSettings(object sender, RoutedEventArgs e)
        //{
        //    await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
        //}
        //#endregion

        #region HelpMenu
        private void HelpOpen(object sender, RoutedEventArgs e)
        {
            OpenDialog(0);
        }

        private void HelpPageClose(object sender, RoutedEventArgs e)
        {
            CloseDialog(0);
        }
        #endregion


        #region FTP
        private void OpenFTP(object sender, RoutedEventArgs e)
        {
            FTPOpen.Begin();
            FTPActive.Begin();
        }
        #endregion
        #region Music
        private void musicToggle_Checked(object sender, RoutedEventArgs e)
        {
            settings.Values["musicPlayer"] = "1";
        }
        private void musicToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            settings.Values["musicPlayer"] = "0";
        }

        private void musThumbnailToggle_Checked(object sender, RoutedEventArgs e)
        {
            settings.Values["musThumbnail"] = "1";
        }

        private void musThumbnailToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            settings.Values["musThumbnail"] = "0";
        }
        #endregion
        #region Videos
        private void videosToggle_Checked(object sender, RoutedEventArgs e)
        {
            settings.Values["videosPlayer"] = "1";
        }
        private void videosToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            settings.Values["videosPlayer"] = "0";
        }
        private void vidThumbnailToggle_Checked(object sender, RoutedEventArgs e)
        {
            settings.Values["vidThumbnail"] = "1";
        }

        private void vidThumbnailToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            settings.Values["vidThumbnail"] = "0";
        }

        #endregion
        #region Ebooks
        private void ebookToggle_Checked(object sender, RoutedEventArgs e)
        {
            settings.Values["ebookViewer"] = "1";
        }
        private void ebookToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            settings.Values["ebookViewer"] = "0";
        }
        #endregion
        #region Pictures
        private void picturesToggle_Checked(object sender, RoutedEventArgs e)
        {
            settings.Values["picturesPlayer"] = "1";
        }

        private void picturesToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            settings.Values["picturesPlayer"] = "0";
        }

        private void picThumbnailToggle_Checked(object sender, RoutedEventArgs e)
        {
            settings.Values["picThumbnail"] = "1";
        }

        private void picThumbnailToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            settings.Values["picThumbnail"] = "0";
        }
        #endregion

        #region DialogOpenClose
        public void OpenDialog(int dialogNo)
        {
            ProgressDialog.Visibility = Visibility.Visible;
            switch (dialogNo)
            {
                case 0:
                    SubFrameGrid.Visibility = Visibility.Visible;
                    SubFrame.Navigate(typeof(HelpPage));
                    SubFrameHeader.Text = "HELP";
                    break;

                case 1:
                    SubFrameGrid.Visibility = Visibility.Visible;
                    SubFrame.Navigate(typeof(VaultPage));
                    SubFrameHeader.Text = "VAULT";
                    break;
                case 2:
                    FileMoveDialog.Visibility = Visibility.Visible;
                    break;
                case 3:
                    ProgInfoView.Visibility = Visibility.Visible;
                    break;

            }
        }


        public void CloseDialog(int dialogNo)
        {
            ProgressDialog.Visibility = Visibility.Collapsed;
            switch (dialogNo)
            {
                case 0:
                    SubFrameGrid.Visibility = Visibility.Collapsed;
                    break;

                case 1:
                    SubFrameGrid.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    FileMoveDialog.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    ProgInfoView.Visibility = Visibility.Collapsed;
                    break;

            }
        }
        #endregion

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            CloseDialog(2);
        }

        private void StopFTP(object sender, RoutedEventArgs e)
        {
            FTPOpen.Stop();
            FTPActive.Stop();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var list = sd.Where(x => x.Name.Contains(searchBox.Text));
                sd.Clear();
                if (list.Count() == 0) return;
                foreach (var item in list)
                {
                    sd.Add(item);
                }
                //foreach (var sdsi in sds)
                //{
                //    sd.Add(sdsi);
                //}
            }
            catch (Exception ex)
            {
                ShowStatus(ex.Message);
                sd.Clear();
            }
        }

        #region AudioMenuHandler
        private void RepeatButtonChecked(object sender, RoutedEventArgs e)
        {
            mediaPlayer.IsLooping = true;
        }

        private void RepeatButtonUnchecked(object sender, RoutedEventArgs e)
        {
            mediaPlayer.IsLooping = false;
        }
        #endregion

        private void PlayNext(object sender, RoutedEventArgs e)
        {

        }

        private void GridItemClick(object sender, ItemClickEventArgs e)
        {
            int index = SDGridView.Items.IndexOf((sdlist)e.ClickedItem);


            ShowStatus(index.ToString());
            IStorageItem selectedItem = Items.ElementAt(index);
            if (selectedItem.IsOfType(StorageItemTypes.Folder))
            {
                Addresser.Address(selectedItem.Name, selectedItem.Path);
                GetFilesAndFolder((IStorageFolder)selectedItem);
                return;
            }
            else if (selectedItem.IsOfType(StorageItemTypes.File))
            {
                ExecuteFile((IStorageFile)selectedItem);
                return;
            }
        }

        public void MultipleSelection(bool con)
        {
            switch (con)
            {
                case true:
                    SDGridView.SelectionMode = ListViewSelectionMode.Multiple;
                    SDGridView.ItemClick -= GridItemClick;
                    SDGridView.IsItemClickEnabled = false;
                    operationBar.Visibility = Visibility.Visible;
                    operationBarNormal.Visibility = Visibility.Collapsed;
                    break;

                case false:
                    SDGridView.SelectionMode = ListViewSelectionMode.None;
                    SDGridView.ItemClick += GridItemClick;
                    SDGridView.IsItemClickEnabled = true;
                    operationBar.Visibility = Visibility.Collapsed;
                    operationBarNormal.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ItemHolding(object sender, HoldingRoutedEventArgs e)
        {
            MultipleSelection(true);
        }

        private void SDListChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SDGridView.SelectedIndex == -1)
            {
                MultipleSelection(false);
            }
        }

        private void CloseSetting(object sender, RoutedEventArgs e)
        {
            OpenSettings.Stop();
        }

        private void FocusSearch(object sender, RoutedEventArgs e)
        {
            searchBox.Visibility = Visibility.Visible;
            searchBox.Focus(FocusState.Programmatic);
        }

        private void OpenMusicSetting(object sender, RoutedEventArgs e)
        {
            ShowSetting();
        }

        private void OpenPictureSetting(object sender, RoutedEventArgs e)
        {
            ShowSetting();
        }

        private void OpenVideoSetting(object sender, RoutedEventArgs e)
        {
            ShowSetting();
        }

        private void ShowSetting()
        {
            OpenSettings.Begin();
        }

        private void OpenSecuritySetting(object sender, RoutedEventArgs e)
        {
            OpenDialog(1);
        }

        private void ListRecents(object sender, RoutedEventArgs e)
        {
            SideMenu.CloseDrawer();
        }

        private void searchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            searchBox.Visibility = Visibility.Collapsed;
        }
    }
}