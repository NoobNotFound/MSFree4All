using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFree4All.Core.Windows
{
    public class WindowsCore
    {
        /// <summary>
        /// The KMS and digital activators
        /// </summary>
        public Windows.Activator Activator { get; private set; } = new();

        /// <summary>
        /// The Fido ISO downloader
        /// </summary>
        public Windows.Downloader Downloader { get; private set; } = new();

        /// <summary>
        /// The Insider settings changer
        /// </summary>
        public Windows.Insider Insider { get; private set; } = new();
    }
}
