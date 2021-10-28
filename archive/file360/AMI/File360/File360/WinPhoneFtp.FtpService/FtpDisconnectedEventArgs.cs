namespace WinPhoneFtp.FtpService
{
	public class FtpDisconnectedEventArgs
	{
		internal FtpDisconnectedEventArgs(FtpDisconnectReason DisconnectReason)
		{
			this.DisconnectReason = DisconnectReason;
		}

		public FtpDisconnectReason DisconnectReason
		{
			get;
			private set;
		}
	}
}