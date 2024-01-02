using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using MSFree4All.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using MSFree4All.Helpers;
using Windows.Storage.Pickers;
using System.Threading.Tasks;
namespace MSFree4All.Enums
{
    public enum StorageItemType
    {
        Folder,
        File
    }
}
namespace MSFree4All.Models
{
    public class StorageItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void Set<T>(ref T obj, T value, string name = null)
        {
            obj = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private IStorageItem _Item;
        public IStorageItem Item { get => _Item; set => Set(ref _Item, value); }


        private BitmapImage _Thumbnail;
        public BitmapImage Thumbnail { get => _Thumbnail; set => Set(ref _Thumbnail, value); }

        private bool _IsChecked;
        public bool IsChecked { get => _IsChecked; set => Set(ref _IsChecked, value); }

        private Enums.StorageItemType _ItemType;
        public Enums.StorageItemType ItemType { get => _ItemType; private set => Set(ref _ItemType, value); }

        public string DisplayName()
        {
            return ItemType == Enums.StorageItemType.Folder ? (string.IsNullOrEmpty(((StorageFolder)Item).DisplayName) ? ((StorageFolder)Item).Name : ((StorageFolder)Item).DisplayName) : (string.IsNullOrEmpty(((StorageFile)Item).DisplayName) ? ((StorageFile)Item).Name : ((StorageFile)Item).DisplayName);
        }
        public StorageItem(BitmapImage thumbnail,Enums.StorageItemType type,IStorageItem item)
        {
            Thumbnail = thumbnail;
            ItemType = type;
            Item = item;

        }
    }
}
namespace MSFree4All.UserControls
{
    public sealed partial class FolderPicker : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? SelectionChanged;
        public event EventHandler<StorageItem>? FileClicked;
        private void Set<T>(ref T obj, T value, string name = null)
        {
            obj = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<StorageItem> _ItemsSource = new();
        public ObservableCollection<StorageItem> ItemsSource { get => _ItemsSource; private set => Set(ref _ItemsSource, value); }


        private StorageFolder _BaseFolder;
        public StorageFolder BaseFolder { get => _BaseFolder; private set { Set(ref _BaseFolder, value); Refresh(); } }

        private StorageFolder _RootFolder = null;
        public StorageFolder RootFolder { get => _RootFolder; set { Set(ref _RootFolder, value); Refresh(); } }

        private ListViewSelectionMode _SelectionMode = ListViewSelectionMode.Extended;
        public ListViewSelectionMode SelectionMode { get => _SelectionMode; set => Set(ref _SelectionMode, value); }

        public List<(IStorageItem Item, Enums.StorageItemType ItemType)> SelectedItems { get; private set; } = new();

        private bool _IsTopFolderButtonEnabled = true;
        public bool IsTopFolderButtonEnabled { get => _IsTopFolderButtonEnabled; set => Set(ref _IsTopFolderButtonEnabled, value); }


        private bool _ItemsClickEnabled = true;
        public bool ItemsClickEnabled { get => _ItemsClickEnabled; set => Set(ref _ItemsClickEnabled, value); }


        public bool FileClickEnabled { get; set; } = true;
        public bool FileClickOnlyIf { get; set; } = false;
        public IList<string> FileClickOnlyIfType { get; set; } = new List<string>();
        public bool LaunchFiles { get; set; } = true;
        public bool SelectFiles { get; set; } = true;
        public bool SelectFolders { get; set; } = true;
        public bool SelectFilesOnlyIf { get; set; } = false;
        public bool GotoFolders { get; set; } = true;
        public IList<string> SelectFilesOnlyIfType { get; private set; } = new List<string>();
        public bool LaunchFilesOnlyIf { get; set; } = false;
        public IList<string> LaunchFilesOnlyIfType { get; private set; } = new List<string>();
        public bool IsDirectoryBarEnabled { get; set; } = true;
        public IList<string> NeverGotoFolders { get; private set; } = new List<string>();

        public IList<string> NoDelete { get; private set; } = new List<string>();

        public FolderPicker(StorageFolder baseFolder)
        {
            this.InitializeComponent();
            this.DataContext = this;
            BaseFolder = baseFolder;
        }

        private bool clickedOnce = false;
        private StorageItem OldItem = null;
        private async void AdaptiveGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is StorageItem item)
            {
                if (!clickedOnce)
                {
                    OldItem = item;
                    clickedOnce = true;
                    await Task.Delay(500);
                    clickedOnce = false;
                    OldItem = null;
                    return;
                }
                if (OldItem == item)
                {
                    if (item.ItemType == Enums.StorageItemType.Folder)
                    {
                        if (GotoFolders && !NeverGotoFolders.Contains(item.Item.Name))
                        {
                            BaseFolder = (StorageFolder)item.Item;
                        }
                    }
                    else
                    {
                        var file = (StorageFile)item.Item;
                        if (FileClickEnabled && ((FileClickOnlyIf && FileClickOnlyIfType.Contains(file.FileType.ToLower())) || !FileClickOnlyIf))
                        {
                            FileClicked?.Invoke(this, item);
                        }
                        if (LaunchFiles)
                        {

                            if (LaunchFilesOnlyIf && !LaunchFilesOnlyIfType.Contains(file.FileType.ToLower()))
                                return;

                            if (file.FileType.ToLower() == ".exe")
                            {
                                try
                                {
                                    new System.Diagnostics.Process() { StartInfo = new() { FileName = file.Path } }.Start();
                                }
                                catch { }
                            }
                            else
                            {
                                await Windows.System.Launcher.LaunchFileAsync(file);
                            }
                        }
                    }
                }
            }
        }
        public async void Refresh()
        {
            this.IsEnabled = false;
            if (RootFolder != null && !BaseFolder.Path.Contains(RootFolder.Path))
            {
                BaseFolder = RootFolder;
            }
            pbLoading.Visibility = Visibility.Visible;
            var fos = await (await BaseFolder.GetFoldersAsync()).ToStorageItemsArray();
            var fis = await (await BaseFolder.GetFilesAsync()).ToStorageItemsArray();
            var l = new List<StorageItem>();
            l.AddRange(fos);
            l.AddRange(fis);
            txtPath.Text = BaseFolder.Path;
            var bcbFs = new ObservableCollection<StorageFolder>();
            var f = BaseFolder;
            bcbFs.Add(BaseFolder);
            while (await f.GetParentAsync() != null)
            {
                var folder = await f.GetParentAsync();
                if (folder != null)
                {
                    bcbFs.Add(folder);
                    f = folder;
                }
            }
            var r = new ObservableCollection<StorageFolder>();
            for (int i = 0; i <= bcbFs.Count - 1; i++)
            {
                r.Add(bcbFs[bcbFs.Count - 1 - i]);
            }
            ItemsSource = new(l);
            pbLoading.Visibility = Visibility.Collapsed;
            BcBPath.ItemsSource = r;
            if (IsTopFolderButtonEnabled)
            {
                btnTopFolder.IsEnabled = BaseFolder == RootFolder ? false : await BaseFolder.GetParentAsync() != null;
            }
            txtEmpty.Visibility = ItemsSource.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
            this.IsEnabled = true;
        }
        private async void btnTopFolder_Click(object sender, RoutedEventArgs e)
        {
            if (await BaseFolder.GetParentAsync() != null)
            {
                BaseFolder = RootFolder != null && !(await BaseFolder.GetParentAsync()).Path.Contains(RootFolder.Path) ? RootFolder : await BaseFolder.GetParentAsync();
            }
        }

        private void BcBPath_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            if (BaseFolder.Path != ((StorageFolder)args.Item).Path && IsDirectoryBarEnabled)
                BaseFolder = (StorageFolder)args.Item;
        }


        private async void btnEditDir_Click(object sender, RoutedEventArgs e)
        {
            if (fDir.Glyph == "\xE70F")
            {
                fDir.Glyph = "\xE838";
                txtPath.Visibility = Visibility.Visible;
                BcBPath.Visibility = Visibility.Collapsed;
                txtPath.Focus(FocusState.Keyboard);
            }
            else
            {
                var fop = new Windows.Storage.Pickers.FolderPicker();

                WinRT.Interop.InitializeWithWindow.Initialize(fop, WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow));
                var f = await fop.PickSingleFolderAsync();

                if (f != null)
                    BaseFolder = f;
            }
        }

        private async void txtPath_LostFocus(object sender, RoutedEventArgs e)
        {
            await Task.Delay(new TimeSpan(0, 0, 1));
            if (txtPath.FocusState == FocusState.Unfocused)
            {
                ChangeDir(txtPath.Text);
                fDir.Glyph = "\xE70F";
                txtPath.Visibility = Visibility.Collapsed;
                BcBPath.Visibility = Visibility.Visible;
            }
        }

        private void txtPath_KeyDown(object sender, KeyRoutedEventArgs e)
        {

            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                ChangeDir(txtPath.Text);
                fDir.Glyph = "\xE70F";
                txtPath.Visibility = Visibility.Collapsed;
                BcBPath.Visibility = Visibility.Visible;
            }
        }

        public async void ChangeDir(string dir)
        {
            StorageFolder f;
            try
            {
                f = await StorageFolder.GetFolderFromPathAsync(dir);
            }
            catch
            {
                f = null;
            }
            if (f != null)
            {
                if (BaseFolder.Path != f.Path)
                    BaseFolder = f;
            }
            else
            {
                txtPath.Text = BaseFolder.Path;
            }
        }

        private async void bthDel_Click(object sender, RoutedEventArgs e)
        {
            if (agv.SelectedItems.Count > 0)
            {
                this.IsEnabled = false;
                pbLoading.Visibility = Visibility.Visible;
                var noDeleteItems = new List<string>();
                foreach (StorageItem item in agv.SelectedItems)
                {
                    if (!NoDelete.Contains(item.Item.Name))
                    {
                        await item.Item.DeleteAsync();
                    }
                    else
                    {
                        noDeleteItems.Add(item.Item.Name);
                    }
                }
                if (noDeleteItems.Any())
                {
                    new BulletsList() { ItemsSource = noDeleteItems }.ToContentDialog("You are not allowed to delete", "Okay").Show();
                }
                Refresh();
            }
        }

        private void agv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItems = new();
            foreach (StorageItem item in agv.SelectedItems.Cast<StorageItem>())
            {
                if ((SelectFiles && item.ItemType == Enums.StorageItemType.File && ((SelectFilesOnlyIf && SelectFilesOnlyIfType.Contains((item.Item as StorageFile).FileType.ToLower())) || !SelectFilesOnlyIf)) || SelectFolders)
                {
                    SelectedItems.Add((item.Item, item.ItemType));
                }
            }
            SelectionChanged?.Invoke(this, new());
        }

        private async void btnOlocation_Click(object sender, RoutedEventArgs e)
        {
            var itm = agv.SelectedItems.FirstOrDefault();
            if (itm != null)
            {
                var f = (StorageItem)itm;
                if (f.ItemType == Enums.StorageItemType.Folder)
                {
                    await Windows.System.Launcher.LaunchFolderAsync((StorageFolder)f.Item);
                }
                else
                {
                    await Windows.System.Launcher.LaunchFolderAsync(BaseFolder);
                }
            }
            else
            {
                await Windows.System.Launcher.LaunchFolderAsync(BaseFolder);
            }
        }

        private void MenuFlyout_Opened(object sender, object e)
        {
            try
            {
                if (!agv.SelectedItems.Contains((sender as FrameworkElement).DataContext))
                    agv.SelectedItems.Add((sender as FrameworkElement).DataContext);
            }
            catch { }
        }
        private void Grid_ContextRequested(UIElement sender, ContextRequestedEventArgs args)
        {
        }
    }
}