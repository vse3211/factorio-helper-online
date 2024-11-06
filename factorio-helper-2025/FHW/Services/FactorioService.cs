using System.Text.RegularExpressions;

namespace FHW.Services;

public sealed class FactorioService
{
    private readonly HttpClient _client;

    public FactorioService(HttpClient client)
    {
        _client = client;
    }

    public async Task<Core.Game.Classes.LatestVersions.Branch?> GetLatestVersionsAsync()
    {
        Core.Game.Classes.LatestVersions.Branch? result = await _client.GetFromJsonAsync<Core.Game.Classes.LatestVersions.Branch>("api/latest-releases");
        return result;
    }

    public async Task<List<Core.Game.Classes.FileInfoExtractor>?> GetSHA256SumsAsync()
    {
        string? sha256data = await _client.GetStringAsync($"download/sha256sums");
        if (sha256data is null) return null;
        List<string[]> sha256list = GetHashFileList(sha256data);
        List<Core.Game.Classes.FileInfoExtractor> content = new();
        foreach (var item in sha256list)
        {
            string hash = item[0];
            string filename = item[1];
            (string? oldVersion, string? newVersion) = ExtractVersion(filename);
            Core.Game.Classes.FileInfoExtractor fileInfo = new(hash, filename);
            if (fileInfo != null) content.Add(fileInfo);
        }
        return content;
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
        var match = Regex.Match(filename, @"(?:_|-)(\d+\.\d+\.\d+)(?:-(\d+\.\d+\.\d+))?");
        if (match.Success)
        {
            string oldVersion = match.Groups[1].Value;
            string? newVersion = match.Groups[2].Success ? match.Groups[2].Value : null;
            return (oldVersion, newVersion);
        }
        return (null, null);
    }
}
