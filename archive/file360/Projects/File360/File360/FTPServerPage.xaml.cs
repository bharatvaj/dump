using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Storage;
using Windows.System;
using System.IO.IsolatedStorage;
using WinPhoneFtp.FtpService;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.UI.Core;

namespace File360
{
    public partial class FTPServerPage : PhoneApplicationPage
    {
        public FTPServerPage()
        {
            InitializeComponent();
        }

        FtpClient ftpClient = null;
        Logger logger = null;


        async private void Connect_Click(object sender, RoutedEventArgs e)
        {
            logger = Logger.GetDefault(this.Dispatcher);
            lstLogs.ItemsSource = logger.Logs;
            ftpClient = new FtpClient(txtIp.Text, this.Dispatcher);
            //ftpClient = new PqaFtpClient(txtIp.Text);
            ftpClient.FtpConnected += ftpClient_FtpConnected;
            ftpClient.FtpDisconnected += ftpClient_FtpDisconnected;
            ftpClient.FtpAuthenticationSucceeded += ftpClient_FtpAuthenticationSucceeded;
            ftpClient.FtpAuthenticationFailed += ftpClient_FtpAuthenticationFailed;
            ftpClient.FtpDirectoryChangedFailed += ftpClient_FtpDirectoryChangedFailed;
            ftpClient.FtpDirectoryChangedSucceded += ftpClient_FtpDirectoryChangedSucceded;
            ftpClient.FtpPresentWorkingDirectoryReceived += ftpClient_FtpPresentWorkingDirectoryReceived;
            ftpClient.FtpFileUploadSucceeded += ftpClient_FtpFileUploadSucceeded;
            ftpClient.FtpFileDownloadSucceeded += ftpClient_FtpFileDownloadSucceeded;
            ftpClient.FtpFileUploadFailed += ftpClient_FtpFileUploadFailed;
            ftpClient.FtpFileDownloadFailed += ftpClient_FtpFileDownloadFailed;
            ftpClient.FtpDirectoryListed += ftpClient_FtpDirectoryListed;
            ftpClient.FtpFileTransferProgressed += ftpClient_FtpFileTransferProgressed;
            await ftpClient.ConnectAsync();
        }

        void ftpClient_FtpDisconnected(object sender, FtpDisconnectedEventArgs e)
        {
            logger.AddLog(String.Format("FTP Disconnected. Reason: {0}", e.DisconnectReason));
        }

        void ftpClient_FtpFileTransferProgressed(object sender, FtpFileTransferProgressedEventArgs e)
        {
            logger.AddLog(String.Format("File {0}, Data Transferred: {1}", e.IsUpload ? "Upload" : "Download", e.BytesTransferred));
        }

        void ftpClient_FtpDirectoryListed(object sender, FtpDirectoryListedEventArgs e)
        {
            logger.AddLog("Directory Listing");

            foreach (String filename in e.GetFilenames())
            {
                logger.AddLog("file: " + filename);
            }

            foreach (String dir in e.GetDirectories())
            {
                logger.AddLog("directory: " + dir);
            }
        }

        async private void Command_Click(object sender, RoutedEventArgs e)
        {
            if (txtCmd.Text.StartsWith("STOR"))
            {
                logger.Logs.Clear();
                String Filename = txtCmd.Text.Split(new char[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
                StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(txtCmd.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                await ftpClient.UploadFileAsync(await file.OpenStreamForReadAsync(), "123.wmv");
                return;
            }

            if (txtCmd.Text.StartsWith("RETR"))
            {
                logger.Logs.Clear();
                String Filename = txtCmd.Text.Split(new char[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
                StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(txtCmd.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1], CreationCollisionOption.OpenIfExists);
                await ftpClient.DownloadFileAsync(await file.OpenStreamForWriteAsync(), Filename);
                return;
            }

            if (txtCmd.Text.Equals("PWD"))
            {
                logger.Logs.Clear();
                await ftpClient.GetPresentWorkingDirectoryAsync();
                return;
            }

            if (txtCmd.Text.Equals("LIST"))
            {
                logger.Logs.Clear();
                await ftpClient.GetDirectoryListingAsync();
                return;
            }

            if (txtCmd.Text.StartsWith("CWD"))
            {
                logger.Logs.Clear();
                await ftpClient.ChangeWorkingDirectoryAsync(txtCmd.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                return;
            }

            if (txtCmd.Text.Equals("QUIT"))
            {
                logger.Logs.Clear();
                await ftpClient.DisconnectAsync();
                return;
            }
        }

        void ftpClient_FtpFileDownloadFailed(object sender, FtpFileTransferFailedEventArgs e)
        {
            //e.LocalFileStream.Close();
            //e.LocalFileStream.Dispose();
            logger.AddLog("download failed");
        }

        void ftpClient_FtpFileUploadFailed(object sender, FtpFileTransferFailedEventArgs e)
        {
            //e.LocalFileStream.Close();
            //e.LocalFileStream.Dispose();
            logger.AddLog("upload failed");
        }

        void ftpClient_FtpFileDownloadSucceeded(object sender, FtpFileTransferEventArgs e)
        {
            //e.LocalFileStream.Close();
            //e.LocalFileStream.Dispose();
            logger.AddLog("download done");
        }

        void ftpClient_FtpFileUploadSucceeded(object sender, FtpFileTransferEventArgs e)
        {
            //e.LocalFileStream.Close();
            //e.LocalFileStream.Dispose();
            logger.AddLog("upload done");
        }

        async void ftpClient_FtpConnected(object sender, EventArgs e)
        {
            await (sender as FtpClient).AuthenticateAsync("admin","admin");
        }

        void ftpClient_FtpPresentWorkingDirectoryReceived(object sender, FtpPresentWorkingDirectoryEventArgs e)
        {
            logger.AddLog(String.Format("Present working directory is: {0}", e.PresentWorkingDirectory));
        }

        void ftpClient_FtpDirectoryChangedSucceded(object sender, FtpDirectoryChangedEventArgs e)
        {
            logger.AddLog(String.Format("Directory {0} change succeeded", e.RemoteDirectory));
        }

        void ftpClient_FtpDirectoryChangedFailed(object sender, FtpDirectoryChangedEventArgs e)
        {
            logger.AddLog("Directory change failed");
        }

        void ftpClient_FtpAuthenticationFailed(object sender, EventArgs e)
        {
            logger.AddLog("Authentication failed");
        }

        void ftpClient_FtpAuthenticationSucceeded(object sender, EventArgs e)
        {
            logger.AddLog("Authentication succeeded");
        }
    }
}