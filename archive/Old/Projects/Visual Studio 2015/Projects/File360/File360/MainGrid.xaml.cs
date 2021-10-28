using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Activation;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.FileProperties;
using System.Threading.Tasks;

namespace File360
{
    public sealed partial class MainGrid : Page, IFolderPickerContinuable
    {
        #region Variables

        #region SettingsVaribles
        string UserName="User";
        bool GridType = true;
        #endregion
        ApplicationDataContainer settings;
        StorageFolder folder;
        BitmapImage niy = new BitmapImage(new Uri("ms-appx:///Assets/IMG-20150528-WA0003.jpg", UriKind.Absolute));
        ImageSource musicCoverArt = null;
        IReadOnlyList<IStorageItem> Items;
        List<sdlist> sd = new List<sdlist>(); 
        private string mruToken = null;
        IStorageFolder CurrentFolder = null;
        string[] fT = { ".zip",".rar",".mkv",".",".inf",".ini"};
        #endregion
        #region MainMethod
        public MainGrid()
        {
            this.InitializeComponent();
            SideMenu.InitializeDrawerLayout();
            Addresser.InitializeComponent();
            musicCoverArt = niy;
            #region SettingsSerializer
            settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (settings.Values.Count == 0)
            {
                settings.Values["userName"] = "User";
                UserName = (string)settings.Values["userName"];
                settings.Values["itemType"] = "0";
                GridType = ((string)settings.Values["itemType"] == "0");
            }
            else
            {
                UserName = (string)settings.Values["userName"];
                GridType = ((string)settings.Values["itemType"] == "0");
            }
            #endregion
            this.NavigationCacheMode = NavigationCacheMode.Required;
            #region HardwareHandler
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            #endregion

            #region Orientation
            Window.Current.SizeChanged += Current_SizeChanged;
            #endregion
        }
        #region UserManipulation
        private void RemoveFolderLocation(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.CommandParameter != null)
            {
                var folders = App.ViewModel.Folders;
                var tempFolder = folders.First();
                folders.Remove(tempFolder);

            }
        }

        private void Reset()
        {
            App.ViewModel.Folders.Clear();
        }

        private void AddFolderLocation(object sender, RoutedEventArgs e)
        {
            var folder = new Folder();
            folder.Name = "User - " + (App.ViewModel.Folders.Count + 1);
            App.ViewModel.Folders.Add(folder);
        }

      
        #endregion  
        #region OrientationHelpers
        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (IsVertical())
            {
            }
            if (!IsVertical())
            {

            }
        }
        public bool IsVertical()
        {
            if (Window.Current.Bounds.Height > Window.Current.Bounds.Width) return true;
            else return false;
        }
        #endregion
        #endregion
        #region BackButtonHandler
        private async void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            if (SettingsGrid.Visibility == Windows.UI.Xaml.Visibility.Visible)
            {
                SettingsFrame.Content= null;
                SettingsGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                return;
            }
            else if(CurrentFolder != null)
            {
                StorageFolder sf = await StorageFolder.GetFolderFromPathAsync(CurrentFolder.Path.Replace(CurrentFolder.Name,String.Empty));
                GetFilesAndFolder(sf);
                this.Addresser.Children.RemoveAt(this.Addresser.Children.Count - 1);
            }
            else if (this.SideMenu.IsDrawerOpen == true)
            {
                this.SideMenu.CloseDrawer();
                return;
            }
            else if (SDGridView.SelectionMode == ListViewSelectionMode.Multiple)
            {
                scrollBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                headerText.Visibility = Windows.UI.Xaml.Visibility.Visible;
                SDGridView.SelectionMode = ListViewSelectionMode.None;
                SDGridView.IsItemClickEnabled = true;
                return;
            } 
            else if(!this.DialogBox.IsOpen)
            {
                this.DialogBox.IsOpen = true;
                this.DialogBox.LeftButtonHandler += ExitApplicaton;
            }
        }

        #endregion
        #region ExitApplication

        private void ExitApplicaton(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
        #endregion
        #region FileLister
        private async void GetFilesAndFolder(IStorageFolder anyFolder)
        {
            CurrentFolder = anyFolder;
            Items = null;
            sd.Clear();
            SDGridView.ItemsSource = null;
            Items = await anyFolder.GetItemsAsync();
            foreach (IStorageItem Data in Items)
            {
                if (Data.IsOfType(StorageItemTypes.Folder))
                {
                    IStorageFolder Folder;
                    Folder = (IStorageFolder)Data;
                    IReadOnlyList<IStorageItem> item = await Folder.GetItemsAsync();
                    headerText.Text = anyFolder.Name;
                    sd.Add(new sdlist(Folder.Name, "f", item.Count.ToString()));
                }
                if (Data.IsOfType(StorageItemTypes.File))
                {
                    IStorageFile File;
                    File = (IStorageFile)Data;
                    string FileName = File.Name;
                    sd.Add(new sdlist(FileName, "Q"));
                    //string FileType = File.FileType;
                    //#region FileTypes
                    //foreach (string ft in fT)
                    //{
                        //if (ft == FileType)
                        //{
                            //sd.Add(new sdlist(FileName, "Q"));
                        //}
                    //}
                    //#endregion
                    
                }


            }
            SDGridView.ItemsSource = sd;
            //SDGridView.ItemTemplate = GridType ? GridFoldersView : ListFoldersView;
        }
        #endregion
        #region LateralMenu
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if(SideMenu.IsDrawerOpen == false)
            {
                SideMenu.OpenDrawer();
            }
            else
            {
                SideMenu.CloseDrawer();
            }
        }
        #endregion
        #region MusicMenu
        private void MusicMenuClick(object sender, RoutedEventArgs e)
        {
            if (MusicElement.Visibility == Windows.UI.Xaml.Visibility.Visible)
            {
                MusicElement.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                OpenMusic.Content = "u";
                return;
            }
            else if (MusicElement.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
            {
                MusicElement.Visibility = Windows.UI.Xaml.Visibility.Visible;
                OpenMusic.Content = "d";
            }
        }
        #endregion
        #region ListBox
        Object preObj = null;
        Object curObj = null;

        private void SideMenuLeft_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lBox = (ListBox)sender;
            if (preObj != null)
            {
                SideBarMenuButton preSBMB = (SideBarMenuButton)preObj;
                preSBMB.BackgroundColor = null;
            }
            curObj = lBox.SelectedItem;
            preObj = curObj;
            SideBarMenuButton stkChildren = (SideBarMenuButton)curObj;
            stkChildren.BackgroundColor = (SolidColorBrush)Application.Current.Resources["PhoneAccentBrush"];
        }
        #endregion
        #region NavigationHelper
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SuspensionManager.RegisterFrame(SettingsFrame, "SettingsFrame");
            if (SettingsFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the ScenarioList
                if (!SettingsFrame.Navigate(typeof(SettingsPage)))
                {
                    throw new Exception("Failed to create scenario list");
                }
            }
        }
        #endregion
        #region MenuHandlers
        #region SDCard
        private void SDStorageEnumerator(object sender, TappedRoutedEventArgs e)
        {
            if (true)
            {
                FolderPicker folderPicker = new FolderPicker();
                folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
                try
                {
                    SDGridView.ItemsPanel = GridType ? GridViewItemsPanel : ListViewItemsPanel;
                    folderPicker.PickFolderAndContinue();
                }
                catch
                {
                    return;
                }
            }
        }
        #endregion
        #region Phone
        private void PhoneStorageEnumerator(object sender, TappedRoutedEventArgs e)
        {

        }
        #endregion
        #region Pictures
        private async void PicturesLibraryTapped(object sender, TappedRoutedEventArgs e)
        {
             var pA = KnownFolders.PicturesLibrary;
             var pF = await pA.GetFoldersAsync();
            SDGridView.ItemsSource = null;
            sd.Clear();
            if (pF.Count != 0)
            {
                SDGridView.ItemTemplate = PicturesView;
                SDGridView.ItemsPanel = GridViewItemsPanel;
                Items = pF;
                //BitmapImage bm = new BitmapImage(new Uri("ms-appx:///SampleData/SampleDataSource1/SampleDataSource1_Files/image01.png", UriKind.RelativeOrAbsolute));
                //var wb = await WinRTXamlToolkit.Imaging.WriteableBitmapFromBitmapImageExtension.FromBitmapImage(bm);
                //ColorSampler cs = new ColorSampler();
                //WriteableBitmap wd = wb;
                //MusicDock.Background = new SolidColorBrush(cs.GetPixel(wd));
                foreach (var pFo in pF)
                {
                    try
                    {
                        GetThumbnailImageAsync(pFo, ThumbnailMode.PicturesView);
                    }
                    catch (Exception)
                    {
                        //MessageDialog md = new MessageDialog(ex.Message + ex.Source + ex.StackTrace);
                        //md.ShowAsync();
                    }

                }
                SDGridView.ItemsSource = sd;
                MusicView.ItemsSource = sd;
            }
        }
        private void SettingsPage(object sender, TappedRoutedEventArgs e)
        {
            SettingsGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        #endregion
        #region Video
        private async void VideoLibraryTapped(object sender, TappedRoutedEventArgs e)
        {
            var vL = KnownFolders.VideosLibrary;
            var vA = await vL.GetFoldersAsync();
            SDGridView.ItemsSource = null;
            sd.Clear();
            if (vA.Count != 0)
            {
                SDGridView.ItemTemplate = VideosView;
                SDGridView.ItemsPanel = GridViewItemsPanel;
                foreach (var vF in vA)
                {
                    try
                    {
                        GetThumbnailImageAsync(vF, ThumbnailMode.VideosView);
                    }

                    catch (Exception ex)
                    {
                        MessageDialog md = new MessageDialog(ex.Message+ex.Source+ex.StackTrace);
                        md.ShowAsync();
                    }
                }
                SDGridView.ItemsSource = sd;
            }
            else
            {
                return;//The Folder is empty :(
            }
        }
        
        #region Music
        private async void MusicLibraryTapped(object sender, TappedRoutedEventArgs e)
        {
            IReadOnlyList<IStorageItem> mA = KnownFolders.MusicLibrary.GetItemsAsync().GetResults();

            SDGridView.ItemsSource = null;
            sd.Clear();
            if (mA.Count != 0)
            {
                SDGridView.ItemTemplate = MusicsView;
                SDGridView.ItemsPanel = GridViewItemsPanel;
                foreach (var mF in mA)
                {
                    if (mF.IsOfType(StorageItemTypes.Folder))
                    {
                        StorageFolder sf = (StorageFolder)mF;
                        GetThumbnailImageAsync(sf, ThumbnailMode.MusicView);
                    }
                    else
                    {

                    }
                try
                {
                }
                catch (Exception ex)
                {
                    MessageDialog mds = new MessageDialog(ex.Message + ex.Source + ex.StackTrace);
                    mds.ShowAsync();
                }
            }

        }
                SDGridView.ItemsSource = sd;
            //}
            //else
            //{
            //    return;//Add details.
            //}
        }
        public static async Task<bool> IsEmpty(StorageFolder directory)
        {
            var items = await directory.GetItemsAsync();
            return items.Count == 0;
        }
        #endregion
        async void GetThumbnailImageAsync(StorageFolder item, ThumbnailMode mode)
        {
            if (item == null)
                return;

            using (var thumbnail = await item.GetThumbnailAsync(mode, 100))
            {
                if (thumbnail != null && thumbnail.Type == ThumbnailType.Image)
                {
                    BitmapImage bitmap = new BitmapImage();

                    Grid grf;
                    MessageDialog md = new MessageDialog("drg");
                    ImageBrush imgs = new ImageBrush();
                    imgs.Stretch = Stretch.UniformToFill;
                    imgs.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => bitmap.SetSource(thumbnail)).Completed += delegate
                    {
                        ImageBrush img = new ImageBrush();
                        img.ImageSource = bitmap;
                        Grid grd = new Grid();
                        grd.Height = PicHeight;
                        grd.Width = PicWidth;

                        Grid subGrd = new Grid();
                        subGrd.Height = ImageHeight;
                        subGrd.Width = ImageWidth;
                        subGrd.Background = img;

                        TextBlock txt = new TextBlock();
                        txt.VerticalAlignment = VerticalAlignment.Center;
                        txt.HorizontalAlignment = HorizontalAlignment.Center;
                        txt.Text = item.DisplayName;

                        Grid idGrid = new Grid();
                        idGrid.VerticalAlignment = VerticalAlignment.Bottom;
                        idGrid.Background = (SolidColorBrush)App.Current.Resources["PhoneChromeBrush"];
                        idGrid.Children.Add(txt);
                        ObservableCollection<Grid> mediaCollect = new ObservableCollection<Grid>();
                        mediaCollect.Add(grd);
                    };

                    


                    // < Grid Heigth = "{Binding PicHeight}" Width = "{Binding PicWidth}" >
                    //< Grid Height = "{Binding ImageHeight}" Width = "{Binding ImageWidth}" >
                    //       < Grid.Background >
                    //           <  ImageSource = "{Binding Background}" Stretch = "UniformToFill" />
                    //          </ Grid.Background >
                    //          < Grid VerticalAlignment = "Bottom" Background = "{ThemeResource PhoneChromeBrush}" >
                    //                 < TextBlock HorizontalAlignment = "Center" VerticalAlignment = "Center" Text = "{Binding Name}" FontFamily = "Assets/Font/Custom/Lobster 1.4.otf#Lobster 1.4" />
                    //                    </ Grid >
                    //                </ Grid >
                    //            </ Grid >
                }
                else return;
            }
        }
        #endregion
        #endregion

        #region UI
        public double PicHeight
        {
            get
            {
                if (IsVertical()) return Window.Current.Bounds.Height / 3;
                else return Window.Current.Bounds.Width / 3;
            }
        }
        public double PicWidth
        {
            get
            {
                if (IsVertical()) return Window.Current.Bounds.Width / 2;
                else return Window.Current.Bounds.Height / 2;
            }
        }
        public double ImageHeight
        {
            get
            {
                if (IsVertical()) return Window.Current.Bounds.Height / 3.5;
                else return Window.Current.Bounds.Width / 3.5;
            }
        }
        public double ImageWidth
        {
            get
            {
                if (IsVertical()) return Window.Current.Bounds.Width / 2.2;
                else return Window.Current.Bounds.Height / 2.2;
            }
        }
        public double Space
        {
            get { return Window.Current.Bounds.Height / 600; }
        }
        public double GridHeight
        {
            get
            {
                if (IsVertical()) return Window.Current.Bounds.Height / 4;
                else return Window.Current.Bounds.Width / 4;
            }
        }
        public double GridWidth
        {
            get
            {
                if (IsVertical()) return Window.Current.Bounds.Width / 3;
                else return Window.Current.Bounds.Height / 3;
            }
        }
        #endregion

        #region FolderPicker


        public void ContinueFolderPicker(FolderPickerContinuationEventArgs args)
        {
            folder = args.Folder;
            try
            {
                var id = folder.FolderRelativeId;
                if (folder != null)
                {
                    if (folder.IsOfType(StorageItemTypes.Folder))
                    {
                        this.Addresser.Address = "D:";
                    }
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                    mruToken = Windows.Storage.AccessCache.StorageApplicationPermissions.MostRecentlyUsedList.Add(folder);
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
        #endregion
        #region FolderInteraction
        private void FolderHold(object sender, HoldingRoutedEventArgs e)
        {
            if (scrollBar.Visibility == Windows.UI.Xaml.Visibility.Collapsed)
            {
                headerText.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                scrollBar.Visibility = Windows.UI.Xaml.Visibility.Visible;//Start Cool Animations also :)
                SDGridView.SelectionMode = ListViewSelectionMode.Multiple;
                SDGridView.IsItemClickEnabled = false;
                sdlist sdl = (sdlist)SDGridView.SelectedItem;
                SDGridView.SelectedIndex = sd.IndexOf(sdl);
            }
        }
        private async void GridItemClick(object sender, ItemClickEventArgs e)
        {
                sdlist sdl = (sdlist)e.ClickedItem;
                IStorageItem selectedItem = Items.ElementAt(sd.IndexOf(sdl));
                if (selectedItem.IsOfType(StorageItemTypes.Folder))
                {
                    this.Addresser.Address = selectedItem.Name;
                    GetFilesAndFolder((IStorageFolder)selectedItem);
                    return;
                }
                else if (selectedItem.IsOfType(StorageItemTypes.File))
                {
                    MessageDialog mDialog = new MessageDialog("File Selected");
                    await mDialog.ShowAsync();
                }
            }
        #endregion
        #region FileOperations
        private void SelectAll(object sender, TappedRoutedEventArgs e)
        {
            BottomBarItem bbi = (BottomBarItem)sender;
            if (bbi.ContentText == "unselect all")
            {
                bbi.ContentText = "select all";
                scrollBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                headerText.Visibility = Windows.UI.Xaml.Visibility.Visible;
                SDGridView.SelectionMode = ListViewSelectionMode.None;
                SDGridView.IsItemClickEnabled = true;
                return;
            }
            bbi.ContentText = "unselect all";
            SDGridView.SelectAll();
            SDGridView.IsItemClickEnabled = false;
        }
        #endregion
        #region ApplicationSettings
        private void SaveData(string key, string value)
        {
            //Assuming you have the variable declared in App.Xaml.cs.
            //settings.Values[value] = App.UserName;
        }
        // Method to Load data from IsolatedStorageSettings
        private void LoadData(string key, string value)
        {
            //if (settings.Values.Count &lt;= 0) return;
            //{
                //Assuming you to have the variable present in App.Xaml.cs
                //App.UserName = settings.Values.ContainsKey(value)? settings.Values[value] as String : String.Empty;
            //}
        }
        #endregion
        #region SDGridViewSelectionChanged
        private void SDGridViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SDGridView.SelectedIndex == -1)
            {
                scrollBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                headerText.Visibility = Windows.UI.Xaml.Visibility.Visible;
                SDGridView.SelectionMode = ListViewSelectionMode.None;
                SDGridView.IsItemClickEnabled = true;
                return;
            }
            else { return; }
        }
        #endregion
        #region MusicOptions
        private void PlayCurrent(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        private void SortList(object sender, TappedRoutedEventArgs e)
        {
            this.Addresser.SelectedFolder(2);
        }

        private void ListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Addresser.Reset();
            sd.Clear();
            SDGridView.ItemsSource = null;
            headerText.Text = String.Empty;
        }
    }
}
