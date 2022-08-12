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
        public static Frame RootMainFrame;

        public static bool DisableGoBack = false;
        public static void NavigateFrame(Type pageType)
        {
            RootMainFrame.Navigate(pageType, null, new EntranceNavigationTransitionInfo());
        }
        public MainWindow()
        {
            Title = "MSFree4all";
            this.InitializeComponent();
            Titlebar();
            RootMainFrame = MainFrame;
            RootMainFrame.Navigated += RootMainFrame_Navigated;
            RootMainFrame.Content = OfficePage;
            navView.SelectedItem = mitDeployOffice;
            this.Activated += OnWindowCreate;
        }
        void Titlebar()
        {
            FrameworkElement RootUI = (FrameworkElement)Content;
            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                AppWindow AppWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(this)));
                var titlebar = AppWindow.TitleBar;
                titlebar.ExtendsContentIntoTitleBar = true;
                void SetColor(ElementTheme acualTheme)
                {
                    titlebar.ButtonHoverBackgroundColor = App.LayerFillColorDefaultColor;
                    titlebar.ButtonBackgroundColor = titlebar.ButtonInactiveBackgroundColor = Colors.Transparent;
                    switch (acualTheme)
                    {
                        case ElementTheme.Dark:
                            titlebar.ButtonForegroundColor = Colors.White;
                            titlebar.ButtonHoverForegroundColor = Colors.Silver;
                            titlebar.ButtonPressedForegroundColor = Colors.Silver;
                            break;
                        case ElementTheme.Light:
                            titlebar.ButtonForegroundColor = Colors.Black;
                            titlebar.ButtonHoverForegroundColor = Colors.DarkGray;
                            titlebar.ButtonPressedForegroundColor = Colors.DarkGray;
                            break;
                    }
                }
                RootUI.ActualThemeChanged += (s, _) => SetColor(s.ActualTheme);
                SetTitleBar(AppTitleBar);
                SetColor(RootUI.ActualTheme);
            }
            else
            {
                ExtendsContentIntoTitleBar = true;
                SetTitleBar(AppTitleBar);
            }
        }
        private void OnWindowCreate(object _, WindowActivatedEventArgs _1)
        {
            var icon = User32.LoadImage(
                hInst: IntPtr.Zero,
                name: $@"{Package.Current.InstalledLocation.Path}\Assets\MSFree4All.ico".ToCharArray(),
                type: User32.ImageType.IMAGE_ICON,
                cx: 0,
                cy: 0,
                fuLoad: User32.LoadImageFlags.LR_LOADFROMFILE | User32.LoadImageFlags.LR_DEFAULTSIZE | User32.LoadImageFlags.LR_SHARED
            );
            var Handle = WindowNative.GetWindowHandle(this);
            User32.SendMessage(Handle, User32.WindowMessage.WM_SETICON, (IntPtr)1, icon);
            User32.SendMessage(Handle, User32.WindowMessage.WM_SETICON, (IntPtr)0, icon);
            Activated -= OnWindowCreate;
        }
        public void TriggerTitleBarRepaint()
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
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
            string h = "";
            Thickness margin;
            if (t.Equals(typeof(Views.OfficeMainPage)))
            {
                h = "Office";
            }
            else
            {
                h = ((NavViewHeaderTemplate)navView.Header).HeaderText;
            }
            if(navView.DisplayMode == NavigationViewDisplayMode.Minimal)
            {
                navView.IsPaneToggleButtonVisible = true;
                margin = new Thickness(35, -40, 0, 0);
            }
            else
            {
                navView.IsPaneToggleButtonVisible = false;
                margin = new Thickness(-30, -20, 0, 10);
            }
            navView.Header = new NavViewHeaderTemplate { HeaderMargin = margin , HeaderText = h};
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
                    if(itm.Tag.ToString() == "OfficePage")
                    {
                        if(MainFrame.Content != OfficePage)
                        {
                            MainFrame.Content = OfficePage;
                        }
                    }
                }
            }
        }

        #endregion
    }
    public class NavViewHeaderTemplate
    { 
        public string HeaderText { get; set; }
        public Thickness HeaderMargin { get; set; }
    }
}
