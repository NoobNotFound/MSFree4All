using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MSFree4All.Helpers;
using MSFree4All.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace MSFree4All.Views
{
    public sealed partial class OfficeActPage : Page
    {
        public OfficeActPage()
        {
            this.InitializeComponent();
        }

        private async void NicExpander_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as NicExpander).Tag as string)
            {
                case "2016":
                    MainWindow.NotificationBar.Notify("Activating Office...", InfoBarSeverity.Informational);
                    MainCore.OfficeCore.Activator.Initialize((await App.GetLocalFolder()).Path);
                    _ = MainCore.OfficeCore.Activator.Activate2016();
                    break;
                case "2019":
                    MainWindow.NotificationBar.Notify("Activating Office...", InfoBarSeverity.Informational);
                    MainCore.OfficeCore.Activator.Initialize((await App.GetLocalFolder()).Path);
                    _ = MainCore.OfficeCore.Activator.Activate2019();
                    break;
                default:
                    MainWindow.NotificationBar.Notify("Deactivating Office...", InfoBarSeverity.Informational);
                    MainCore.OfficeCore.Activator.Initialize((await App.GetLocalFolder()).Path);
                    _ = MainCore.OfficeCore.Activator.Deactivate();
                    break;
            }
        }
    }
}