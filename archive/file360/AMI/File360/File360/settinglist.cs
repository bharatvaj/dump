using System.Text;
using Microsoft.Phone.Storage;

namespace File360
{
    class settinglist
    {
        public string SettingName { set; get; }
        public string SettingDescription { set; get; }
        public string SettingPicture { set; get; }

        public settinglist(string name, string description, string picture)
        {
            this.SettingName = name;
            this.SettingDescription = description;
            this.SettingPicture = picture;
        }
    }
}


