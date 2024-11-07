using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using FHW.Components;
using FHW.Services;

namespace FHW.Core.Game;

public class Controller
{
    private static DateTimeOffset GlobalLastRefresh = DateTimeOffset.MinValue;
    private static int failedTryCount = 0;
    public static Timer? WatchDog { get; private set; } = new(new TimerCallback(async _ =>
    {
        if (DateTimeOffset.Now - GlobalLastRefresh > TimeSpan.FromSeconds((failedTryCount + 1) * 10))
        {
            if (failedTryCount > 5)
            {
                System.Console.WriteLine("Application cant work!");
                await WatchDog!.DisposeAsync();
                WatchDog = null;
                return;
            }
            if (FactorioService.Instance is null)
            {
                System.Console.WriteLine($"{DateTime.UtcNow} factorioService is NULL!");
                GlobalLastRefresh = DateTimeOffset.Now;
                failedTryCount++;
                return;
            }
            if (FactorioUpdateService.Instance is null)
            {
                System.Console.WriteLine($"{DateTime.UtcNow} factorioUpdateService is NULL!");
                GlobalLastRefresh = DateTimeOffset.Now;
                failedTryCount++;
                return;
            }
            if (!isAvailableVersionUpdating && DateTimeOffset.Now - LastVersionsCheck > TimeSpan.FromMinutes(5))
            {
                await UpdateAvailableVersions(FactorioService.Instance);
            }
            if (!isInstalationInProgress && DateTimeOffset.Now - lastGameUpdate > TimeSpan.FromMinutes(1))
            {
                await CheckGameUpdates(FactorioService.Instance);
            }
        }
    }), null, 0, 100);
    public static string? Username = Environment.GetEnvironmentVariable("FACTORIO_USERNAME");
    public static string? Token = Environment.GetEnvironmentVariable("FACTORIO_TOKEN");

    public static string PublicDownloadsDirertory => $"{Directory.GetCurrentDirectory()}/wwwroot/factorio";
    public static string? InstallationDirectiry => $"{Directory.GetCurrentDirectory()}/game";

    public static DateTimeOffset LastVersionsCheck = DateTimeOffset.MinValue;
    public static string BasicFileTag => "factorio";
    public static string ExtensionFileTag => "factorio-space-age";

    public static Classes.LatestVersions.Branch? AvailableVersions;
    public static bool isAvailableVersionUpdating;

    public static async Task UpdateAvailableVersions(FactorioService factorioService)
    {
        isAvailableVersionUpdating = true;
        AvailableVersions = await factorioService.GetLatestVersionsAsync();
        LastVersionsCheck = DateTimeOffset.Now;
        isAvailableVersionUpdating = false;
    }

    public static List<Classes.FileInfoExtractor>? sha256Sums;
    public static async Task PrintSHA256Sums(FactorioService factorioService)
    {
        var sums = await factorioService.GetSHA256SumsAsync();
        if (sums is null)
        {
            System.Console.WriteLine("SHA256SUMS IS NULL");
            return;
        }
        sums.ForEach(x => Console.WriteLine(x));
    }
    public static async Task GetSHA256Sums(FactorioService factorioService)
    {
        sha256Sums = await factorioService.GetSHA256SumsAsync();
    }

    public static string? InstalledExtensionVersion { 
        get
        {
            string? lastVersion = null;
            Directory.GetFiles(PublicDownloadsDirertory).ToList().ForEach(x =>
            {
                Classes.FileInfoExtractor xInfo = new(String.Empty, Path.GetFileName(x));
                if (!xInfo.IsUpdate)
                {
                    if (xInfo.Version is null) return;
                    string[] xVersionArray = xInfo.Version.Split('.');
                    if (String.IsNullOrEmpty(lastVersion))
                    {
                        lastVersion = x;
                        return;
                    }
                    string[] lastVersionArray = lastVersion.Split('.');
                    for(int i = 0; i < xVersionArray.Length; i++)
                    {
                        if (Convert.ToInt32(xVersionArray[i]) > Convert.ToInt32(lastVersionArray[i]))
                        {
                            lastVersion = x;
                            return;
                        }
                    }
                }
            });
            return lastVersion;
        }
    }
    public static DateTimeOffset lastGameUpdate = DateTimeOffset.MinValue;
    public static bool isInstalationInProgress;
    public static int? downloadingProgress;
    public static async Task CheckGameUpdates(FactorioService factorioService)
    {
        isInstalationInProgress = true;
        if (!Directory.Exists($"{PublicDownloadsDirertory}/temp")) Directory.CreateDirectory($"{PublicDownloadsDirertory}/temp");
        bool isExistsLatestStable = false;
        Directory.GetFiles(PublicDownloadsDirertory).ToList().ForEach(x =>
        {
            if (Path.GetExtension(x) != ".txt")
                if (new Classes.FileInfoExtractor(String.Empty, x).Version == AvailableVersions.Stable.Expansion)
                    isExistsLatestStable = true;
        });
        if (!isExistsLatestStable)
        {
            await factorioService.DownloadGame(
                version: AvailableVersions!.Stable.Expansion,
                type: "expansion",
                platform: "win64-manual",
                destinationFolder: $"{PublicDownloadsDirertory}/temp",
                username: Username,
                token: Token);
            downloadingProgress = null;
            while (Directory.GetFiles($"{PublicDownloadsDirertory}/temp").Count() > 0)
            {
                string oldFile = Directory.GetFiles($"{PublicDownloadsDirertory}/temp").First();
                File.Move(oldFile, $"{PublicDownloadsDirertory}/{Path.GetFileName(oldFile)}");
            }
            Directory.GetFiles(PublicDownloadsDirertory).ToList().ForEach(x =>
            {
                Classes.FileInfoExtractor xInfo = new(String.Empty, Path.GetFileName(x));
                if ((!xInfo.IsUpdate && xInfo.Version != AvailableVersions.Stable.Expansion)
                || (xInfo.IsUpdate && xInfo.NewVersion != AvailableVersions.Stable.Expansion))
                    File.Delete(x);
            });
        }
        lastGameUpdate = DateTimeOffset.Now;
        isInstalationInProgress = false;
    }

    public static void UnzipGame()
    {
    }

    public static void RemoveInstalledGame()
    {
    }

    public static void CleanPublicGameFolder()
    {
    }
}
