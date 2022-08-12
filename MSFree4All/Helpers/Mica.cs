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
using System.Runtime.InteropServices; // For DllImport
using WinRT;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Composition;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
namespace MSFree4All.Helpers
{
    public class WindowsSystemDispatcherQueueHelper
    {
        private object? _dispatcherQueueController;

        [StructLayout(LayoutKind.Sequential)]
        internal struct DispatcherQueueOptions
        {
            internal int dwSize;
            internal int threadType;
            internal int apartmentType;
        }

        [DllImport("CoreMessaging.dll")]
        private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object? dispatcherQueueController);

        public void EnsureWindowsSystemDispatcherQueueController()
        {
            if (Windows.System.DispatcherQueue.GetForCurrentThread() != null)
            {
                // one already exists, so we'll just use it.
                return;
            }

            if (_dispatcherQueueController == null)
            {
                DispatcherQueueOptions options;
                options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
                options.threadType = 2;
                options.apartmentType = 2;

                CreateDispatcherQueueController(options, ref _dispatcherQueueController);
            }
        }
    }
    public class MicaBackground
    {
        private readonly Window _window;
        private MicaController _micaController = new();
        private SystemBackdropConfiguration _backdropConfiguration = new();
        private readonly WindowsSystemDispatcherQueueHelper _dispatcherQueueHelper = new();

        public MicaBackground(Window window)
        {
            _window = window;
        }

        public bool TrySetMicaBackdrop()
        {
            if (MicaController.IsSupported())
            {
                _dispatcherQueueHelper.EnsureWindowsSystemDispatcherQueueController();
                _window.Activated += WindowOnActivated;
                _window.Closed += WindowOnClosed;
                ((FrameworkElement)_window.Content).ActualThemeChanged += MicaBackground_ActualThemeChanged;
                _backdropConfiguration.IsInputActive = true;
                _backdropConfiguration.Theme = _window.Content switch
                {
                    FrameworkElement { ActualTheme: ElementTheme.Dark } => SystemBackdropTheme.Dark,
                    FrameworkElement { ActualTheme: ElementTheme.Light } => SystemBackdropTheme.Light,
                    FrameworkElement { ActualTheme: ElementTheme.Default } => SystemBackdropTheme.Default,
                    _ => throw new InvalidOperationException("Unknown theme")
                };

                _micaController.AddSystemBackdropTarget(_window.As<ICompositionSupportsSystemBackdrop>());
                _micaController.SetSystemBackdropConfiguration(_backdropConfiguration);
                return true;
            }

            return false;
        }

        private void MicaBackground_ActualThemeChanged(FrameworkElement sender, object args)
        {
            if (_backdropConfiguration != null)
            {
                SetConfigurationSourceTheme();
            }

        }
        private void SetConfigurationSourceTheme()
        {
            switch (((FrameworkElement)_window.Content).ActualTheme)
            {
                case ElementTheme.Dark: _backdropConfiguration.Theme = SystemBackdropTheme.Dark; break;
                case ElementTheme.Light: _backdropConfiguration.Theme = SystemBackdropTheme.Light; break;
                case ElementTheme.Default: _backdropConfiguration.Theme = SystemBackdropTheme.Default; break;
            }
        }
        private void WindowOnClosed(object sender, WindowEventArgs args)
        {
            _micaController.Dispose();
            _micaController = null!;
            _window.Activated -= WindowOnActivated;
            _backdropConfiguration = null!;
        }

        private void WindowOnActivated(object sender, WindowActivatedEventArgs args)
        {
            _backdropConfiguration.IsInputActive = args.WindowActivationState is not WindowActivationState.Deactivated;
        }
    }
}
