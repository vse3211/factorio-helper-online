using System;
using System.Diagnostics;

namespace FHW.Core.Game;

public class Controller
{
    private static Timer WatchDog = new(new TimerCallback(_ =>
    {

    }), null, 0, 100);

    public static string PublicGameDirertory => $"{Directory.GetCurrentDirectory()}/wwwroot/factorio";
    public static string? GameInstallationDirectiry => $"{Directory.GetCurrentDirectory()}/factorio";

    public static DateTimeOffset LastGameCheck = DateTimeOffset.MinValue;
    public static string GameFileTag => "factorio-space-age";
    public static string? InstalledGameVersion;
    public static string? AvailableGameVersion;
    public static string? AvailableExperimentalGameVersion;
    public static string? StoredStableGameVersion
    {
        get
        {
            return null;
        }
    }
    public static string? StoredExperimentalGameVersion
    {
        get
        {
            return null;
        }
    }
    public static bool isGameUpdating;
    public static bool isGameExtracting;

    public static void CheckGameVersion()
    {

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
