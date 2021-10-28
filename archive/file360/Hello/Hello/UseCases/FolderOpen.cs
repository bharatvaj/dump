using System.Collections;
using Hello.Interfaces.Data;
namespace Hello.UseCases
{
    public class FolderOpen : IGetFiles
    {
        public string FolderPath { get; set; }

        public IList IGetFiles.Files
        {
            get
            {
                IGetFiles getFiles= new GetLocalFiles();
                return getFiles.Files;
            }
        }

        public FolderOpen(string folderPath)
        {
            FolderPath = folderPath;
        }
        
    }
}
