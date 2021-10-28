using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File360
{
    class musiclist
    {
        public string SONG
        {
            set;
            get;
        }
        public string ARTIST { set; get; }

        public musiclist(string songName, string artistName)
        {
            this.SONG = songName;
            this.ARTIST = artistName;
        }
    }
}
