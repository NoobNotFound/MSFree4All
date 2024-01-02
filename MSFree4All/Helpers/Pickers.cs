using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MSFree4All.Helpers.Pickers
{
    public class FileOpenPicker
    {
        public event EventHandler<StorageFile>? FilePicked;
        private ContentDialog cd;
        private StorageFile file;
        public FileOpenPicker(string Title, StorageFolder currentFolder, StorageFolder root, string[] fileTypes)
        {
            var fop = new UserControls.FolderPicker(currentFolder)
            {
                RootFolder = root,
                SelectFilesOnlyIf = fileTypes.Any(),
                FileClickOnlyIf = fileTypes.Any(),
                SelectFiles = true,
                SelectFolders = false,
                LaunchFiles = false,
                SelectionMode = ListViewSelectionMode.Single
            };
            if (fileTypes.Any())
                foreach (var item in fileTypes)
                {
                    fop.SelectFilesOnlyIfType.Add(item);
                    fop.FileClickOnlyIfType.Add(item);
                }
            cd = fop.ToContentDialog(Title, "Cancel", ContentDialogButton.Primary);
            cd.Background = Application.Current.Resources["ContentDialogSolidBackground"] as SolidColorBrush;
            cd.PrimaryButtonText = "Select";
            cd.IsPrimaryButtonEnabled = false;
            fop.SelectionChanged += (_, _) =>
            {
                if (fop.SelectedItems.Any())
                {
                    file = (fop.SelectedItems[0].Item as StorageFile);
                    cd.IsPrimaryButtonEnabled = true;
                }
                else
                {
                    file = null;
                    cd.IsPrimaryButtonEnabled = false;
                }


            };
            fop.FileClicked += (_, e) =>
            {
                file = (e.Item as StorageFile);
                cd.Hide();
            };
        }

        public async Task<StorageFile> PickSingleFileAsync()
        {
            await cd.ShowAsync();
            return file;
        }
    }
}
