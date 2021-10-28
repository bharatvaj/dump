using System;

namespace WinPhoneFtp.FtpService
{
	public class FtpDirectoryChangedEventArgs
	{
		internal FtpDirectoryChangedEventArgs(String RemoteDirectory)
		{
			this.RemoteDirectory = RemoteDirectory;
		}

		public String RemoteDirectory
		{
			get;
			private set;
		}
	}
}