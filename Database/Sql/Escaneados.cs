using Perfect_Scan.Manager;
using System.ComponentModel;

namespace Perfect_Scan.Database.Sql
{
    public class Escaneados : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string displayName;

        [SQLite.Net.Attributes.PrimaryKey, SQLite.Net.Attributes.AutoIncrement]
        public int ID { get; set; }
        public string Icone { get { return CodigoRetorno.GetIcone(Tipo); } }
        public string DisplayName
        {
            get
            { return displayName; }
            set
            {
                displayName = value;
                OnPropertyChanged("DisplayName");
            }
        }
        public string Codigo { get; set; }
        public string Data { get; set; }
        public string Formato { get; set; }
        public string Tipo { get; set; }
        public bool IsNuvem { get; set; }
        public string UUID { get; set; }

        public string GetResumo
        {
            get { return Data + " ~ " + Codigo; }
        }


        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}