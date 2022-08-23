using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSFree4All.Core.Office.Enums;
using System.Collections.ObjectModel;
using MSFree4All.Core.Office;
namespace MSFree4All.Core.Office.Models
{
    public class OfficeProduct : INotifyPropertyChanged
    {
        public string PIDKEY { get; set; } = "";
        public event PropertyChangedEventHandler? PropertyChanged;
        public List<OfficeApps> ExcludeApps = new();
        public OfficeProductIDs? ID { get; set; } = null;
        public int Count { get; private set; }
        public ObservableCollection<OfficeLanguage> Languages { get; set; } = new ObservableCollection<OfficeLanguage>();
        public string Version { get { return ID.ConverToVer().ToString(); } }
        public string DisplayName { get { return ID.ConvertToString(); } }
        public int LanguagesIDsCount { get; set; } = 0;
        public OfficeProduct(int count) { this.Count =count; }
    }
    public class OfficeLanguage : INotifyPropertyChanged
    {
        public override string ToString()
        {
            return Culture;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _DisplayName = "";
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set { _DisplayName = value; OnPropertyChanged(); }
        }
        private string _Culture = "";
        public string Culture
        {
            get
            {
                return _Culture;
            }
            set { _Culture = value; OnPropertyChanged(); }
        }

        public int Count;
        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        public OfficeLanguage(int count)
        {
            this.Count = count;
        }
    }
}
