using System.Text;
using Microsoft.Phone.Storage;

namespace File360
{
    class sdlist
    {

        public string Name 
        {
            set;
            get;
        }
        public string Name2 { set; get; }

        public sdlist(string name, string FImage)
        {
            this.Name = name;
            this.Name2 = FImage;
        }
    }
}
   

