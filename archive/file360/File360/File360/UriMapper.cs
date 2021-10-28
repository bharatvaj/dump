using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Windows.Phone.Storage.SharedAccess;

namespace WindowsPhoneFileandURIAssociationDemo
{
    class UriMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            string uriToLaunch = uri.ToString();

            // File association launch
            if (uriToLaunch.Contains("/FileTypeAssociation"))
            {
                
                int fileIDIndex = uriToLaunch.IndexOf("fileToken=") + "fileToken=".Length;
                string fileID = uriToLaunch.Substring(fileIDIndex);

                // Get the file name.
                string incomingFileName = SharedStorageAccessManager.GetSharedFileName(fileID);

                // Get the file extension.
                string incomingFileType = Path.GetExtension(incomingFileName);

                // Map the file extension to different pages.
                switch (incomingFileType)
                {
                    case ".pdf":
                        return new Uri("/EBookReader.xaml?fileToken=" + fileID, UriKind.Relative);
                    case ".svg":
                        return new Uri("/VideoPlayer.xaml?fileToken=" + fileID, UriKind.Relative);
                    case ".mkv":
                        return new Uri("/VideoPlayer.xaml?fileToken=" + fileID, UriKind.Relative);
                    case ".rar":
                        return new Uri("/VideoPlayer.xaml?fileToken=" + fileID, UriKind.Relative);
                    case ".csv":
                        return new Uri("/EBookReader.xaml?fileToken=" + fileID, UriKind.Relative);
                    case ".avi":
                        return new Uri("/EBookReader.xaml?fileToken=" + fileID, UriKind.Relative);
                    case ".7z":
                        return new Uri("/EBookReader.xaml?fileToken=" + fileID, UriKind.Relative);
                    default:
                        return new Uri("/MainPage.xaml", UriKind.Relative);
                }
            }
            else if (System.Net.HttpUtility.UrlDecode(uriToLaunch).Contains("f360:ShowURIPage?UniqueID="))
            {
                string uniqueId = uriToLaunch.Substring(System.Net.HttpUtility.UrlDecode(uriToLaunch).IndexOf("UniqueID=") + "UniqueID=".Length);

                // Map the request to URIScheme Landing page URISchemeFooBarLandingPage.xaml
                return new Uri("/URISchemeExtensionsLandingPage.xaml?UniqueID=" + uniqueId, UriKind.Relative);

            }
            // Otherwise perform normal launch.
            return uri;
        }

    }
}
