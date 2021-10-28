using System.Collections.ObjectModel;
using System.ComponentModel;

namespace File360
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<Folder> Folders { get; set; }

        public MainViewModel()
        {
            Folders = new ObservableCollection<Folder>();
        }


        #region Interface Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaiseNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}