using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFree4All.Core.Office
{
    public class Activator
    {
        public string TempPath { get; set; }
        public string ActPath => System.IO.Path.Combine(TempPath, "act.bat");
        public void Initialize(string tempPath) =>
            TempPath = tempPath;
        
        private async Task CreateFile(string script)
        {
            await File.WriteAllTextAsync(ActPath, script);
        }
        public async Task Activate2016()
        {
            await CreateFile(Consts.O2016cript);
            Process proc = new()
            {
                StartInfo = new(ActPath)
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            var u = Util.ProcessUtil.CreateUtilAndAddProcess(proc, "Office Activator (2016 KMS)");
            u.StartWithEvents(true, true);
        }
        public async Task Activate2019()
        {
            await CreateFile(Consts.O2019cript);
            Process proc = new()
            {
                StartInfo = new(ActPath)
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            var u = Util.ProcessUtil.CreateUtilAndAddProcess(proc, "Office Activator (2019 KMS)");
            u.StartWithEvents(true, true);
        }
        public async Task Deactivate()
        {
            await CreateFile(Consts.ODeactivate);
            Process proc = new()
            {
                StartInfo = new(ActPath)
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            var u = Util.ProcessUtil.CreateUtilAndAddProcess(proc, "Deactivate Office (only KMS)");
            u.StartWithEvents(true, true);
        }
    }
}
