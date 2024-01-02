// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MSFree4All.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WindowsPage : Page
    {
        public WindowsPage()
        {
            this.InitializeComponent();
        }

        private async void NicExpander_Click(object sender, RoutedEventArgs e)
        {
            MainCore.WindowsCore.Activator.Initialize((await App.GetLocalFolder()).Path);
            switch ((sender as NicExpander).Tag as string)
            {
                case "KMS":
                    MainWindow.NotificationBar.Notify("Activating Windows with KMS...", InfoBarSeverity.Informational);
                    MainCore.WindowsCore.Activator.ActivateWithKMS();
                    break;
                case "CheckStatus":
                    var nID = MainWindow.NotificationBar.Notify("Getting Windows activation status...", InfoBarSeverity.Informational,autoHide:false);
                    var key = await MainCore.WindowsCore.Activator.CheckStatusAsync();
                    MainWindow.NotificationBar.Hide(nID);
                    key.ToCodeContentDialog(null, "Activation status").Show();
                    break;
                case "GetDLKey":
                    MainWindow.NotificationBar.Notify("Getting Windows Product Key...", InfoBarSeverity.Informational);
                    MainCore.WindowsCore.Activator.GetDefaultKey();
                    break;
            }
        }
    }
}
