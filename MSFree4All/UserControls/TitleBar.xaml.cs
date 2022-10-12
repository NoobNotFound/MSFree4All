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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MSFree4All.UserControls
{
    public sealed partial class TitleBar : UserControl
    {
        private static event EventHandler? OnTitleChanged;
        private static string appTitle;
        public static string AppTitle { get => string.IsNullOrEmpty(appTitle) ? "MSFree4All" : appTitle; set { appTitle = value; OnTitleChanged?.Invoke(appTitle, new()); } }
        public TitleBar()
        {
            this.InitializeComponent();
            OnTitleChanged += TitleBar_OnTitleChanged;
            TitleBar_OnTitleChanged(null,null);
        }

        private void TitleBar_OnTitleChanged(object sender, EventArgs e)
        {
            this.DispatcherQueue.TryEnqueue(() =>
            {
                txtAppTitle.Text = "";
                txtAppTitle.Text = AppTitle;
            });
        }
    }
}
