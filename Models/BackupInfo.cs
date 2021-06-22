
using System.ComponentModel;

namespace Perfect_Scan.Models
{
    public class BackupInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string displayName;
        public string BackupName
        {
            get
            { return displayName; }
            set
            {
                displayName = value;
                OnPropertyChanged("DisplayName");
            }
        }
        public string BackupData { get; set; }
        public string BackupSize { get; set; }
        public string BackupPath { get; set; } 
        public string BackupResume { get; set; }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}