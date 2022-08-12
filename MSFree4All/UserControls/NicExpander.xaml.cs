using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MSFree4All.Enums;
using MSFree4All.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MSFree4All.UserControls
{
    [ContentProperty(Name = "Controls")]
    public sealed partial class NicExpander : UserControl
    {
        private readonly ExpanderViewModel ViewModel = new();
        public NicExpander()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// <inheritdoc cref="ExpanderViewModel.Title"/>
        /// </summary>
        public string Title
        {
            get => ViewModel.Title;
            set => ViewModel.Title = value;
        }

        /// <summary>
        /// <inheritdoc cref="ExpanderViewModel.Description"/>
        /// </summary>
        public string Description
        {
            get => ViewModel.Description;
            set => ViewModel.Description = value;
        }

        /// <summary>
        /// <inheritdoc cref="ExpanderViewModel.ExpanderStyle"/>
        /// </summary>
        public ExpanderStyles ExpanderStyle
        {
            get => ViewModel.ExpanderStyle;
            set => ViewModel.ExpanderStyle = value;
        }

        /// <summary>
        /// <inheritdoc cref="ExpanderViewModel.Icon"/>
        /// </summary>
        public string Icon
        {
            get => ViewModel.Icon;
            set => ViewModel.Icon = value;
        }

        /// <summary>
        /// <inheritdoc cref="ExpanderViewModel.Controls"/>
        /// </summary>
        public object Controls
        {
            get => ViewModel.Controls;
            set => ViewModel.Controls = value;
        }

        /// <summary>
        /// <inheritdoc cref="ExpanderViewModel.HeaderControls"/>
        /// </summary>
        public object HeaderControls
        {
            get => ViewModel.HeaderControls;
            set => ViewModel.HeaderControls = value;
        }

        public event RoutedEventHandler Click;
        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
    public class ExpanderTemplateSelector : DataTemplateSelector
    {
        public ExpanderStyles Style { get; set; }

        public DataTemplate Default { get; set; }
        public DataTemplate Static { get; set; }
        public DataTemplate Button { get; set; }
        public DataTemplate Transparent { get; set; }
        public DataTemplate Disabled { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            switch (Style)
            {
                case ExpanderStyles.Static:
                    return Static;

                case ExpanderStyles.Button:
                    return Button;

                case ExpanderStyles.Transparent:
                    return Transparent;

                case ExpanderStyles.Disabled:
                    return Disabled;

                default:
                    return Default;
            }
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            switch (Style)
            {
                case ExpanderStyles.Static:
                    return Static;

                case ExpanderStyles.Button:
                    return Button;

                case ExpanderStyles.Transparent:
                    return Transparent;

                case ExpanderStyles.Disabled:
                    return Disabled;

                default:
                    return Default;
            }
        }
    }
    // https://social.msdn.microsoft.com/Forums/windowsapps/en-US/b2e0f948-df35-49da-a70f-1892205b8570/contenttemplateselector-datatemplateselectorselecttemplatecore-item-parameter-is-always-null?forum=winappswithcsharp

    /// <summary>
    /// A version of the ContentControl that works with the ContentTemplateSelector.
    /// </summary>
    public class CompositionControl : ContentControl
    {
        /// <summary>
        /// Invoked when the value of the Content property changes. 
        /// </summary>
        /// <param name="oldContent">The old value of the Content property.</param>
        /// <param name="newContent">The new value of the Content property.</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            // There is a bug in the standard content control that trashes the value passed into the SelectTemplateCore method.  This is a
            // work-around that allows the same basic structure and can hopefully be replaced when the bug is fixed.  Basically take the new content
            // and figure out what template should be used with it based on the structure of the template selector.
            if (ContentTemplateSelector is DataTemplateSelector dataTemplateSelector)
            {
                ContentTemplate = dataTemplateSelector.SelectTemplate(newContent, null);
            }

            // Allow the base class to handle the rest of the call.
            base.OnContentChanged(oldContent, newContent);
        }
    }
}
