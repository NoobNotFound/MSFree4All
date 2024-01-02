using MSFree4All.Core;
using MSFree4All.Core.Office;
using MSFree4All.Core.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFree4All.Helpers
{
    public static class MainCore
    {
        public static OfficeCore OfficeCore { get; set; } = new();
        public static int SelectedProductCount = 0;

        public static WindowsCore WindowsCore { get; set; } = new();
    }
}
