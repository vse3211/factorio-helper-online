using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Hosting.Internal;

namespace FHW.Services;

public sealed class FactorioService
{
    public static FactorioService? Instance { get; private set; }
    private readonly HttpClient _client;

    public FactorioService(HttpClient client)
    {
        _client = client;
        if (Instance is null) Instance = this;
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

    public async Task DownloadGame(string version, string type, string platform, string destinationFolder, string? username, string? token)
    {
        if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(token)) throw new ArgumentNullException("No username or token!");
        if (!Directory.Exists(destinationFolder)) throw new Exception("Path not exists!");
        string url = $"get-download/{version}/{type}/{platform}?username={username}&token={token}";
        
        HttpResponseMessage response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        var finalUrl = response.RequestMessage!.RequestUri!.ToString();
        string? fileName = GetFileNameFromContentDisposition(response);
        if (string.IsNullOrEmpty(fileName)) fileName = Path.GetFileName(finalUrl);
        fileName = fileName.Split('?')[0];
        string filePath = Path.Combine(destinationFolder, fileName);
        var totalBytes = response.Content.Headers.ContentLength ?? -1L;
        var totalBytesRead = 0L;
        var buffer = new byte[8192];
        int bytesRead;
        using (var stream = await response.Content.ReadAsStreamAsync())
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await fileStream.WriteAsync(buffer, 0, bytesRead);
                totalBytesRead += bytesRead;

                // Вычисляем прогресс и передаем его через IProgress
                if (totalBytes != -1)
                {
                    double percentComplete = (double)totalBytesRead / totalBytes * 100;
                    Core.Game.Controller.downloadingProgress = Convert.ToInt32(percentComplete);
                }
            }
        }
        System.Console.WriteLine("SUCCESS: Downloading game completed!");
    }

    private string? GetFileNameFromContentDisposition(HttpResponseMessage response)
    {
        if (response.Content.Headers.ContentDisposition != null)
        {
            return response.Content.Headers.ContentDisposition.FileName?.Trim('"');
        }
        return null;
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
