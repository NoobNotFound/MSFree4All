using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSFree4All.Core.Office.Deployer.Enums;
using MSFree4All.Core;
using System.Diagnostics;

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
        //public string BatchFilePath { get => BaseFolderPath + "\\run.bat"; }
        public string Destination { get; set; }
        public string SourcePath { get; set; }
        private bool IsIntialized = false;


        /// <summary>
        /// Sets the <see cref="Deployer.SetupPath"/> of the deployer and intialize. <paramref name="setupPath"/> can be exist or not.
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
        public async Task<StringErrorsList> Deploy(DeployType deployType, string xML)
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
                    if(deployType == DeployType.CreateMedia)
                    {
                        proc.StartInfo = new(SetupPath)
                        {
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            Arguments = $"/download \"{ConfigurationPath}\""
                        };
                        Util.ProcessUtil.AddProcessAndStart(proc);
                    }
                    else if(deployType == DeployType.InstallFromMedia)
                    {

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
