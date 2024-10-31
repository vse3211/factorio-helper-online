using System.Diagnostics;

namespace FHW.Core;

public class CurlDownloader
{
    public bool IsDownloading {get; private set;} = false;
    public bool IsDownloadingCompletedOrError {get; private set;} = false;
    public string DownloadOutput {get; private set;} = "Class initialized\n";

    public async void DownloadFileTask(string TargetPath, string uri)
    {
        if (IsDownloading || IsDownloadingCompletedOrError) return;
        IsDownloading = true;
        DownloadOutput = $"{DateTime.UtcNow} | Начинаем загрузку...";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/sh",
                Arguments = $"-c \"curl -o {TargetPath} {uri}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.OutputDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                DownloadOutput += args.Data + "\n";
            }
        };

        process.ErrorDataReceived += (sender, args) =>
        {
            if (!string.IsNullOrEmpty(args.Data))
            {
                DownloadOutput += $"{DateTime.UtcNow} | Error: " + args.Data + "\n";
                IsDownloadingCompletedOrError = true;
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        await process.WaitForExitAsync();

        IsDownloadingCompletedOrError = true;
        IsDownloading = false;
        DownloadOutput += "Загрузка завершена.\n";
    }
}
