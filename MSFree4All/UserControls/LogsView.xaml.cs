using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Wallet;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MSFree4All.Models
{
    public class SubLog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void Set<T>(ref T ob, T value)
        {
            ob = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        private string _Message;
        public string Message { get => _Message; set => Set(ref _Message, value); }
        private InfoBarSeverity _Severty;
        public InfoBarSeverity Severity { get => _Severty; set => Set(ref _Severty, value); }
        public string SevertyString
        {
            get => Severity switch
            {
                InfoBarSeverity.Error => "Error",
                InfoBarSeverity.Warning => "Warning",
                InfoBarSeverity.Success => "Success",
                _ => "Message",
            };
        }
        public DateTime Time { get; private set; }
        public SubLog(string message, InfoBarSeverity severity)
        {
            Message = message;
            Severity = severity;
            Time = DateTime.Now;
        }
    }
    public interface Log : INotifyPropertyChanged
    {
        public string Content { get; set; }
        public bool LoadSubLogs { get; set; }
        public object UniqueThings { get; set; }
        public DateTime Time { get; set; }
        public int ID { get; set; }
        public void OnPropertyChanged(string propertyName);
        public InfoBarSeverity Severity { get; set; }
        public ObservableCollection<UIElement> CustomControls { get; set; }
        public ObservableCollection<SubLog> SubLogs { get; set; }

    }
    public class StringLog : Log
    {
        public void Set<T>(ref T ob, T value)
        {
            ob = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        private string _Content;
        public string Content { get => _Content; set => Set(ref _Content, value); }
        public DateTime Time { get; set; }
        public int ID { get; set; }
        private InfoBarSeverity _Severty;
        public InfoBarSeverity Severity { get => _Severty; set => Set(ref _Severty, value); }
        public object UniqueThings { get; set; }

        public ObservableCollection<UIElement> _CustomControls;
        public ObservableCollection<UIElement> CustomControls { get => _CustomControls; set => Set(ref _CustomControls, value); }

        public ObservableCollection<SubLog> _SubLogs = new();
        public ObservableCollection<SubLog> SubLogs { get => _SubLogs; set => Set(ref _SubLogs, value); }

        private bool _LoadSubLogs = false;
        public bool LoadSubLogs { get => _LoadSubLogs; set => Set(ref _LoadSubLogs, value); }

        public StringLog(string content, DateTime time, int iD,InfoBarSeverity severity, object uniqueThings = null,ObservableCollection<UIElement> customCOntrols = null, ObservableCollection<SubLog> subLogs = null)
        {
            Content = content;
            Time = time;
            ID = iD;
            this.Severity = severity;
            UniqueThings = uniqueThings;
            CustomControls = customCOntrols;
            SubLogs = subLogs ?? new();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class ProgressLog : Log
    {
        public object UniqueThings { get; set; }
        public void Set<T>(ref T ob, T value)
        {
            ob = value;
            OnPropertyChanged(nameof(ProgressVisibility)); //I guess i should do these lol
            OnPropertyChanged(nameof(Severity)); //I guess i should do these lol
            OnPropertyChanged(null);
        }
        //
        private string _Content;
        public string Content { get => _Content; set => Set(ref _Content, value); }
        //
        public DateTime Time { get; set; }
        //
        public int ID { get; set; }
        //
        private int _Progress;
        public int Progress { get => _Progress; set => Set(ref _Progress, value); }
        //
        private bool _IsIndeterminate;
        public bool IsIndeterminate { get => _IsIndeterminate; set => Set(ref _IsIndeterminate, value); }
        //
        public Visibility ProgressVisibility { get => Progress != 100 || IsIndeterminate ? Visibility.Visible : Visibility.Collapsed; }
        //
        private InfoBarSeverity _Severty;
        public InfoBarSeverity Severity { get => _Severty; set => Set(ref _Severty, value); }
        //
        public ObservableCollection<UIElement> _CustomControls;
        public ObservableCollection<UIElement> CustomControls { get => _CustomControls; set => Set(ref _CustomControls, value); }

        public ObservableCollection<SubLog> _SubLogs = new();
        public ObservableCollection<SubLog> SubLogs { get => _SubLogs; set => Set(ref _SubLogs, value); }

        private bool _LoadSubLogs = false;
        public bool LoadSubLogs { get => _LoadSubLogs; set => Set(ref _LoadSubLogs, value); }
        public ProgressLog(string content, DateTime time, int iD,int progress = 0,InfoBarSeverity severity = InfoBarSeverity.Informational,bool isIndeterminate = false,object uniquethings = null, ObservableCollection<UIElement> customCOntrols = null, ObservableCollection<SubLog> subLogs = null)
        {
            Content = content;
            Time = time;
            ID = iD;
            Progress = progress;
            IsIndeterminate = isIndeterminate;
            UniqueThings = uniquethings;
            Severity = severity;
            CustomControls = customCOntrols;
            SubLogs = subLogs ?? new();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
namespace MSFree4All.UserControls
{
    public class LogsViewTemplateSelector : DataTemplateSelector
    {

        public DataTemplate String { get; set; }
        public DataTemplate Progress { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return ((Models.Log)item is Models.StringLog) ? String : Progress;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return ((Models.Log)item is Models.StringLog) ? String : Progress;
        }
    }
    public sealed partial class LogsView : UserControl
    {
        private int lastID = 0;

        private ObservableCollection<Models.Log> Logs { get; set; } = new();
        public List<Models.Log> AllLogs { get => Logs.ToList(); }

        public LogsView()
        {
            this.InitializeComponent();
            lv.ItemsSource = Logs;
        }
        public LogsView(IEnumerable<Models.Log> logs)
        {
            this.InitializeComponent();
            Logs = new(logs);
            lv.ItemsSource = Logs;
            lastID = logs.Count();
        }
        public int AddProgressLog(string content, int progress = 0, InfoBarSeverity severity = InfoBarSeverity.Informational,bool IsIndeterminate = false,object UniqueThings = null, ObservableCollection<UIElement> customCOntrols = null)
        {
            var l = new Models.ProgressLog(content, DateTime.Now, lastID, progress, severity, IsIndeterminate,UniqueThings,customCOntrols);
            lastID++;
            Logs.Add(l);
            return l.ID;
        }
        public object GetUniqueThings(int ID)
        {
            var l = Logs.FirstOrDefault(l => l.ID == ID);
            try
            {
                return l.UniqueThings;
            }
            catch
            {

                return null;
            }
        }
        public int[] SearchByUniqueThingsToString(string uniquethings)
        {
            try
            {
                return Logs.Where(x=> x.UniqueThings != null).Where(x => x.UniqueThings.ToString() == uniquethings).Select(x => x.ID).ToArray();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return Array.Empty<int>();
            }
        }
        public int AddStringLog(string content, InfoBarSeverity severity = InfoBarSeverity.Informational,object uniquethings = null, ObservableCollection<UIElement> customCOntrols = null)
        {
            var l = new Models.StringLog(content, DateTime.Now, lastID, severity,uniquethings,customCOntrols);
            lastID++;
            Logs.Add(l);
            return l.ID;
        }
        public bool RemoveLog(int ID)
        {
            var l = Logs.FirstOrDefault(l => l.ID == ID);
            try
            {
                Logs.Remove(l);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
        }
        public bool ChangeContent(int ID, string content)
        {
            try
            {
                Logs.FirstOrDefault(l => l.ID == ID).Content = content;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
        }
        public bool ChangeCustomControls(int ID, ObservableCollection<UIElement> controls)
        {
            try
            {
                Logs.FirstOrDefault(l => l.ID == ID).CustomControls = controls;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
        }
        public bool ChangeProgress(int ID, int progress)
        {
            try
            {
                ((Models.ProgressLog)Logs.FirstOrDefault(l => l.ID == ID)).Progress = progress;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
        }
        public void Refresh()
        {
            lv.ItemsSource = null;
            lv.ItemsSource = Logs;
        }
        public bool ChangeIndeterminate(int ID, bool isIndeterminate)
        {
            try
            {
                ((Models.ProgressLog)Logs.FirstOrDefault(l => l.ID == ID)).IsIndeterminate = isIndeterminate;
                return true;
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
        }
        public bool ChangeSeverty(int ID, InfoBarSeverity severity)
        {
            try
            {
                Logs.FirstOrDefault(l => l.ID == ID).Severity = severity;
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
        }
        public bool AddSubLog(int ID, Models.SubLog sublog)
        {
            try
            {
                Logs.FirstOrDefault(l => l.ID == ID).SubLogs.Add(sublog);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        private void btnSubLog_Click(object sender, RoutedEventArgs e)
        {
            var log = ((sender as Button).DataContext as Models.Log);
            log.LoadSubLogs = !log.LoadSubLogs;
            
        }
    }
}
