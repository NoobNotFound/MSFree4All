﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class BulletsList : ItemsControl, INotifyPropertyChanged
    {
        private bool wordWrap = true;
        public bool WordWrap { get { return wordWrap; } set { wordWrap = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WordWrap))); } }
        public BulletsList()
        {
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class BulletsListItemTemplateSelector : DataTemplateSelector
    {
        public bool WordWrap { get; set; } = true;
        public DataTemplate WrappedText { get; set; }
        public DataTemplate NoWrapText { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (WordWrap)
                return WrappedText;
            else
                return NoWrapText;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (WordWrap)
                return WrappedText;
            else
                return NoWrapText;
        }

    }
}