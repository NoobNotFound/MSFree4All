using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using MSFree4All.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MSFree4All.Core;
using Windows.UI;
using Windows.Storage;
using System.Threading.Tasks;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MSFree4All
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static async Task<StorageFolder> GetLocalFolder()
        {
            try
            {
                return ApplicationData.Current.LocalFolder;
            }
            catch
            {
                return await (await StorageFolder.GetFolderFromPathAsync(GetAppDir())).CreateFolderAsync("BIN",CreationCollisionOption.OpenIfExists);
            }
        }
        public static string GetAppDir()
        {
            try
            {
                return Package.Current.InstalledLocation.Path ?? Environment.CurrentDirectory;
            }
            catch
            {
                return Environment.CurrentDirectory;
            }
        }
        public static Color LayerFillColorDefaultColor => (Color)Current.Resources["LayerFillColorDefault"];
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (System.Diagnostics.Process.GetProcessesByName("MSFree4All").Length == 1)
            {
                try
                {
                    var f = ApplicationData.Current.LocalFolder;
                }
                catch
                {
                    UserControls.TitleBar.AppTitle = "MSFree4All (Unpackaged)";
                }
                MainWindow = new MainWindow();
                MainWindow.Activate();

                new MicaBackground(MainWindow).TrySetMicaBackdrop();
            }
            else
            {
                Current.Exit();
            }
        }

        public static Window MainWindow;
    }
}
