using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFree4All.Core.Windows
{
    public class Activator
    {
        public string TempPath { get; set; }
        public string ScriptPath => Path.Combine(TempPath, "win.bat");
        public void Initialize(string tempPath) => TempPath = tempPath;
        public async Task<string> CheckStatusAsync()
        {
            await File.WriteAllTextAsync(ScriptPath, Consts.DLiCheck);
            var p = new Process()
            {
                StartInfo = new(ScriptPath)
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                }
            };
            var pu = new Util.ProcessUtil(p);
            await pu.StartWithEventsAsync(true, true);
            return pu.Output;
        }
        public async void GetDefaultKey()
        {
            await File.WriteAllTextAsync(ScriptPath, Consts.DLiGetDefaultKey);
            var p = new Process()
            {
                StartInfo = new(ScriptPath)
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                }
            };
            var pu = Util.ProcessUtil.CreateUtilAndAddProcess(p, "Get default Windows Digital License key");
            await pu.StartWithEventsAsync(true, true);
        }
        public async void ActivateWithKMS()
        {
            await File.WriteAllTextAsync(ScriptPath, Consts.ActKMS);
            var p = new Process()
            {
                StartInfo = new(ScriptPath)
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                }
            };
            var pu = Util.ProcessUtil.CreateUtilAndAddProcess(p, "Activate Windows (KMS)");
            await pu.StartWithEventsAsync(true, true);
        }
    }
}