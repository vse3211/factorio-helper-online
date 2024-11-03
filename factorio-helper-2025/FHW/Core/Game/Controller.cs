using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

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

    public static void CheckGameVersionOld()
    {
        System.Console.WriteLine("Start CheckGameVersion");
        string? sha256data = LMC.Web.GetString("https://factorio.com/download/sha256sums/");
        if (sha256data is null) return;

        //factorioUpdateService.GetAvailablePackages();


        List<string[]> sha256list = GetHashFileList(sha256data);

        foreach (var item in sha256list)
        {
            string hash = item[0];
            string filename = item[1];
            (string? oldVersion, string? newVersion) = ExtractVersion(filename);
            Console.WriteLine($"\n\nHash: {hash}, Filename: {filename}, Old Version: {oldVersion}, New Version: {newVersion}");

            Classes.FileInfoExtractor fileInfo = new(hash, filename);
            System.Console.WriteLine(fileInfo);
        }
    }

    public static async Task CheckGameVersion(FHW.Services.FactorioService factorioService)
    {
        var sums = await factorioService.GetSHA256Sums();
        if (sums is null)
        {
            System.Console.WriteLine("SHA256SUMS IS NULL");
            return;
        }
        sums.ForEach(x => Console.WriteLine(x));
    }

    static List<string[]> GetHashFileList(string response)
    {
        List<string[]> hashFileList = new List<string[]>();

        // Разбиваем полученные данные на строки
        string[] lines = response.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            // Разделяем строку на хеш и имя файла (используем пробел как разделитель)
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                string hash = parts[0];
                string filename = parts[1];
                hashFileList.Add(new[] { hash, filename });
            }
        }

        return hashFileList;
    }

    static (string? oldVersion, string? newVersion) ExtractVersion(string filename)
    {
        // Паттерн для поиска одной или двух версий в формате X.Y.Z
        var match = Regex.Match(filename, @"(?:_|-)(\d+\.\d+\.\d+)(?:-(\d+\.\d+\.\d+))?");

        if (match.Success)
        {
            string oldVersion = match.Groups[1].Value;
            string? newVersion = match.Groups[2].Success ? match.Groups[2].Value : null;
            return (oldVersion, newVersion);
        }

        return (null, null);
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
