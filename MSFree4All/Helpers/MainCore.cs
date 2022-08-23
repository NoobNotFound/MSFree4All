using MSFree4All.Core;
using MSFree4All.Core.Office;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFree4All.Helpers
{
    public static class MainCore
    {
        public static class Office
        {
            public static OfficeCore OfficeCore { get; set; } = new OfficeCore();
            public static int SelectedProductCount = 0;
        }
    }
}
