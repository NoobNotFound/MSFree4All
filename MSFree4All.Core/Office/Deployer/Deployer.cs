using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSFree4All.Core.Office.Deployer.Enums;
using MSFree4All.Core;
using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

namespace MSFree4All.Core.Office.Deployer.EventArgs
{
    public class DownloadRequestedEventArgs : System.EventArgs
    {
        public string FileName { get; private set; }
        public double SizeInMB { get; private set; }
        public DownloadRequestedEventArgs(string fileName, double sizeInMB)
        {
            FileName = fileName;
            SizeInMB = sizeInMB;
        }
    }
}
namespace MSFree4All.Core.Office.Deployer
{
    
    public class Deployer
    {
        public const string SetupUrl = "https://raw.githubusercontent.com/Chaniru22/Modern-MSFree4All-Public/main/setup.exe";
        public string SetupPath { get; set; }
        public string BaseFolderPath { get; set; }

        public string ConfigurationPath { get => BaseFolderPath + "\\Configuration.xml"; }
        public string StartScriptPath { get => BaseFolderPath + "\\Start.bat"; }
        public string ISOScriptPath { get => BaseFolderPath + "\\ISOCreator.ps1"; }
        //public string BatchFilePath { get => BaseFolderPath + "\\run.bat"; }
        public string Destination { get; set; }
        public string SourcePath { get; set; }
        private bool IsIntialized = false;


        /// <summary>
        /// Sets the <see cref="Deployer.SetupPath"/> of the deployer and intialize. <paramref name="setupPath"/> can exist or not.
        /// </summary>
        public void Intialize(string setupPath,string basefolderPath)
        {
            if (string.IsNullOrEmpty(setupPath) || string.IsNullOrWhiteSpace(setupPath))
                return;

            if (string.IsNullOrEmpty(basefolderPath) || string.IsNullOrWhiteSpace(basefolderPath))
                return;

            BaseFolderPath = basefolderPath;
            SetupPath = setupPath;
            IsIntialized = true;
        }

        private bool IsSetupDownloading = false;
        /// <summary>
        /// Download or replace the Setup to <see cref="Deployer.SetupPath"/> from <see cref="Deployer.SetupUrl"/>
        /// </summary>
        /// <exception cref="System.InvalidOperationException"/>
        public async void DownloadSetup()
        {
            if (IsSetupDownloading)
                throw new InvalidOperationException("A setup is already downloading");

            var  i = Util.DownloadsManager.CreateDownloader(SetupUrl, SetupPath);
            var d = Util.DownloadsManager.GetDownloader(i);
            d.ProgressChanged += (a,b,c) =>
            {
                if (c >= 99 || a == b)
                {
                    IsSetupDownloading = false;
                }
            };
            IsSetupDownloading = true;
            await d.StartDownload();

        }

        /// <summary>
        /// Starts Deploy using the <paramref name="xML"/> and other infomation
        /// </summary>
        public async Task<StringErrorsList> Deploy(DeployType deployType, string xML,string ISOPath = null)
        {
            var errors = new StringErrorsList("Errors");
            if (!IsIntialized)
            {
                errors.Add("Deployer wasn't initialized.");
                return errors;
            }

            if (IsSetupDownloading)
                errors.Add("Setup is currently downloading");

            if(!string.IsNullOrEmpty(xML) && !string.IsNullOrEmpty(SetupPath))
            {
                if (File.Exists(SetupPath) && Util.GetFileSize(SetupPath) >= 8000000)
                {
                    await File.WriteAllTextAsync(ConfigurationPath, xML);
                    Process proc = new();
                    if(deployType == DeployType.Download)
                    {
                        proc.StartInfo = new(SetupPath)
                        {
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            Arguments = $"/download \"{ConfigurationPath}\""
                        };
                        Util.ProcessUtil.AddProcessAndStart(proc,"Office Downloader");
                    }
                    else if (deployType == DeployType.Configure)
                    {
                        proc.StartInfo = new(SetupPath)
                        {
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            Arguments = $"/configure \"{ConfigurationPath}\""
                        };
                        Util.ProcessUtil.AddProcessAndStart(proc,"Office Setup");
                    }
                    else if(deployType == DeployType.ISOFromMedia)
                    {
                        var script = Consts.ISOScript.Replace("{ISOPath}", ISOPath).Replace("{BasePath}", BaseFolderPath);
                        await File.WriteAllTextAsync(ISOScriptPath, script);
                        await File.WriteAllTextAsync(Path.Combine(BaseFolderPath,"RunMe.bat"), ".\\setup.exe /configure Configuration.xml");
                        proc.StartInfo = new("powershell.exe")
                        {
                            Arguments = $"-executionpolicy remotesigned -File \"{ISOScriptPath}\""
                        };
                        var u = Util.ProcessUtil.CreateUtilAndAddProcess(proc,"ISO Creator");
                        Util.ProcessUtil.OutputReceived += async (s, e) =>
                        {
                            if (e.ID == u.ID && e.Log.Message == "Exit")
                            {
                                File.Delete(ISOScriptPath);
                                File.Delete(BaseFolderPath + "\\RunMe.bat");
                                try
                                {
                                    _ = (await StorageFolder.GetFolderFromPathAsync("BaseFolderPath + \"\\\\Temp\"")).DeleteAsync();
                                }
                                catch { }
                            }
                        };
                        u.StartWithEvents();
                    }
                }
                else
                {
                    errors.Add("Invalid setup.exe was found");
                    return errors;
                }
            }
            return errors;
        }
    }
}
