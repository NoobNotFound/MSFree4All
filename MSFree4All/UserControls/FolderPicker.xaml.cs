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
    public sealed partial class FolderPicker : UserControl,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void Set<T>(ref T obj,T value,string name = null)
        {
            obj = value;
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<StorageItem> _ItemsSource = new();
        public ObservableCollection<StorageItem> ItemsSource { get => _ItemsSource; private set => Set(ref _ItemsSource, value); }


        private StorageFolder _BaseFolder;
        public StorageFolder BaseFolder { get => _BaseFolder; private set { Set(ref _BaseFolder, value); Refresh(); } }


        private ListViewSelectionMode _SelectionMode = ListViewSelectionMode.Extended;
        public ListViewSelectionMode SelectionMode { get => _SelectionMode; set => Set(ref _SelectionMode, value); }


        private bool _IsTopFolderButtonEnabled = true;
        public bool IsTopFolderButtonEnabled { get => _IsTopFolderButtonEnabled; set => Set(ref _IsTopFolderButtonEnabled, value); }


        private bool _ItemsClickEnabled = true;
        public bool ItemsClickEnabled { get => _ItemsClickEnabled; set => Set(ref _ItemsClickEnabled, value); }

        
        public bool LaunchFiles { get; set; } = true;
        public bool IsDirectoryBarEnabled { get; set; } = true;

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
                        BaseFolder = (StorageFolder)item.Item;
                    }
                    else
                    {
                        if (LaunchFiles)
                        {
                            var file = (StorageFile)item.Item;
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
            pbLoading.Visibility = Visibility.Visible;
            var fos = await(await BaseFolder.GetFoldersAsync()).ToStorageItemsArray();
            var fis = await(await BaseFolder.GetFilesAsync()).ToStorageItemsArray();
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
            var r=new ObservableCollection<StorageFolder>();
            for (int i = 0; i <= bcbFs.Count - 1; i++)
            {
                r.Add(bcbFs[bcbFs.Count - 1 - i]);
            }
            ItemsSource = new(l);
            pbLoading.Visibility = Visibility.Collapsed;
            BcBPath.ItemsSource = r;
            if (IsTopFolderButtonEnabled)
            {
                btnTopFolder.IsEnabled = await BaseFolder.GetParentAsync() != null;
            }
            txtEmpty.Visibility = ItemsSource.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
            this.IsEnabled = true;
        }
        private async void btnTopFolder_Click(object sender, RoutedEventArgs e)
        {
            if (await BaseFolder.GetParentAsync() != null)
                BaseFolder = await BaseFolder.GetParentAsync();
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
            if(f!= null)
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
                foreach (StorageItem item in agv.SelectedItems)
                {
                    await item.Item.DeleteAsync();
                }
                Refresh();
            }
        }
    }
}
