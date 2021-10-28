using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows;
using Windows.Storage;
using System.Text;
using Windows.Storage.Streams;
using System.Windows.Threading;

namespace WinPhoneFtp.FtpService
{
    public sealed class Logger
    {
        private static Logger s_Logger = null;
        public static Logger GetDefault(Dispatcher UIDispatcher)
        {
            if (s_Logger == null)
            {
                s_Logger = new Logger(UIDispatcher);
            }
            return s_Logger;
        }

        private Dispatcher UIDispatcher = null;

        private Logger(Dispatcher UIDispatcher)
        {
            Logs = new ObservableCollection<String>();
            this.UIDispatcher = UIDispatcher != null ? UIDispatcher : Deployment.Current.Dispatcher;
        }

        public ObservableCollection<String> Logs
        {
            get;
            private set;
        }

        public void AddLog(String LogInfo)
        {
            UIDispatcher.BeginInvoke(() =>
            {
                System.Diagnostics.Debug.WriteLine(LogInfo);
                Logs.Add(LogInfo);
            });
        }
    }
}