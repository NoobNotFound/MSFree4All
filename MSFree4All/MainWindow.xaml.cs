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
using System.Runtime.InteropServices;
using WinRT;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Composition;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Windowing;
using MSFree4All.Helpers;
using System.ComponentModel;
using PInvoke;
using Windows.ApplicationModel;
using WinRT.Interop;
using WinUIEx;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MSFree4All
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static Views.OfficeMainPage OfficePage = new();
        public static Views.OfficeActPage OfficeActPage = new();
        public static Views.WindowsPage WindowsPage = new();
        public static Frame RootMainFrame;
        public static UserControls.NotificationBar NotificationBar;
        public static UserControls.LogsView LogsView = new();
        public static bool DisableGoBack = false;
        public static void NavigateFrame(Type pageType)
        {
            RootMainFrame.Navigate(pageType, null, new EntranceNavigationTransitionInfo());
        }
        public MainWindow()
        {

            Title = "MSFree4all";
            this.InitializeComponent();
            TitleBarHelper.SetExtendedTitleBar(this, AppTitleBar);
            RootMainFrame = MainFrame;
            NotificationBar = NotificationsBar;
            RootMainFrame.Navigated += RootMainFrame_Navigated;
            RootMainFrame.Content = OfficePage;
            navView.SelectedItem = NitDeployOffice;
            this.Activated += OnWindowCreate;
            InitializeLogs();
            
            this.Closed += (_, _) => Application.Current.Exit();
        }
        private void InitializeLogs()
        {
            Core.Util.ProcessUtil.ProcessAdded += (n,id) => LogsView.AddProgressLog(n, IsIndeterminate: true,UniqueThings:id);
            Core.Util.ProcessUtil.OutputReceived += (s, e) =>
            {
                this.DispatcherQueue.TryEnqueue(() =>
                {
                    var logID = LogsView.SearchByUniqueThingsToString(e.ID.ToString()).FirstOrDefault();
                    LogsView.AddSubLog(logID, new Models.SubLog(e.Log.Message.Replace("\n", " \\n "), (InfoBarSeverity)e.Log.Severity));
                    if (e.Log.PopUp)
                        e.Log.Message.ToCodeContentDialog(null, e.Log.PopUpTitle).Show();
                });
            };
            Core.Util.ProcessUtil.ProcessRemoved += (name,id) =>
            {
                try
                {
                    this.DispatcherQueue.TryEnqueue(() =>
                    {
                        var logID = LogsView.SearchByUniqueThingsToString(id.ToString()).FirstOrDefault();
                        LogsView.ChangeProgress(logID, 100);
                        LogsView.ChangeIndeterminate(logID, false);
                        LogsView.Refresh();
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            };
        }
        private void OnWindowCreate(object _, WindowActivatedEventArgs _1)
        {
            IconHelper.SetIcon(this);
            Activated -= OnWindowCreate;
        }
        
        public void TriggerTitleBarRepaint()
        {
            var hwnd = WindowNative.GetWindowHandle(this);
            var activeWindow = Win32.GetActiveWindow();
            if (hwnd == activeWindow)
            {
                Win32.SendMessage(hwnd, Win32.WM_ACTIVATE, Win32.WA_INACTIVE, IntPtr.Zero);
                Win32.SendMessage(hwnd, Win32.WM_ACTIVATE, Win32.WA_ACTIVE, IntPtr.Zero);
            }
            else
            {
                Win32.SendMessage(hwnd, Win32.WM_ACTIVATE, Win32.WA_ACTIVE, IntPtr.Zero);
                Win32.SendMessage(hwnd, Win32.WM_ACTIVATE, Win32.WA_INACTIVE, IntPtr.Zero);
            }


        }
        #region NaviagtionView
        private void RootMainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            UpdateNavView();
        }
        public void UpdateNavView()
        {
            var t = RootMainFrame.Content.GetType();
            var tp = new NavViewHeaderTemplate();
            if (t.Equals(typeof(Views.OfficeMainPage)))
            {
                tp.HeaderText = "Office";
            }
            else if (t.Equals(typeof(UserControls.LogsView)))
            {
                tp.HeaderText = "Logs";
                tp.CustomButtonPadding = new Thickness(7);
                tp.CustomButtonText = new FontIcon { Glyph = "\xe8a7" };
            }
            else
            {
                tp.HeaderText = ((NavViewHeaderTemplate)navView.Header).HeaderText;
            }
            if(navView.DisplayMode == NavigationViewDisplayMode.Minimal)
            {
                navView.IsPaneToggleButtonVisible = true;
                tp.HeaderMargin = new Thickness(35, -40, 0, 0);
            }
            else
            {
                navView.IsPaneToggleButtonVisible = false;
                tp.HeaderMargin = new Thickness(-30, -20, 0, 10);
            }
            navView.Header = tp;

        }
        private void navView_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
        {
            UpdateNavView();
        }

        private void navView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {

            }
            else
            {
                if(navView.SelectedItem is NavigationViewItem itm && itm.Tag != null)
                {
                    var tag = itm.Tag.ToString();
                    switch (tag)
                    {
                        case "OfficePage":
                            if (RootMainFrame.Content != OfficePage)
                                RootMainFrame.Content = OfficePage;
                            break;
                        case "OfficeActPage":
                            if (RootMainFrame.Content != OfficeActPage)
                                RootMainFrame.Content = OfficeActPage;
                            break;
                        case "WindowsPage":
                            if (RootMainFrame.Content != WindowsPage)
                                RootMainFrame.Content = WindowsPage;
                            break;
                        case "LogsView":
                            if (RootMainFrame.Content != LogsView)
                            {
                                LogsView = new(LogsView.AllLogs);
                                RootMainFrame.Content = LogsView;
                            }
                            break;
                    }
                }
            }
            UpdateNavView();
        }
        private void NavViewCustomButtonClick(object sender, RoutedEventArgs e)
        {
            var t = RootMainFrame.Content.GetType();
            if (t.Equals(typeof(UserControls.LogsView)))
            {
                LogsInNewWindow();
            }
        }
        private bool IsLogsInNewWindow = false;
        public void LogsInNewWindow()
        {
            if (!IsLogsInNewWindow)
            {
                IsLogsInNewWindow = true;
                RootMainFrame.Content = OfficePage;
                navView.SelectedItem = NitDeployOffice;
                navView.FooterMenuItems.Remove(NitLogs);
                
                var w = new WindowEx { Title = "Logs - MSFree4All",MinHeight = 300, MinWidth = 400 };
                var g = new Grid();
                var cg = new Grid
                {
                    Background = Application.Current.Resources["LayerFillColorDefaultBrush"] as Brush,
                    Padding = new Thickness(5)
                };
                g.RequestedTheme = MainFrame.RequestedTheme;
                g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(48, GridUnitType.Auto) });
                g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(48, GridUnitType.Star) });
                cg.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(48, GridUnitType.Auto) });
                cg.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100, GridUnitType.Star) });
                var t = new UserControls.TitleBar() { Margin = new Thickness(12, 0, 0, 0), VerticalAlignment = VerticalAlignment.Top };
              
                g.Children.Add(t);
                Grid.SetRow(t, 0);
                var h = new TextBlock()
                {
                    Style = Application.Current.Resources["TitleTextBlockStyle"] as Style, 
                    Text = "Logs",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(15,0,0,0)
                };
                g.Children.Add(cg);
                Grid.SetRow(cg, 1);
                cg.Children.Add(h);
                LogsView = new(LogsView.AllLogs);
                cg.Children.Add(LogsView);
                Grid.SetRow(h, 0);
                Grid.SetRow(LogsView, 1);
                LogsView.Margin = new Thickness(10, 0, 10, 10);
                w.Content = g;

                w.Closed += (_, _) =>
                {
                    cg.Children.Remove(LogsView);
                    LogsView = new(LogsView.AllLogs);
                    RootMainFrame.Content = LogsView;
                    navView.FooterMenuItems.Add(NitLogs);
                    navView.SelectedItem = NitLogs;
                    IsLogsInNewWindow = false;
                    UpdateNavView();
                };
                UpdateNavView();
                w.Activate();
                w.SetWindowSize(600, 500);
                TitleBarHelper.SetExtendedTitleBar(w, t);
                new MicaBackground(w,MicaKind.Base).TrySetMicaBackdrop();
                IconHelper.SetIcon(w);
                w.BringToFront();
            }
        }

        #endregion

    }
    public class NavViewHeaderTemplate
    { 
        public string HeaderText { get; set; }
        public object CustomButtonText { get; set; }
        public Visibility CustomButtonVisibility { get => CustomButtonText == null ? Visibility.Collapsed : Visibility.Visible; }
        public Thickness HeaderMargin { get; set; }
        public Thickness CustomButtonPadding { get; set; } = new Button().Padding;
        
    }
}
