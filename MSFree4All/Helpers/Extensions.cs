using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFree4All.Helpers
{
    public static class Extensions
    {
        public static ContentDialog ToContentDialog(this object content,string title,string closebtnText, ContentDialogButton defaultButton)
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
    }
}
