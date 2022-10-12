using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.IO;
using System.Collections.ObjectModel;

namespace MSFree4All.Core
{
    public static class Util
    {
        public static class DownloadsManager
        {
            private static int LastID = 0;
            public static ObservableCollection<FileDownloader> AllDownloaders { get; private set; } = new();
            /// <summary>
            /// A <see cref="HttpClient"/> downloader with progress
            /// </summary>
            public class FileDownloader : IDisposable
            {
                public delegate void FileDownloaderHandler(int ID);
                public int ID { get; private set; }
                public readonly string _downloadUrl;
                private readonly string _destinationFilePath;

                private HttpClient _httpClient;

                public delegate void ProgressChangedHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage);

                public event ProgressChangedHandler ProgressChanged;

                public FileDownloader(string downloadUrl, string destinationFilePath, int iD)
                {
                    _downloadUrl = downloadUrl;
                    _destinationFilePath = destinationFilePath;
                    ID = iD;
                }

                public async Task StartDownload()
                {
                    _httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };

                    using (var response = await _httpClient.GetAsync(_downloadUrl, HttpCompletionOption.ResponseHeadersRead))
                        await DownloadFileFromHttpResponseMessage(response);
                }

                private async Task DownloadFileFromHttpResponseMessage(HttpResponseMessage response)
                {
                    response.EnsureSuccessStatusCode();

                    var totalBytes = response.Content.Headers.ContentLength;

                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                        await ProcessContentStream(totalBytes, contentStream);
                }

                private async Task ProcessContentStream(long? totalDownloadSize, Stream contentStream)
                {
                    var totalBytesRead = 0L;
                    var readCount = 0L;
                    var buffer = new byte[8192];
                    var isMoreToRead = true;

                    using (var fileStream = new FileStream(_destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        do
                        {
                            var bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                            {
                                isMoreToRead = false;
                                TriggerProgressChanged(totalDownloadSize, totalBytesRead);
                                continue;
                            }

                            await fileStream.WriteAsync(buffer, 0, bytesRead);

                            totalBytesRead += bytesRead;
                            readCount += 1;

                            if (readCount % 100 == 0)
                                TriggerProgressChanged(totalDownloadSize, totalBytesRead);
                        }
                        while (isMoreToRead);
                    }
                }

                private void TriggerProgressChanged(long? totalDownloadSize, long totalBytesRead)
                {
                    if (ProgressChanged == null)
                        return;

                    double? progressPercentage = null;
                    if (totalDownloadSize.HasValue)
                        progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2);

                    ProgressChanged(totalDownloadSize, totalBytesRead, progressPercentage);
                }

                public void Dispose()
                {
                    _httpClient?.Dispose();
                }
            }

            public static event FileDownloader.FileDownloaderHandler? DownloaderAdded;
            public static event FileDownloader.FileDownloaderHandler? DownloaderRemoved;

            public static int CreateDownloader(string downloaPath,string savPath)
            {
                var d = new FileDownloader(downloaPath, savPath,LastID);
                LastID++;
                AllDownloaders.Add(d);
                DownloaderAdded.Invoke(d.ID);
                return d.ID;
            }
            public static FileDownloader GetDownloader(int ID)
            {
                return AllDownloaders.Where(x => x.ID == ID).FirstOrDefault();
            }
            public static void DisposeAndRemove(int ID)
            {
                var d = AllDownloaders.Where(x=> x.ID == ID).FirstOrDefault();
                try
                {
                    d.Dispose();
                    AllDownloaders.Remove(d);
                    DownloaderRemoved.Invoke(d.ID);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Get the all values of an <see cref="Enum"/>.
        /// </summary>
        public static List<T> GetEnumList<T>()
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            List<T> list = new(array);
            return list;
        }


        /// <summary>
        /// Gets the size in bytes of the given file using <see cref="System.IO.FileInfo"/>
        /// </summary>
        /// <returns>
        /// an <see cref="int"/>. May can be 0 if the file doesn't exist.
        /// </returns>
        public static int GetFileSize(string path)
        {
            try
            {
                return ((int)new System.IO.FileInfo(path).Length);
            }
            catch
            {
                return 0;
            }
        }
        public class ProcessUtil
        {
            public static event EventHandler<Office.Deployer.EventArgs.OutputReceivedEventArgs>? OutputReceived;
            public static event ProcessIDHandler? ProcessAdded;
            public static event ProcessIDHandler? ProcessRemoved;
            public event EventHandler<string>? outputReceived;
            public event EventHandler? Exited;

            public delegate void ProcessIDHandler(int id);
            public Process Process { get; private set; }

            public ProcessUtil(Process process)
            {
                this.Process = process;
            }
            private static int procCount = 0;
            private static List<(Process,int)> Processes = new();
            public static ProcessStartInfo GetProcessStartInfo(int ID)
            {
                try
                {
                    var proc = Processes.Where(x => x.Item2 == ID).FirstOrDefault();
                    return proc.Item1.StartInfo;
                }
                catch
                {
                    return new ProcessStartInfo();
                }
            }
            public static void StartProcess(int ID)
            {
                new WaitForExitHandler(ID).Start();
            }
            public static int AddProcessAndStart(Process proc)
            {
                var id = procCount;
                Processes.Add((proc, id));
                ProcessAdded?.Invoke(id);
                procCount++;
                new WaitForExitHandler(id).Start();
                return id;
            }
            private class WaitForExitHandler
            {
                public int ID;
                public WaitForExitHandler(int iD)
                {
                    ID = iD;
                }
                public void Start()
                {

                    System.Threading.Thread t = new System.Threading.Thread(SartWithWaitForExitAndInvoke);
                    t.Start();
                }
                private async void SartWithWaitForExitAndInvoke()
                {
                    var p = Processes.Where(x => x.Item2 == ID).FirstOrDefault().Item1;
                    p.Start();
                    await p.WaitForExitAsync();
                    
                    ProcessRemoved?.Invoke(ID);
                    Processes.Remove(Processes.Where(x => x.Item2 == ID).FirstOrDefault());
                }
            }
            private static int AddProcess(Process proc)
            {
                var id = procCount;
                Processes.Add((proc, id));
                ProcessAdded?.Invoke(id);
                procCount++;
                return id;
            }
            public static bool KillProcess(int ID)
            {
                try
                {
                    var proc = Processes.Where(x => x.Item2 == ID).FirstOrDefault();
                    proc.Item1.Kill();
                    Processes.Remove(proc);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            public static ProcessUtil CreateUtilAndAddProcess(Process proc)
            {
                var id = AddProcess(proc);
                var p = new ProcessUtil(Processes.Where(x => x.Item2 == id).FirstOrDefault().Item1);
                p.outputReceived += (_, e) => OutputReceived?.Invoke(p, new Office.Deployer.EventArgs.OutputReceivedEventArgs(Processes.Where(x => x.Item2 == id).FirstOrDefault().Item1.StartInfo.FileName, e));
                return p;
            }
            public static ProcessUtil CreateUtil(Process proc)
            {
                var p = new ProcessUtil(proc);
                p.outputReceived += (_, e) => OutputReceived?.Invoke(p, new Office.Deployer.EventArgs.OutputReceivedEventArgs(proc.StartInfo.FileName, e));
                return p;
            }
            public void StartWithEvents(bool isAdmin = false)
            {
                Process.StartInfo.CreateNoWindow = true;
                Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Process.StartInfo.UseShellExecute = false;
                Process.StartInfo.RedirectStandardError = true;
                if (isAdmin) { Process.StartInfo.Verb = "runas"; }
                Process.StartInfo.RedirectStandardOutput = true;
                Process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                Process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                Process.EnableRaisingEvents = true;
                Process.ErrorDataReceived += (s, e) => outputReceived?.Invoke(this, e.Data ?? "");
                Process.OutputDataReceived += (s, e) => outputReceived?.Invoke(this, e.Data ?? "");
                Process.Exited += (s, e) => Exited?.Invoke(this, new EventArgs());

                Process.Start();
                Process.BeginErrorReadLine();
                Process.BeginOutputReadLine();
            }

            public Task WaitForExitTaskAsync()
            {
                return Task.Run(() =>
                {
                    Process.WaitForExit();
                });
            }
        }
    }
}
namespace MSFree4All.Core.Office.Deployer.EventArgs
{
    public class OutputReceivedEventArgs : System.EventArgs
    {
        public string FileName { get;private set; }
        public string Log { get;private set; }
        public OutputReceivedEventArgs(string fileName,string log)
        {
            FileName = fileName;
            Log = log;
        }
    }
}