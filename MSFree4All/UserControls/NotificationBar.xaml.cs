using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MSFree4All.UserControls
{
    public sealed partial class NotificationBar : UserControl
    {
        public readonly ObservableCollection<NotificationModel> AllNotifications = new();
        public NotificationBar()
        {
            this.InitializeComponent();
            lv.ItemsSource = AllNotifications;
        }

        public int Notify(string title, InfoBarSeverity severity,string description = null,bool autoHide = true,TimeSpan? autoHideTime = null)
        {
            var itm = new NotificationModel( AllNotifications.Count ,title, description, severity, true);
            AllNotifications.Add(itm);
            
            if (autoHide)
            {
                WaintAndHide(autoHideTime != null ? autoHideTime.Value : new TimeSpan(0, 0, 3), AllNotifications.Count - 1);
            }
            return AllNotifications.Count - 1;
        }
        public async void WaintAndHide(TimeSpan time, int ID)
        {
            await System.Threading.Tasks.Task.Delay(time);
            AllNotifications[ID].Visibility = false;
        }
        public void Change(int ID, string title = null, InfoBarSeverity? severity = null, string description = null,bool? visibility = null)
        {
            AllNotifications[ID].Title = title ?? AllNotifications[ID].Title;
            AllNotifications[ID].Severity = severity == null ? AllNotifications[ID].Severity : severity.Value;
            AllNotifications[ID].Description = description ?? AllNotifications[ID].Description;
            AllNotifications[ID].Visibility = visibility == null ? AllNotifications[ID].Visibility : visibility.Value;
        }
        public void Hide(int ID)
        {
            AllNotifications[ID].Visibility = false;
        }
        public void Show(int ID)
        {
            AllNotifications[ID].Visibility = true;
        }
        public void ChangeSeverity(int ID, InfoBarSeverity severity)
        {
            AllNotifications[ID].Severity = severity;
        }
        public void ChangeTitle(int ID, string title)
        {
            AllNotifications[ID].Title = title;
        }

        public void ChangeDescription(int ID,string description)
        {
            AllNotifications[ID].Description = description;
        }

        private void InfoBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var id = (int)(sender as InfoBar).Tag;
            AllNotifications[id].IsSmall = e.NewSize.Width < 30;
        }
    }
    public class NotificationModel : INotifyPropertyChanged
    {
        public int ID { get; private set; }
        private string _Title;
        private bool _IsSmall;
        public bool IsSmall { get { return _IsSmall; } set { _IsSmall = value; PropertyChanged(this, new PropertyChangedEventArgs(nameof(BarVisibility))); } }
        public string Title { get { return _Title; } set { _Title = value; PropertyChanged(this, new PropertyChangedEventArgs(nameof(Title))); } }
        private string _Description;
        public string Description { get { return _Description; } set { _Description = value; PropertyChanged(this, new PropertyChangedEventArgs(nameof(Description))); } }
        private InfoBarSeverity _severity;
        public InfoBarSeverity Severity { get { return _severity; } set { _severity = value; PropertyChanged(this, new PropertyChangedEventArgs(nameof(Severity))); } }
        public NotificationModel(int iD,string title, string description, InfoBarSeverity severity,bool visibility = true)
        {
            Title = title;
            Description = description;
            Severity = severity;
            Visibility = visibility;
            ID = iD;
        }
        bool visibility;
        public bool Visibility { get { return visibility; } set { visibility = value; PropertyChanged(this, new PropertyChangedEventArgs(nameof(BarVisibility))); } }
        public Visibility BarVisibility
        {
            get { return !IsSmall ? (Visibility ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed) : Microsoft.UI.Xaml.Visibility.Collapsed; }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }

}
