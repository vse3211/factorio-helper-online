using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using FHW.Components;
using FHW.Services;
using ModsList;

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
                isAvailableVersionUpdating = true;
                await UpdateAvailableVersions(FactorioService.Instance);
                isAvailableVersionUpdating = false;
            }
        }
    }), null, 0, 100);

    public static string PublicDownloadsDirertory => $"{Directory.GetCurrentDirectory()}/wwwroot/factorio";
    public static string? InstallationDirectiry => $"{Directory.GetCurrentDirectory()}/game";

    public static DateTimeOffset LastVersionsCheck = DateTimeOffset.MinValue;
    public static string BasicFileTag => "factorio";
    public static string ExtensionFileTag => "factorio-space-age";

    public static Classes.LatestVersions.Branch? AvailableVersions;
    public static bool isAvailableVersionUpdating;

    public static string? InstalledAlphaVersion { get; private set; }
    public static string? InstalledExtensionVersion { get; private set; }
    public static bool isInstalationInProgress;

    public static async Task UpdateAvailableVersions(FactorioService factorioService)
    {
        AvailableVersions = await factorioService.GetLatestVersionsAsync();
        LastVersionsCheck = DateTimeOffset.Now;
    }

    public static async Task GetSHA256Sums(FactorioService factorioService)
    {
        var sums = await factorioService.GetSHA256SumsAsync();
        if (sums is null)
        {
            System.Console.WriteLine("SHA256SUMS IS NULL");
            return;
        }
        sums.ForEach(x => Console.WriteLine(x));
    }

    public static void UpdateGame()
    {

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
