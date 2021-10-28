namespace WinPhoneFtp.FtpService
{
	public enum  FtpFileTransferFailureReason: byte
	{
		None,
		MemoryCardNotFound,
		FileDoesNotExist,
		InputOutputError
	}
}