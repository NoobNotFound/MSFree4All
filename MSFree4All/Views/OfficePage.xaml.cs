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
            listProducts.ItemsSource = MainCore.Office.OfficeCore.Add.Products;
            UpdateRemoveMSIStates();

            tglCDNFallback.IsOn = MainCore.Office.OfficeCore.Add.AllowCdnFallback;
            tglDisplay.IsOn = MainCore.Office.OfficeCore.Display.Level == DisplayLevel.None;
            tglForceAppsShutDown.IsOn = MainCore.Office.OfficeCore.PropertyElements.FORCEAPPSHUTDOWN;
            tglPicIcons.IsOn = MainCore.Office.OfficeCore.PropertyElements.PinIconsToTaskbar;
            tglAutoAct.IsOn = MainCore.Office.OfficeCore.PropertyElements.AUTOACTIVATE;
            switch (MainCore.Office.OfficeCore.Add.Architecture)
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
            cmbxLicenseType.SelectedIndex = ((int)MainCore.Office.OfficeCore.PropertyElements.LicensingProperties.Type);
            chkBxSCLCacheOverride.IsChecked = MainCore.Office.OfficeCore.PropertyElements.LicensingProperties.SCLCacheOverride;
            txtSCLCacheOverrideDir.Text = MainCore.Office.OfficeCore.PropertyElements.LicensingProperties.SCLCacheOverrideDirectory;
            UpdateLicenseUI();

            cmbxChannel.SelectedIndex = ((int)MainCore.Office.OfficeCore.Add.Channel);
            txtDescription.Text = MainCore.Office.OfficeCore.Description;
            txtOrgName.Text = MainCore.Office.OfficeCore.CompanyName;
            txtFullVer.Text = MainCore.Office.OfficeCore.Add.Version;
            txtDownloadPath.Text = MainCore.Office.OfficeCore.Add.DownloadPath;

            cmbxUpdates.SelectedIndex = MainCore.Office.OfficeCore.Updates.Enabled.ToInt();
            cmbxUpdateChannel.SelectedIndex = MainCore.Office.OfficeCore.Updates.Channel == null ? 8 : ((int)MainCore.Office.OfficeCore.Updates.Channel.Value);

            txtUpdatesDeadline.Text = MainCore.Office.OfficeCore.Updates.DeadLine;
            txtUpdateVer.Text = MainCore.Office.OfficeCore.Updates.TargetVersion;
            txtUpdatePath.Text = MainCore.Office.OfficeCore.Updates.UpdatePath;
            UpdateUpdatesUI();
        }

        #region Products
        private void mitRemove_Click(object sender, RoutedEventArgs e)
        {

            foreach (var item in MainCore.Office.OfficeCore.Add.Products)
            {
                if (item.Count == int.Parse(((MenuFlyoutItem)sender).Tag.ToString()))
                {
                    MainCore.Office.OfficeCore.Add.Products.Remove(item);
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
            MainCore.Office.OfficeProductsCount++;
            MainCore.Office.OfficeCore.Add.Products.Add(new Core.Office.Models.OfficeProduct(MainCore.Office.OfficeProductsCount));
            MainCore.Office.SelectedProductCount = MainCore.Office.OfficeProductsCount;
            OfficeMainPage.MainFrame.Navigate(typeof(OfficeProductEditor), null, new Microsoft.UI.Xaml.Media.Animation.SlideNavigationTransitionInfo { Effect = Microsoft.UI.Xaml.Media.Animation.SlideNavigationTransitionEffect.FromRight });
        }
        #endregion

        #region RemoveMSI
        private void chkbxMSIRemove_Click(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.RemoveMSI.SetValues((bool)chkbxMSIRemove.IsChecked);
            UpdateRemoveMSIStates();
        }
        public void UpdateRemoveMSIStates()
        {

            chkbxForceUpgrade.IsChecked = MainCore.Office.OfficeCore.Add.ForceUpgrade;
            chkbxMSIRemove.IsChecked = MainCore.Office.OfficeCore.RemoveMSI.Value;
            chkbxMSILang.IsChecked = MainCore.Office.OfficeCore.RemoveMSI.IsSameLang;

            chkbxMSILang.IsEnabled = MainCore.Office.OfficeCore.RemoveMSI.Value;
            chkbxMSIVisPro.IsEnabled = MainCore.Office.OfficeCore.RemoveMSI.Value;
            chkbxMSIVisStd.IsEnabled = MainCore.Office.OfficeCore.RemoveMSI.Value;
            chkbxMSIPrjPro.IsEnabled = MainCore.Office.OfficeCore.RemoveMSI.Value;
            chkbxMSIPrjStd.IsEnabled = MainCore.Office.OfficeCore.RemoveMSI.Value;
            chkbxMSIInfoPath.IsEnabled = MainCore.Office.OfficeCore.RemoveMSI.Value;
            chkbxMSIInfoPathR.IsEnabled = MainCore.Office.OfficeCore.RemoveMSI.Value;
            chkbxMSISharePoint.IsEnabled = MainCore.Office.OfficeCore.RemoveMSI.Value;

            foreach (var item in MainCore.Office.OfficeCore.RemoveMSI.Apps)
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
            MainCore.Office.OfficeCore.RemoveMSI.IsSameLang = (bool)chkbxMSILang.IsChecked;
        }

        private void chkbxMSIApp_Click(object sender, RoutedEventArgs e)
        {
            var s = (CheckBox)sender;
            if (s.IsChecked == true)
            {
                MainCore.Office.OfficeCore.RemoveMSI.AddRemoveApp((RemoveMSIApps)Enum.Parse(typeof(RemoveMSIApps), s.Tag.ToString()));
            }
            else
            {
                MainCore.Office.OfficeCore.RemoveMSI.RemoveRemoveApp((RemoveMSIApps)Enum.Parse(typeof(RemoveMSIApps), s.Tag.ToString()));
            }
        }

        private void chkbxForceUpgrade_Click(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.Add.ForceUpgrade = (bool)chkbxForceUpgrade.IsChecked;
        }
        #endregion

        private void tglCDNFallback_Toggled(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.Add.AllowCdnFallback = tglCDNFallback.IsOn;
        }

        private void cmbxArch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbxArch.SelectedItem.ToString())
            {
                case "64 Bit":
                    MainCore.Office.OfficeCore.Add.Architecture = Architecture.x64;
                    break;

                case "32 Bit":
                    MainCore.Office.OfficeCore.Add.Architecture = Architecture.x86;
                    break;

                case "Match OS":
                    MainCore.Office.OfficeCore.Add.Architecture = Architecture.AutoDetect;
                    break;
            }
        }

        private void tglDisplay_Toggled(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.Display.Level = tglDisplay.IsOn ? DisplayLevel.None : DisplayLevel.Full;
        }

        private void tglForceAppsShutDown_Toggled(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.PropertyElements.FORCEAPPSHUTDOWN = tglForceAppsShutDown.IsOn;
        }

        private void tglPicIcons_Toggled(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.PropertyElements.PinIconsToTaskbar = tglPicIcons.IsOn;
        }

        private void tglAutoAct_Toggled(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.PropertyElements.AUTOACTIVATE = tglAutoAct.IsOn;
        }

        #region Licensing
        private void cmbxLicenseType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbxLicenseType.SelectedItem.ToString())
            {
                case "User Based":
                    MainCore.Office.OfficeCore.PropertyElements.LicensingProperties.Type = LicensingType.UserBased;
                    break;
                case "Device Based":
                    MainCore.Office.OfficeCore.PropertyElements.LicensingProperties.Type = LicensingType.DeviceBased;
                    break;
                case "Shared Computer":
                    MainCore.Office.OfficeCore.PropertyElements.LicensingProperties.Type = LicensingType.SharedComputer;
                    break;
            }
            UpdateLicenseUI();
        }
        private void UpdateLicenseUI()
        {
            bool isSCL = MainCore.Office.OfficeCore.PropertyElements.LicensingProperties.Type == LicensingType.SharedComputer;
            chkBxSCLCacheOverride.IsEnabled = isSCL;
            txtSCLCacheOverrideDir.IsEnabled = isSCL && chkBxSCLCacheOverride.IsChecked == true;

        }

        private void chkBxSCLCacheOverride_Click(object sender, RoutedEventArgs e)
        {
            MainCore.Office.OfficeCore.PropertyElements.LicensingProperties.SCLCacheOverride = chkBxSCLCacheOverride.IsChecked == true;
            UpdateLicenseUI();
        }

        private void txtSCLCacheOverrideDir_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.PropertyElements.LicensingProperties.SCLCacheOverrideDirectory = txtSCLCacheOverrideDir.Text;
        }
        #endregion

        private void cmbxChannel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Add.Channel = Enum.Parse<Channel>((cmbxChannel.SelectedItem as ComboBoxItem).Tag.ToString());
        }

        #region Information
        private void txtOrgName_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.CompanyName = txtOrgName.Text;
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Description = txtDescription.Text;
        }
        #endregion

        private void txtFullVer_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Add.Version = txtFullVer.Text;
        }

        private void txtDownloadPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Add.DownloadPath = txtDownloadPath.Text;
        }

        private void txtSourcePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Add.SourcePath = txtSourcePath.Text;
        }

        #region Updates
        private void cmbxUpdates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Updates.Enabled = cmbxUpdates.SelectedIndex.ToBool();
            UpdateUpdatesUI();
        }

        private void UpdateUpdatesUI()
        {
            txtUpdatePath.IsEnabled = MainCore.Office.OfficeCore.Updates.Enabled == true;
            txtUpdatesDeadline.IsEnabled = MainCore.Office.OfficeCore.Updates.Enabled == true;
            txtUpdateVer.IsEnabled = MainCore.Office.OfficeCore.Updates.Enabled == true;
            cmbxUpdateChannel.IsEnabled = MainCore.Office.OfficeCore.Updates.Enabled == true;
        }

        private void cmbxUpdateChannel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tag = ((ComboBoxItem)cmbxUpdateChannel.SelectedItem).Tag;
            MainCore.Office.OfficeCore.Updates.Channel = tag == null ? null : Enum.Parse<Channel>(tag.ToString());
        }

        private void txtUpdatePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Updates.UpdatePath = txtUpdatePath.Text;
        }

        private void txtUpdateVer_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Updates.TargetVersion = txtUpdateVer.Text;
        }

        private void txtUpdatesDeadline_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainCore.Office.OfficeCore.Updates.DeadLine = txtUpdatesDeadline.Text;
        }
        #endregion

        private void btnViewXML_Click(object sender, RoutedEventArgs e)
        {
            var r = MainCore.Office.OfficeCore.Compile();
            if(r.Count() > 0)
            {
                var s = new StackPanel();
                foreach (var item in r)
                {
                    s.Children.Add(new TextBlock() { Text = item.Title,FontWeight = FontWeights.SemiBold,FontSize = 16 });
                    var li = from t in item select t.ToReadableString();
                    s.Children.Add(new UserControls.BulletsList(li));
                }
                try
                {
                    _ = s.ToContentDialog("Serialize Error!", "Ok", ContentDialogButton.Close).ShowAsync();
                }
                catch { }
            }
            else
            {
                var dataPackage = new DataPackage();
                dataPackage.SetText(MainCore.Office.OfficeCore.SerializeLastCompiled());
                var d = new MarkdownTextBlock() { CornerRadius = new CornerRadius { TopLeft = 7, BottomLeft = 7, BottomRight = 7, TopRight = 7 }, Padding = new Thickness(0), Text = $"```xml\n{MainCore.Office.OfficeCore.SerializeLastCompiled()}\n```", TextWrapping = TextWrapping.WrapWholeWords }.ToContentDialog("Output", "Ok", ContentDialogButton.Primary);
                d.PrimaryButtonText = "Copy to clipboard";
                d.PrimaryButtonClick += (_,_) => Clipboard.SetContent(dataPackage);
                _ = d.ShowAsync();
            }
        }

        private void btnLoadXML_Click(object sender, RoutedEventArgs e)
        {
            var r = MainCore.Office.OfficeCore.DeserializeFromString("<configuration>\n<Add name=\"\" MigrateArch=\"True\" />\n</configuration>");
            var s = new StackPanel();
            foreach (var item in r)
            {
                s.Children.Add(new TextBlock() { Text = item.Title, FontWeight = FontWeights.SemiBold, FontSize = 16 });
                s.Children.Add(new UserControls.BulletsList(item));
            }
            try
            {
                _ = s.ToContentDialog("Serialize Error!", "Ok", ContentDialogButton.Close).ShowAsync();
            }
            catch { }
        }
    }
}
