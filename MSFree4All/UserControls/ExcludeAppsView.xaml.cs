using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MSFree4All.Core.Office.Enums;
using System.ComponentModel;
using System.Collections.ObjectModel;
using MSFree4All.Core;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MSFree4All.UserControls
{
    public sealed partial class ExcludeAppsView : UserControl
    {
        private ObservableCollection<ExcludeAppTemplate> AppsTemplates = new();
        public ExcludeAppsView()
        {
            foreach (var app in Util.GetEnumList<OfficeApps>())
            {
                AppsTemplates.Add(new ExcludeAppTemplate(app, true));
            }
            this.InitializeComponent();
        }
        public List<OfficeApps> ExcludeApps { get; set; } = new List<OfficeApps>();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in ExcludeApps)
            {
                foreach (var app in AppsTemplates)
                {
                    if(item == app.App)
                    {
                        app.Exists = false;
                    }
                }
            }
            lv.ItemsSource = AppsTemplates;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if(sender is CheckBox bx)
            {
                var app =  (OfficeApps)Enum.Parse(typeof(OfficeApps), bx.Content.ToString());
                foreach (var item in AppsTemplates)
                {
                    if(item.App == app)
                    {
                        if (bx.IsChecked == true)
                        {
                            item.Exists = true;
                            try
                            {
                                ExcludeApps.Remove(app);
                            }
                            catch { }
                        }
                        else
                        {
                            item.Exists = false;
                            ExcludeApps.Add(app);
                        }
                    }
                }
            }
        }
    }
    public class ExcludeAppTemplate : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
       public OfficeApps App { get; set; }
        private bool _Exists;
        public bool Exists { get { return _Exists; } set { _Exists = value; this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); } }

        public ExcludeAppTemplate(OfficeApps app, bool exists)
        {
            App = app;
            Exists = exists;
        }
    }
}
