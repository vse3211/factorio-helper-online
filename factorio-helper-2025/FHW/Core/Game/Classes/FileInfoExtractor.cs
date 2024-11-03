namespace FHW.Core.Game.Classes;

using System.Text.RegularExpressions;

public class FileInfoExtractor
{
    public string Hash { get; private set; }
    public string Filename { get; private set; }
    public string? Extension { get; private set; }
    public string? Component { get; private set; }
    public string? Platform { get; private set; }
    public string? Version { get => OldVersion ?? NewVersion; }
    public string? OldVersion { get; private set; }
    public string? NewVersion { get; private set; }
    public bool IsUpdate { get; private set; }

    public FileInfoExtractor(string hash, string filename)
    {
        Hash = hash;
        Filename = filename;
        ParseFilename();
    }

    private void ParseFilename()
    {
        Regex regex;
        if (Filename.Contains("-update"))
        {
            regex = new(@"^(?<component>[A-Za-z0-9_]+)[-](?<platform>[A-Za-z0-9_]+)[-](?<oldVersion>[0-9.]+)[-](?<newVersion>[0-9.]+)[-](?<updateFlag>[A-Za-z0-9]+)\.(?<extension>[a-z.]+)$");
            var match = regex.Match(Filename);
            if (match.Success)
            {
                Extension = match.Groups["extension"].Success ? match.Groups["extension"].Value : null;
                Component = match.Groups["component"].Success ? match.Groups["component"].Value : null;
                Platform = match.Groups["platform"].Success ? match.Groups["platform"].Value : null;
                OldVersion = match.Groups["oldVersion"].Success ? match.Groups["oldVersion"].Value : null;
                NewVersion = match.Groups["newVersion"].Success ? match.Groups["newVersion"].Value : null;
                IsUpdate = match.Groups["updateFlag"].Success;
            }
        }
        else
        {
            regex = new(@"^(?<component>[A-Za-z0-9-]+)[_](?<platform>[A-Za-z0-9-]+)[_](?<oldVersion>[0-9.]+)\.(?<extension>[a-z.]+)$");
            var match = regex.Match(Filename);
            if (match.Success)
            {
                Extension = match.Groups["extension"].Success ? match.Groups["extension"].Value : null;
                if (Filename.Contains("Setup_"))
                {
                    Component = match.Groups["platform"].Success ? match.Groups["platform"].Value : null;
                    Platform = "win64";
                }
                else
                {
                    Component = match.Groups["component"].Success ? match.Groups["component"].Value : null;
                    Platform = match.Groups["platform"].Success ? match.Groups["platform"].Value : null;
                }
                OldVersion = match.Groups["oldVersion"].Success ? match.Groups["oldVersion"].Value : null;
                IsUpdate = false;
            }
        }
    }

    public override string ToString()
    {
        return $"--------------\nHash : {Hash}\nFilename: {Filename}\nExtension: {Extension}\nComponent: {Component}\nPlatform: {Platform}\nVersion: {Version}\nOld Version: {OldVersion}\nNew Version: {NewVersion}\nIs Update: {IsUpdate}";
    }
}

