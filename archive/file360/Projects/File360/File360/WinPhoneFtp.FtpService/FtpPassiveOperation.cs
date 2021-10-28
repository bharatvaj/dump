namespace WinPhoneFtp.FtpService
{
    public enum FtpPassiveOperation: byte
    {
        None,
        FileUpload,
        FileDownload,
		ListDirectory
    }
}