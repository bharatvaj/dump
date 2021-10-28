using System.Text;
using Microsoft.Phone.Storage;

namespace File360
{
    class vaultlist
    {

        public string VaultName { set; get; }
        public string VaultName2 { set; get; }

        public vaultlist(string Vaultname, string VaultsubFolderCount)
        {
            this.VaultName = Vaultname;
            this.VaultName2 = VaultsubFolderCount;
        }
    }
}


