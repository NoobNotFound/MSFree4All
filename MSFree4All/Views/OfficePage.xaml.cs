using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MSFree4All.Helpers;
using MSFree4All.Core.Office;
using MSFree4All.Core.Office.Enums;
using Microsoft.UI.Text;
using CommunityToolkit.WinUI.UI.Controls;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.ApplicationModel;

namespace MSFree4All.Views
{
    public sealed partial class OfficePage : Page
    {
        public OfficePage()
        {
            this.InitializeComponent();
            this.Initialize();
        }

        /// <summary>
        /// Loads data from the Core
        /// </summary>
        public void Initialize()
        {
            listProducts.ItemsSource = MainCore.Office.OfficeCore.Configuration.Add.Products;
            UpdateRemoveMSIStates();

            tglCDNFallback.IsOn = MainCore.Office.OfficeCore.Configuration.Add.AllowCdnFallback;
            tglDisplay.IsOn = MainCore.Office.OfficeCore.Configuration.Display.Level == DisplayLevel.None;
            tglForceAppsShutDown.IsOn = MainCore.Office.OfficeCore.Configuration.PropertyElements.FORCEAPPSHUTDOWN;
            tglPicIcons.IsOn = MainCore.Office.OfficeCore.Configuration.PropertyElements.PinIconsToTaskbar;
            tglAutoAct.IsOn = MainCore.Office.OfficeCore.Configuration.PropertyElements.AUTOACTIVATE;
            switch (MainCore.Office.OfficeCore.Configuration.Add.Architecture)
            {
                case Architecture.x64:
                    cmbxArch.SelectedIndex = 0;
                    break;

                case Architecture.x86:
                    cmbxArch.SelectedIndex = 1;
                    break;

                case Architecture.AutoDetect:
                    cmbxArch.SelectedIndex = 2;
                    break;
            }
            cmbxLicenseType.SelectedIndex = ((int)MainCore.Office.OfficeCore.Configuration.PropertyElements.LicensingProperties.Type);
            chkBxSCLCacheOverride.IsChecked = MainCore.Office.OfficeCore.Configuration.PropertyElements.LicensingProperties.SCLCacheOverride;
            txtSCLCacheOverrideDir.Text = MainCore.Office.OfficeCore.Configuration.PropertyElements.LicensingProperties.SCLCacheOverrideDirectory;
            UpdateLicenseUI();

            cmbxChannel.SelectedIndex = ((int)MainCore.Office.OfficeCore.Configuration.Add.Channel);
            txtDescription.Text = MainCore.Office.OfficeCore.Configuration.Description;
            txtOrgName.Text = MainCore.Office.OfficeCore.Configuration.CompanyName;
            txtFullVer.Text = MainCore.Office.OfficeCore.Configuration.Add.Version;
            txtDownloadPath.Text = MainCore.Office.OfficeCore.Configuration.Add.DownloadPath;

            cmbxUpdates.SelectedIndex = MainCore.Office.OfficeCore.Configuration.Updates.Enabled.ToInt();
            cmbxUpdateChannel.SelectedIndex = MainCore.Office.OfficeCore.Configuration.Updates.Channel == null ? 8 : ((int)MainCore.Office.OfficeCore.Configuration.Updates.Channel.Value);

            txtUpdatesDeadline.Text = MainCore.Office.OfficeCore.Configuration.Updates.DeadLine;
            txtUpdateVer.Text = MainCore.Office.OfficeCore.Configuration.Updates.TargetVersion;
            txtUpdatePath.Text = MainCore.Office.OfficeCore.Configuration.Updates.UpdatePath;
            UpdateUpdatesUI();
        }

        #region Products
        private void mitRemove_Click(object sender, RoutedEventArgs e)
        {

            foreach (var item in MainCore.Office.OfficeCore.Configuration.Add.Products)
            {
                if (item.Count == int.Parse(((MenuFlyoutItem)sender).Tag.ToString()))
                {
                    MainCore.Office.OfficeCore.Configuration.Add.Products.Remove(item);
                    return;
                }
            }
        }

        private void mitEdit_Click(object sender, RoutedEventArgs e)
        {
            MainCore.Office.SelectedProductCount = int.Parse(((MenuFlyoutItem)sender).Tag.ToString());
            OfficeMainPage.MainFrame.Navigate(typeof(OfficeProductEditor), null, new Microsoft.UI.Xaml.Media.Animation.SlideNavigationTransitionInfo { Effect = Microsoft.UI.Xaml.Media.Animation.SlideNavigationTransitionEffect.FromRight });
        }

        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            MainCore.Office.SelectedProductCount = int.Parse(((Button)sender).Tag.ToString());
            OfficeMainPage.MainFrame.Navigate(typeof(OfficeProductEditor));
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.OfficeProductsIDsCount++;
            MainCore.Office.OfficeCore.Configuration.Add.Products.Add(new Core.Office.Models.OfficeProduct(MainCore.Office.OfficeCore.OfficeProductsIDsCount));
            MainCore.Office.SelectedProductCount = MainCore.Office.OfficeCore.OfficeProductsIDsCount;
            OfficeMainPage.MainFrame.Navigate(typeof(OfficeProductEditor), null, new Microsoft.UI.Xaml.Media.Animation.SlideNavigationTransitionInfo { Effect = Microsoft.UI.Xaml.Media.Animation.SlideNavigationTransitionEffect.FromRight });
        }
        #endregion

        #region RemoveMSI
        private void chkbxMSIRemove_Click(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.RemoveMSI.SetValues((bool)chkbxMSIRemove.IsChecked);
            UpdateRemoveMSIStates();
        }
        public void UpdateRemoveMSIStates()
        {

            chkbxForceUpgrade.IsChecked = MainCore.Office.OfficeCore.Configuration.Add.ForceUpgrade;
            chkbxMSIRemove.IsChecked = MainCore.Office.OfficeCore.Configuration.RemoveMSI.Value;
            chkbxMSILang.IsChecked = MainCore.Office.OfficeCore.Configuration.RemoveMSI.IsSameLang;

            chkbxMSILang.IsEnabled = MainCore.Office.OfficeCore.Configuration.RemoveMSI.Value;
            chkbxMSIVisPro.IsEnabled = MainCore.Office.OfficeCore.Configuration.RemoveMSI.Value;
            chkbxMSIVisStd.IsEnabled = MainCore.Office.OfficeCore.Configuration.RemoveMSI.Value;
            chkbxMSIPrjPro.IsEnabled = MainCore.Office.OfficeCore.Configuration.RemoveMSI.Value;
            chkbxMSIPrjStd.IsEnabled = MainCore.Office.OfficeCore.Configuration.RemoveMSI.Value;
            chkbxMSIInfoPath.IsEnabled = MainCore.Office.OfficeCore.Configuration.RemoveMSI.Value;
            chkbxMSIInfoPathR.IsEnabled = MainCore.Office.OfficeCore.Configuration.RemoveMSI.Value;
            chkbxMSISharePoint.IsEnabled = MainCore.Office.OfficeCore.Configuration.RemoveMSI.Value;

            foreach (var item in MainCore.Office.OfficeCore.Configuration.RemoveMSI.Apps)
            {
                foreach (var Element in pnlRemMSIApps.Children)
                {
                    if(Element is CheckBox bx)
                    {
                        if(bx.Tag != null)
                        {
                            if(bx.Tag.ToString() == item.ToString())
                            {
                                bx.IsChecked = true;
                            }
                        }
                    }
                }
            }
        }

        private void chkbxMSILang_Click(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.RemoveMSI.IsSameLang = (bool)chkbxMSILang.IsChecked;
        }

        private void chkbxMSIApp_Click(object sender, RoutedEventArgs e)
        {
            var s = (CheckBox)sender;
            if (s.IsChecked == true)
            {
                MainCore.Office.OfficeCore.Configuration.RemoveMSI.AddRemoveApp((RemoveMSIApps)Enum.Parse(typeof(RemoveMSIApps), s.Tag.ToString()));
            }
            else
            {
                MainCore.Office.OfficeCore.Configuration.RemoveMSI.RemoveRemoveApp((RemoveMSIApps)Enum.Parse(typeof(RemoveMSIApps), s.Tag.ToString()));
            }
        }

        private void chkbxForceUpgrade_Click(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.Add.ForceUpgrade = (bool)chkbxForceUpgrade.IsChecked;
        }
        #endregion

        private void tglCDNFallback_Toggled(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.Add.AllowCdnFallback = tglCDNFallback.IsOn;
        }

        private void cmbxArch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbxArch.SelectedItem.ToString())
            {
                case "64 Bit":
                    MainCore.Office.OfficeCore.Configuration.Add.Architecture = Architecture.x64;
                    break;

                case "32 Bit":
                    MainCore.Office.OfficeCore.Configuration.Add.Architecture = Architecture.x86;
                    break;

                case "Match OS":
                    MainCore.Office.OfficeCore.Configuration.Add.Architecture = Architecture.AutoDetect;
                    break;
            }
        }

        private void tglDisplay_Toggled(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.Display.Level = tglDisplay.IsOn ? DisplayLevel.None : DisplayLevel.Full;
        }

        private void tglForceAppsShutDown_Toggled(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.PropertyElements.FORCEAPPSHUTDOWN = tglForceAppsShutDown.IsOn;
        }

        private void tglPicIcons_Toggled(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.PropertyElements.PinIconsToTaskbar = tglPicIcons.IsOn;
        }

        private void tglAutoAct_Toggled(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.PropertyElements.AUTOACTIVATE = tglAutoAct.IsOn;
        }

        #region Licensing
        private void cmbxLicenseType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbxLicenseType.SelectedItem.ToString())
            {
                case "User Based":
                    MainCore.Office.OfficeCore.Configuration.PropertyElements.LicensingProperties.Type = LicensingType.UserBased;
                    break;
                case "Device Based":
                    MainCore.Office.OfficeCore.Configuration.PropertyElements.LicensingProperties.Type = LicensingType.DeviceBased;
                    break;
                case "Shared Computer":
                    MainCore.Office.OfficeCore.Configuration.PropertyElements.LicensingProperties.Type = LicensingType.SharedComputer;
                    break;
            }
            UpdateLicenseUI();
        }
        private void UpdateLicenseUI()
        {
            bool isSCL = MainCore.Office.OfficeCore.Configuration.PropertyElements.LicensingProperties.Type == LicensingType.SharedComputer;
            chkBxSCLCacheOverride.IsEnabled = isSCL;
            txtSCLCacheOverrideDir.IsEnabled = isSCL && chkBxSCLCacheOverride.IsChecked == true;

        }

        private void chkBxSCLCacheOverride_Click(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.PropertyElements.LicensingProperties.SCLCacheOverride = chkBxSCLCacheOverride.IsChecked == true;
            UpdateLicenseUI();
        }

        private void txtSCLCacheOverrideDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.PropertyElements.LicensingProperties.SCLCacheOverrideDirectory = txtSCLCacheOverrideDir.Text;
        }
        #endregion

        private void cmbxChannel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.Add.Channel = Enum.Parse<Channel>((cmbxChannel.SelectedItem as ComboBoxItem).Tag.ToString());
        }

        #region Information
        private void txtOrgName_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.CompanyName = txtOrgName.Text;
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.Description = txtDescription.Text;
        }
        #endregion

        private void txtFullVer_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.Add.Version = txtFullVer.Text;
        }

        private void txtDownloadPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.Add.DownloadPath = txtDownloadPath.Text;
        }

        private void txtSourcePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.Add.SourcePath = txtSourcePath.Text;
        }

        #region Updates
        private void cmbxUpdates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.Updates.Enabled = cmbxUpdates.SelectedIndex.ToBool();
            UpdateUpdatesUI();
        }

        private void UpdateUpdatesUI()
        {
            txtUpdatePath.IsEnabled = MainCore.Office.OfficeCore.Configuration.Updates.Enabled == true;
            txtUpdatesDeadline.IsEnabled = MainCore.Office.OfficeCore.Configuration.Updates.Enabled == true;
            txtUpdateVer.IsEnabled = MainCore.Office.OfficeCore.Configuration.Updates.Enabled == true;
            cmbxUpdateChannel.IsEnabled = MainCore.Office.OfficeCore.Configuration.Updates.Enabled == true;
        }

        private void cmbxUpdateChannel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tag = ((ComboBoxItem)cmbxUpdateChannel.SelectedItem).Tag;
            MainCore.Office.OfficeCore.Configuration.Updates.Channel = tag == null ? null : Enum.Parse<Channel>(tag.ToString());
        }

        private void txtUpdatePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.Updates.UpdatePath = txtUpdatePath.Text;
        }

        private void txtUpdateVer_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.Updates.TargetVersion = txtUpdateVer.Text;
        }

        private void txtUpdatesDeadline_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Configuration.Updates.DeadLine = txtUpdatesDeadline.Text;
        }
        #endregion

        #region ToolBar
        public bool Compile()
        {
            var nID = MainWindow.NotificationBar.Notify("Compiling...", InfoBarSeverity.Informational, autoHide: false);
            var r = MainCore.Office.OfficeCore.Compile();
            MainWindow.NotificationBar.Hide(nID);
            if (r.Count() > 0)
            {
                MainWindow.NotificationBar.Change(nID,
                    title: "Compile Failed!",
                    severity: InfoBarSeverity.Error,
                    description: "Some errors were detected.");
                MainWindow.NotificationBar.Show(nID);
                var s = new StackPanel();
                foreach (var item in r)
                {
                    s.Children.Add(new TextBlock() { Text = item.Title, FontWeight = FontWeights.SemiBold, FontSize = 16 });
                    var li = from t in item select t.ToReadableString();
                    s.Children.Add(new UserControls.BulletsList() { ItemsSource = li });
                }
                try
                {
                    _ = s.ToContentDialog("Serialize Error!", "Ok", ContentDialogButton.Close).ShowAsync();
                }
                catch { }
                MainWindow.NotificationBar.WaintAndHide(new TimeSpan(0, 0, 3), nID);
                var btn = new Button() { Content = "Errors" };
                int eID=  MainWindow.LogsView.AddStringLog("Failed to compile Office configuration.", InfoBarSeverity.Error, r,customCOntrols: new() { btn });
                btn.Click += (_, _) =>
                {
                    s = new StackPanel();
                    foreach (var item in (ErrorsList[])MainWindow.LogsView.GetUniqueThings(eID))
                    {
                        s.Children.Add(new TextBlock() { Text = item.Title, FontWeight = FontWeights.SemiBold, FontSize = 16 });
                        var li = from t in item select t.ToReadableString();
                        s.Children.Add(new UserControls.BulletsList() { ItemsSource = li });
                    }
                    try 
                    {
                        _ = s.ToContentDialog("Serialize Error!", "Ok", ContentDialogButton.Close).ShowAsync();
                    } 
                    catch { } };
                return false;
            }
            MainWindow.NotificationBar.Change(nID,
                title: "Compile Complete!",
                severity: InfoBarSeverity.Success, visibility: true);
            var sbtn = new Button() { Content = "View" };
            var lid = MainWindow.LogsView.AddStringLog("Successfully compiled Office configuration.", InfoBarSeverity.Success, MainCore.Office.OfficeCore.SerializeLastCompiled(), customCOntrols: new() { sbtn });
            sbtn.Click += (_, _) => { try { _ = GetViewConfigDialog(MainWindow.LogsView.GetUniqueThings(lid).ToString()).ShowAsync(); } catch { } };
            MainWindow.NotificationBar.WaintAndHide(new TimeSpan(0, 0, 2), nID);
            return true;
        }
        private void btnViewXML_Click(object sender, RoutedEventArgs e)
        {
            if(Compile())
            {
                _ = GetViewConfigDialog().ShowAsync();
            }
        }
        private ContentDialog GetViewConfigDialog(string xml = null)
        {
            var cxml = xml ?? MainCore.Office.OfficeCore.SerializeLastCompiled();
            var dataPackage = new DataPackage();
            dataPackage.SetText(cxml);
            var d = new MarkdownTextBlock() { CornerRadius = new CornerRadius { TopLeft = 7, BottomLeft = 7, BottomRight = 7, TopRight = 7 }, Padding = new Thickness(0), Text = $"```xml\n{cxml}\n```", TextWrapping = TextWrapping.WrapWholeWords }.ToContentDialog("Output", "Ok", ContentDialogButton.Close);
            d.PrimaryButtonText = "Copy to clipboard";
            d.PrimaryButtonClick += (_, _) => Clipboard.SetContent(dataPackage);
            d.SecondaryButtonText = "Save";
            d.SecondaryButtonClick += (_, _) => btnSaveXML_Click(null, null);
            return d;
        }
        private async void btnLoadXML_Click(object sender, RoutedEventArgs e)
        {
            var nID = MainWindow.NotificationBar.Notify("Importing XML", InfoBarSeverity.Informational,description: "Waiting for the file...", autoHide:false);
            var fop = new FileOpenPicker();
            fop.FileTypeFilter.Add(".xml");
            WinRT.Interop.InitializeWithWindow.Initialize(fop, WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow));
            var f = await fop.PickSingleFileAsync();
            if (f != null)
            {
                var str = await FileIO.ReadTextAsync(f);
                var r = MainCore.Office.OfficeCore.DeserializeFromString(str,false);
                if (r.Count > 0)
                {
                    MainWindow.NotificationBar.Change(nID,
                        title: "Import Failed!",
                        severity: InfoBarSeverity.Error,
                        description: "There were errors in the configuration");
                    MainWindow.NotificationBar.WaintAndHide(new TimeSpan(0, 0, 3), nID);
                    var s = new StackPanel();
                    s.Children.Add(new UserControls.BulletsList { ItemsSource = r, WordWrap = false });
                    try
                    {
                        var d = new ScrollViewer() { Content = s, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto, HorizontalScrollMode = ScrollMode.Enabled, Padding = new Thickness(0, 0, 7, 7) }.ToContentDialog("Deserialize Error!", "Ok", ContentDialogButton.Close);
                        d.PrimaryButtonText = "Continue Anyaway";
                        d.PrimaryButtonClick += (s, e) => MainCore.Office.OfficeCore.DeserializeFromString(str, true);
                        _ = d.ShowAsync();
                    }
                    catch { }
                }
                else
                {
                    MainWindow.NotificationBar.Change(nID,
                        title: "Import Successful!",
                        severity: InfoBarSeverity.Success,
                        description: "Successfully imported XML from\n" + f.Path);
                    MainWindow.NotificationBar.WaintAndHide(new TimeSpan(0, 0, 3), nID);
                }
            }
            else
            {
                MainWindow.NotificationBar.Change(nID,
                    title: "Import Canceled!",
                    severity: InfoBarSeverity.Warning,
                    description: "The dialog was closed.");
                MainWindow.NotificationBar.WaintAndHide(new TimeSpan(0, 0, 3), nID);
            }
        }

        private async void btnSaveXML_Click(object sender, RoutedEventArgs e)
        {
            if(Compile())
            {

                var nID = MainWindow.NotificationBar.Notify("Picking a File", InfoBarSeverity.Informational,description:"Waiting for the user.", autoHide: false);
                var fsp = new FileSavePicker();
                fsp.FileTypeChoices.Add("XML Configuration", new List<string> { ".xml" });
                WinRT.Interop.InitializeWithWindow.Initialize(fsp, WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow));
                var f = await fsp.PickSaveFileAsync();
                if (f != null)
                {
                    await FileIO.WriteTextAsync(f, MainCore.Office.OfficeCore.SerializeLastCompiled());
                    MainWindow.NotificationBar.Change(nID,
                        title: "Export Successful!",
                        description:"",
                        severity: InfoBarSeverity.Success);
                }
                else
                {
                    MainWindow.NotificationBar.Change(nID,
                        title: "Export Failed!",
                        severity: InfoBarSeverity.Warning,
                        description:"The dialog was closed");
                }
                MainWindow.NotificationBar.WaintAndHide(new TimeSpan(0, 0, 3), nID);
            }
        }


        private async void MitDMedia_Click(object sender, RoutedEventArgs e)
        {
            if (Compile())
            {
                var nID = MainWindow.NotificationBar.Notify("Deploying Office", InfoBarSeverity.Informational,autoHide:false);
                string t = DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
                var folder = await (await App.GetLocalFolder()).CreateFolderAsync(t,CreationCollisionOption.GenerateUniqueName);
                var file = await (await StorageFile.GetFileFromPathAsync($@"{App.GetAppDir()}\Assets\Setup.exe")).CopyAsync(folder,"setup.exe",NameCollisionOption.ReplaceExisting);
                MainCore.Office.OfficeCore.Deployer.Intialize(file.Path,folder.Path);

                var errs = await MainCore.Office.OfficeCore.Deployer.Deploy(Core.Office.Deployer.Enums.DeployType.CreateMedia, MainCore.Office.OfficeCore.SerializeLastCompiled());
                if(errs.Count > 0)
                {

                }
                else
                {
                    MainWindow.NotificationBar.Change(nID,"Setup is running!",InfoBarSeverity.Success,"Successfully created config files to the " + folder.Path + ".");
                    MainWindow.NotificationBar.WaintAndHide(new TimeSpan(0, 0, 3), nID);
                }
            }
        }

        private async void btnDeployMents_Click(object sender, RoutedEventArgs e)
        {
            OfficeMainPage.MainFrame.Content = new UserControls.FolderPicker(await App.GetLocalFolder());
        }
        #endregion
    }
}
