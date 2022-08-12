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


namespace MSFree4All.UserControls
{
    /// <summary>
    /// A strings list control with bullets.
    /// </summary>
    public sealed partial class BulletsList : ItemsControl
    {
        public BulletsList()
        {

        }
        public BulletsList(IEnumerable<string> strings)
        {
            this.InitializeComponent();
            this.ItemsSource = strings;
        }
    }
}
