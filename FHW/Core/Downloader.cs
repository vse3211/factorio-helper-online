using BlazorBootstrap;
using System.Collections.Concurrent;
using System.Net;

namespace FHW.Core
{
    public class Downloader
    {
        public static Download? CurrentDownload;
        public static List<Download> Downloads { get; private set; } = new List<Download>();
        public static Queue<Download> DownloadsQueue { get; private set; } = new Queue<Download>();

        public static void AddQueue(Download _download)
        {
            DownloadsQueue.Enqueue(_download);
            if (CurrentDownload is null || CurrentDownload.DownloadState == 100) DownloadFile();
        }

        private static void DownloadFile()
        {
            if (DownloadsQueue.Count > 0)
            {
                CurrentDownload = DownloadsQueue.Dequeue();
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                    wc.DownloadFileAsync(
                        // Param1 = Link of file
                        new System.Uri(CurrentDownload.Uri),
                        // Param2 = Path to save
                        Path.Combine(Directory.GetCurrentDirectory(), "Files", CurrentDownload.FileName)
                    );
                }
            }
        }

        private static void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (CurrentDownload != null)
            {
                CurrentDownload.DownloadState = e.ProgressPercentage;
            }
        }

        private static void Wc_DownloadFileCompleted(object? sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            CurrentDownload.IsCompleted = !e.Cancelled;
            CurrentDownload.DownloadState = 100;
            Downloads.Add(CurrentDownload);
            DownloadFile();
        }

        

        public class Download
        {
            public string Uri { get; set; }
            public string Title { get; set; }
            public string FileName { get; set; }
            public int DownloadState { get; set; } = 0;
            public bool IsCompleted { get; set; } = false;
        }
    }
}
