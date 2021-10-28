using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VideoFX
{
    public sealed partial class RecordPage : Page
    {
        private Windows.Media.Capture.MediaCapture mediaCaptureMgr;
        private bool isRecording = false;
        private Windows.Storage.StorageFile photoStorageFile;
        private Windows.Storage.StorageFile videoStorageFile;
        private readonly String PHOTO_FILE_NAME = "photo.jpg";
        private readonly String VIDEO_FILE_NAME = "video.mp4";
        private bool isUsingFrontCam = false;

        DispatcherTimer photoClean = new DispatcherTimer();


        DeviceInformation frontCam;
        DeviceInformation rearCam;

        MainPage rootFrame = MainPage.Current;
        public RecordPage()
        {
            this.InitializeComponent();
            #region InitializePreview
            StartDevice();
            #endregion
            #region DeviceHandler
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtonBackPressed;
            #endregion
        }

        private void HardwareButtonBackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            ShowStatusMessage("BackPressed");
        }
        internal async void CheckedRecordButton(Object sender, RoutedEventArgs e)
        {
            try
            {
                //videoSurface.Source = null;
                if (!mediaCaptureMgr.MediaCaptureSettings.ConcurrentRecordAndPhotoSupported)
                    {
                    //if camera does not support record and Takephoto at the same time
                    //disable TakePhoto button when recording
                    videoSurface.IsTapEnabled = false;
                    }
                videoSurface.Source = mediaCaptureMgr;
                await StartRecord();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
            }

        }
        private async Task StartRecord()
        {
            try
            {
                //StartPreview();
                ShowStatusMessage("Starting Record");

                videoStorageFile = await Windows.Storage.KnownFolders.VideosLibrary.CreateFileAsync(VIDEO_FILE_NAME, Windows.Storage.CreationCollisionOption.GenerateUniqueName);

                ShowStatusMessage("Create record file successful");

                MediaEncodingProfile recordProfile = null;
                recordProfile = MediaEncodingProfile.CreateMp4(Windows.Media.MediaProperties.VideoEncodingQuality.Auto);
                

                await mediaCaptureMgr.StartRecordToStorageFileAsync(recordProfile, videoStorageFile);
                 //m_bRecording = true;

                ShowStatusMessage("Start Record successful");
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
            }
        }
        protected /*async*/ override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Geolocator locator = new Geolocator();
            //locator.DesiredAccuracyInMeters = 50;

            ////must enable location capability
            //var position = await locator.GetGeopositionAsync();
            //await map.TrySetViewAsync(position.Coordinate.Point, 18D);
        }

        #region InitializePreview
        internal async void StartDevice()
        {
            try
            {
                ShowStatusMessage("Starting device");
                mediaCaptureMgr = new MediaCapture();
                var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                foreach (var device in devices)
                {
                    switch (device.EnclosureLocation.Panel)
                    {
                        case Windows.Devices.Enumeration.Panel.Front:
                            frontCam = device; //frontCamera is of type DeviceInformation
                            //isUsingFrontCam = true;
                            break;
                        case Windows.Devices.Enumeration.Panel.Back:
                            rearCam = device; //rearCamera is of type DeviceInformation
                            break;
                        default:
                            //you can also check for Top, Left, right and Bottom
                            break;
                    }
                }
                await mediaCaptureMgr.InitializeAsync(new MediaCaptureInitializationSettings { VideoDeviceId = rearCam.Id});
                isUsingFrontCam = false;

                if (!string.IsNullOrEmpty(mediaCaptureMgr.MediaCaptureSettings.VideoDeviceId) && !string.IsNullOrEmpty(mediaCaptureMgr.MediaCaptureSettings.AudioDeviceId))
                {

                    ShowStatusMessage("Device initialized successful");

                    mediaCaptureMgr.RecordLimitationExceeded += new Windows.Media.Capture.RecordLimitationExceededEventHandler(RecordLimitationExceeded);
                    mediaCaptureMgr.Failed += new Windows.Media.Capture.MediaCaptureFailedEventHandler(Failed);
                }
                else
                {
                    ShowStatusMessage("No VideoDevice/AudioDevice Found");
                }
                StartPreview();
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
            }
        }
        internal async void StartPreview()
        {
            try
            {
                ShowStatusMessage("Starting preview");
                videoSurface.Source = mediaCaptureMgr;
                await mediaCaptureMgr.StartPreviewAsync();
                //video frames are being pushed to the CaptureElement (instantiated in XAML)

                //if ((mediaCaptureMgr.VideoDeviceController.Brightness != null) && mediaCaptureMgr.VideoDeviceController.Brightness.Capabilities.Supported)
                //{
                //    SetupVideoDeviceControl(m_mediaCaptureMgr.VideoDeviceController.Brightness, sldBrightness);
                //}
                //if ((mediaCaptureMgr.VideoDeviceController.Contrast != null) && mediaCaptureMgr.VideoDeviceController.Contrast.Capabilities.Supported)
                //{
                //    SetupVideoDeviceControl(m_mediaCaptureMgr.VideoDeviceController.Contrast, sldContrast);
                //}
                ShowStatusMessage("Start preview successful");

            }
            catch (Exception exception)
            {
                videoSurface.Source = null;
                ShowExceptionMessage(exception);
            }
        }

        #region SwitchCameras
        private async Task SwitchCameras(object sender, RoutedEventArgs e)
        {
            await mediaCaptureMgr.StopPreviewAsync();
            mediaCaptureMgr.Dispose();
            mediaCaptureMgr = null;
            mediaCaptureMgr = new MediaCapture();

            if (isUsingFrontCam)
            {
                await mediaCaptureMgr.InitializeAsync(new MediaCaptureInitializationSettings { VideoDeviceId = rearCam.Id });

                mediaCaptureMgr.SetRecordRotation(VideoRotation.Clockwise90Degrees);
                mediaCaptureMgr.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
            }
            else
            {
                await mediaCaptureMgr.InitializeAsync(new MediaCaptureInitializationSettings { VideoDeviceId = frontCam.Id });

                mediaCaptureMgr.SetRecordRotation(VideoRotation.Clockwise270Degrees);
                mediaCaptureMgr.SetPreviewRotation(VideoRotation.Clockwise270Degrees);
            }

            isUsingFrontCam = !isUsingFrontCam;

            StartPreview();
        }
        #endregion

        #endregion
        #region Camera Handler
        public async void Failed(Windows.Media.Capture.MediaCapture currentCaptureObject, MediaCaptureFailedEventArgs currentFailure)
        {
            try
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ShowStatusMessage("Fatal error" + currentFailure.Message);
                });
            }
            catch (Exception e)
            {
                ShowExceptionMessage(e);
            }
        }
        public async void RecordLimitationExceeded(Windows.Media.Capture.MediaCapture currentCaptureObject)
        {

            if (isRecording)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        ShowStatusMessage("Stopping Record on exceeding max record duration");
                        await mediaCaptureMgr.StopRecordAsync();
                        isRecording = false;
                        ShowStatusMessage("Stopped record on exceeding max record duration:" + videoStorageFile.Path);

                        if (!mediaCaptureMgr.MediaCaptureSettings.ConcurrentRecordAndPhotoSupported)
                        {
                            //if camera does not support record and Takephoto at the same time
                            //enable TakePhoto button again, after record finished
                        }
                    }
                    catch (Exception e)
                    {
                        ShowExceptionMessage(e);
                    }
                });
            }
        }
        #endregion


        

        internal async void SurfaceTapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                ShowStatusMessage("Taking photo");

                if (!mediaCaptureMgr.MediaCaptureSettings.ConcurrentRecordAndPhotoSupported)
                {
                    //if camera does not support record and Takephoto at the same time
                    //disable Record button when taking photo
                }

                photoStorageFile = await Windows.Storage.KnownFolders.PicturesLibrary.CreateFileAsync(PHOTO_FILE_NAME, Windows.Storage.CreationCollisionOption.GenerateUniqueName);

                ShowStatusMessage("Create photo file successful");
                ImageEncodingProperties imageProperties = ImageEncodingProperties.CreateJpeg();

                await mediaCaptureMgr.CapturePhotoToStorageFileAsync(imageProperties, photoStorageFile);
                ShowStatusMessage("Photo taken");

                //if (!mediaCaptureMgr.MediaCaptureSettings.ConcurrentRecordAndPhotoSupported)
                //{
                //    //if camera does not support record and Takephoto at the same time
                //    //enable Record button after taking photo
                //    btnStartStopRecord1.IsEnabled = true;
                //}

                //var photoStream = await photoStorageFile.OpenAsync(Windows.Storage.FileAccessMode.Read);

                //ShowStatusMessage("File open successful");
                //WriteableBitmap bmpimg;

                //bmpimg.SetSource(photoStream);
                //videoSurface.Source = bmpimg;//videoSurface should be replaced with image
                //ShowStatusMessage("Photo Loaded!");
                //photoClean.Interval = new TimeSpan(0, 0, 2);
                //photoClean.Start();
                //photoClean.Tick += RestorePreview;
                
                ShowStatusMessage(photoStorageFile.Path);

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
            }
        }

        private void RestorePreview(object sender, object e)//after photo display
        {
            //photoClean.Stop();
            ShowStatusMessage(e.GetType().ToString());
        }

        //async IAsyncAction som()
        //{
        //    Task t = new Task(async delegate { await mediaCaptureMgr.StartPreviewAsync(); });
        //    return t;
        //}

        internal void UncheckedRecordButton(object sender, RoutedEventArgs e)
        {

            ShowStatusMessage("Stopping Record");

           // await mediaCaptureMgr.StopRecordAsync().Completed(som, AsyncStatus.Completed); ;

            if (!mediaCaptureMgr.MediaCaptureSettings.ConcurrentRecordAndPhotoSupported)
            {
                //if camera does not support lowlag record and lowlag photo at the same time
                //enable TakePhoto button after recording
                videoSurface.IsTapEnabled = true;
            }

            ShowStatusMessage("Stop record successful");
            //StartPreview();

        }

        #region StatusUpdater
        void ShowStatusMessage(string info)
        {
            rootFrame.Status(info);
        }
        void ShowExceptionMessage(Exception info)
        {
            rootFrame.Status(info.Message);
        }
        #endregion
    }
}
