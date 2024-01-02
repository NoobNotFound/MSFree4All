using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace MSFree4All.Helpers
{
    public static class Extensions
    {
        public static ContentDialog ToContentDialog(this UIElement content,string title,string closebtnText = null, ContentDialogButton defaultButton = ContentDialogButton.Close)
        {
            ContentDialog dialog = new()
            {
                XamlRoot = App.MainWindow.Content.XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = title,
                CloseButtonText = closebtnText,
                DefaultButton = defaultButton,
                Content = content
            };
            return dialog;
        }
        public static ContentDialog ToCodeContentDialog(this string code,string codeType,string Title)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(code);
            var d = new MarkdownTextBlock() { CornerRadius = new CornerRadius { TopLeft = 7, BottomLeft = 7, BottomRight = 7, TopRight = 7 }, Padding = new Thickness(0), Text = $"```{codeType}\n{code}\n```", TextWrapping = TextWrapping.WrapWholeWords }.ToContentDialog(Title, "Ok", ContentDialogButton.Close);
            d.PrimaryButtonText = "Copy to clipboard";
            d.PrimaryButtonClick += (_, _) => { Clipboard.SetContent(dataPackage); MainWindow.NotificationBar.Notify("Copied to Clipboard!", InfoBarSeverity.Success); };
            return d;
        }
        public static void Show(this ContentDialog dialog)
        {
            try
            {
                _ = dialog.ShowAsync();
            }
            catch
            {}
        }
        public static async Task<BitmapImage> ToThumbnail(this StorageFile file)
        {
            try
            {
                var bm = new BitmapImage();
                await bm.SetSourceAsync(await file.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.SingleItem, 1024));
                return bm;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<BitmapImage> ToThumbnail(this StorageFolder Folder)
        {
            try
            {
                var bm = new BitmapImage();
                await bm.SetSourceAsync(await Folder.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.SingleItem, 1024));
                return bm;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<Models.StorageItem[]> ToStorageItemsArray(this IEnumerable<StorageFolder> folders)
        {
            var folderList = new List<Models.StorageItem>();
            foreach (var item in folders)
            {
                folderList.Add(new(item: item, type: Enums.StorageItemType.Folder, thumbnail: await item.ToThumbnail()));
            }
            return folderList.ToArray();
        }
        public static async Task<Models.StorageItem[]> ToStorageItemsArray(this IEnumerable<StorageFile> files)
        {
            var folderList = new List<Models.StorageItem>();
            foreach (var item in files)
            {
                folderList.Add(new(item: item, type: Enums.StorageItemType.File, thumbnail: await item.ToThumbnail()));
            }
            return folderList.ToArray();
        }
    }
}
