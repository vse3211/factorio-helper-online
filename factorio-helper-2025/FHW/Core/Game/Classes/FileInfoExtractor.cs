namespace FHW.Core.Game.Classes;

using System;
using System.Text.RegularExpressions;

public class FileInfoExtractor
{
    public string Filename { get; private set; }
    public string Component { get; private set; }
    public string Platform { get; private set; }
    public string Version { get; private set; }
    public string OldVersion { get; private set; }
    public string NewVersion { get; private set; }
    public bool IsUpdate { get; private set; }

    public FileInfoExtractor(string filename)
    {
        Filename = filename;
        ParseFilename();
    }

    private void ParseFilename()
    {
        // Шаблон для распознавания компонентов, платформ, и версий
        var regex = new Regex(@"(?<component>[a-zA-Z\-]+)?_?(?<platform>[a-zA-Z0-9]+)?(?:[-_](?<oldVersion>\d+\.\d+\.\d+))?(?:[-_](?<newVersion>\d+\.\d+\.\d+))?(?<updateFlag>-update)?");

        var match = regex.Match(Filename);

        if (match.Success)
        {
            Component = match.Groups["component"].Success ? match.Groups["component"].Value : "Unknown";
            Platform = match.Groups["platform"].Success ? match.Groups["platform"].Value : "Unknown";
            OldVersion = match.Groups["oldVersion"].Success ? match.Groups["oldVersion"].Value : "Version not found";
            NewVersion = match.Groups["newVersion"].Success ? match.Groups["newVersion"].Value : "Version not found";
            IsUpdate = match.Groups["updateFlag"].Success;
            Version = !IsUpdate && NewVersion != "Version not found" ? NewVersion : "Version not found";
        }
        else
        {
            Component = "Unknown";
            Platform = "Unknown";
            Version = "Version not found";
            OldVersion = "Version not found";
            NewVersion = "Version not found";
            IsUpdate = false;
        }
    }

    public override string ToString()
    {
        return $"Filename: {Filename}\nComponent: {Component}\nPlatform: {Platform}\nVersion: {Version}\nOld Version: {OldVersion}\nNew Version: {NewVersion}\nIs Update: {IsUpdate}";
    }
}

