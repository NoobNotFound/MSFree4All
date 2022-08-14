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
using MSFree4All.Helpers;
using MSFree4All.Core.Office.Enums;
using MSFree4All.Core.Office;
using MSFree4All.UserControls;  
using MSFree4All.Core.Office.Models;
using MSFree4All.Core;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MSFree4All.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OfficeProductEditor : Page
    {
        public int Count;
        int langCount;
        public OfficeProductEditor()
        {
            this.InitializeComponent();
            this.Count = MainCore.Office.SelectedProductCount;
            foreach (var item in MainCore.Office.OfficeCore.Add.Products)
            {
                if (item.Count == Count)
                {
                    var id = item.ID;
                    if (id.ConvertToString() != "")
                    {
                        btnID.Content = id.ConvertToString() + " " + id.ConverToVer();
                    }
                    lvLangs.ItemsSource = item.Languages;
                    appsView.ExcludeApps = item.ExcludeApps;
                    txtPIDKEY.Text = item.PIDKEY;
                    return;
                }
            }
        }

        private void mitChooseProduct_Click(object sender, RoutedEventArgs e)
        {
            var s = ((MenuFlyoutItem)sender);
            foreach (var item in MainCore.Office.OfficeCore.Add.Products)
            {
                if(item.Count == Count)
                {
                    var id = (OfficeProductIDs)Enum.Parse(typeof(OfficeProductIDs), ToolTipService.GetToolTip(s).ToString());
                    item.ID = id;
                    btnID.Content = item.ID.ConvertToString() + " " + item.ID.ConverToVer();
                    return;
                }
            }
        }

        private void btnAddLang_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in MainCore.Office.OfficeCore.Add.Products)
            {
                if (item.Count == Count)
                {
                    langCount++;
                    item.Languages.Add(new OfficeLanguage(langCount));
                    return;
                }
            }
        }

        private void txtLang_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            try
            {
                if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    var suitableItems = new List<OfficeLanguage>();
                    var splitText = sender.Text.ToLower().Split(" ");
                    foreach (var itm in Consts.Languages)
                    {
                        var found = splitText.All((key) =>
                        {
                            return itm.Item1.ToLower().Contains(key) || itm.Item2.ToLower().Contains(key);
                        });
                        if (found)
                        {
                            suitableItems.Add(new OfficeLanguage(0) { Culture = itm.Item2, DisplayName = itm.Item1 });
                        }
                    }
                    if (suitableItems.Count == 0)
                    {
                        suitableItems.Add(new OfficeLanguage(0) { Culture = "MatchOS", DisplayName = "System Language" });
                    }
                    sender.ItemsSource = suitableItems;
                }
            }
            catch { }
        }

        private void txtLang_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            foreach (var item in MainCore.Office.OfficeCore.Add.Products)
            {
                if (item.Count == Count)
                {
                    foreach (var lang in item.Languages)
                    {
                        if(lang.Count == int.Parse(sender.Tag.ToString()))
                        {
                            lang.Culture = ((OfficeLanguage)args.SelectedItem).Culture;
                            lang.DisplayName = ((OfficeLanguage)args.SelectedItem).DisplayName;
                            sender.Text = ((OfficeLanguage)args.SelectedItem).Culture;
                            return;
                        }
                    }
                }
            }
        }

        private void btnRemoveLang_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in MainCore.Office.OfficeCore.Add.Products)
            {
                if (item.Count == Count)
                {
                    foreach (var lang in item.Languages)
                    {
                        if (lang.Count == int.Parse(((Button)sender).Tag.ToString()))
                        {
                            item.Languages.Remove(lang);
                            return;
                        }
                    }
                }
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in MainCore.Office.OfficeCore.Add.Products)
            {
                if (item.Count == Count)
                {
                    MainCore.Office.OfficeCore.Add.Products.Remove(item);
                    OfficeMainPage.MainFrame.Navigate(typeof(OfficePage),null, new Microsoft.UI.Xaml.Media.Animation.SlideNavigationTransitionInfo { Effect = Microsoft.UI.Xaml.Media.Animation.SlideNavigationTransitionEffect.FromLeft });
                    return;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var errors = MainCore.Office.OfficeCore.Add.CheckProductErrors(Count);
            if (errors.Count() == 0)
            {
                OfficeMainPage.MainFrame.Navigate(typeof(OfficePage), null, new Microsoft.UI.Xaml.Media.Animation.SlideNavigationTransitionInfo { Effect = Microsoft.UI.Xaml.Media.Animation.SlideNavigationTransitionEffect.FromLeft });
            }
            else
            {
                List<string> errorsList = new();
                foreach (var item in errors)
                {
                    errorsList.Add(item.ToReadableString());
                }
                _ = new BulletsList(errorsList).ToContentDialog("Fix these errors before you go!", "Ok", ContentDialogButton.Close).ShowAsync();

            }
        }

        private void txtPIDKEY_TextChanged(object sender, TextChangedEventArgs e)
        {

            foreach (var item in MainCore.Office.OfficeCore.Add.Products)
            {
                if (item.Count == Count)
                {
                    item.PIDKEY = txtPIDKEY.Text;
                    return;
                }
            }
        }
    }
}
