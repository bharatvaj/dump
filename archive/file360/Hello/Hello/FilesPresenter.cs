using System;
using System.Collections;
using Hello.Interfaces;
namespace Hello
{
    public class FilesPresenter
    {
        public FilesPresenter()
        {

        }

        void OnFileOpen()
        {
            //
        }

        void OnFolderOpen()
        {
            /* 
             * show loading           
             * async load files.afterLoad(()=>{
             * update to view
             * hide loading
             * }
             */
            var  localFiles = new GetLocalFiles("Get path from addresesr");

        }

        void OnFolderBack()
        {

        }
    }
}
