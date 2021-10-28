using System.Collections;
using Hello.Interfaces.Data;

namespace Hello
{
    public class GetLocalFiles : IGetFiles
    {
        IList IGetFiles.Files
        {
            get
            {
                // TODO delegate
                ArrayList arrayList = new ArrayList();
                //
                arrayList.Add(23);
                arrayList.Add("hello");
                return arrayList;
            }
        }
    }
}
