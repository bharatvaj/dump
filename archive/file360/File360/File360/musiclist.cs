using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File360
{
    class musiclist
    {

        public string SongName{set;get;}
        public string ArtistName { set; get; }
        public string AlbumName { set; get; }
        public string Musicindex { set; get; }

        public musiclist(string songName, string artistName, string albumName, string index)
        {
            this.SongName = songName;
            this.ArtistName = artistName;
            this.AlbumName = albumName;
            this.Musicindex = index;
        }
    }
}
