namespace FHW.Core.Game.Classes;
using System;
using System.Text.RegularExpressions;

public class FactorioFileInfo
{
    public string Hash { get; set; }
    public string FileName { get; set; }
    public string Platform { get; private set; }
    public string Component { get; private set; }
    public string Version { get; private set; }
    public string OldVersion { get; private set; }
    public string NewVersion { get; private set; }
    public bool IsUpdate { get; private set; }

    public FactorioFileInfo(string hash, string fileName)
    {
        Hash = hash;
        FileName = fileName;
        ParseFileName();
    }

    private void ParseFileName()
    {
        // Example patterns for recognizing versions and components in file names
        var updatePattern = new Regex(@"(\w+)-(\w+)-(\d+\.\d+\.\d+)-(\d+\.\d+\.\d+)-update");
        var versionPattern = new Regex(@"(\w+)_([a-z]+)_(\d+\.\d+\.\d+)");
        
        // Check if the file is an update
        var updateMatch = updatePattern.Match(FileName);
        if (updateMatch.Success)
        {
            Component = updateMatch.Groups[1].Value;
            Platform = updateMatch.Groups[2].Value;
            OldVersion = updateMatch.Groups[3].Value;
            NewVersion = updateMatch.Groups[4].Value;
            IsUpdate = true;
        }
        else
        {
            // Check if it's a regular versioned file
            var versionMatch = versionPattern.Match(FileName);
            if (versionMatch.Success)
            {
                Component = versionMatch.Groups[1].Value;
                Platform = versionMatch.Groups[2].Value;
                Version = versionMatch.Groups[3].Value;
                IsUpdate = false;
            }
            else
            {
                Component = "Unknown";
                Platform = "Unknown";
                Version = "Version not found";
                OldVersion = "Version not found";
                NewVersion = "Version not found";
            }
        }
    }

    public override string ToString()
    {
        return $"Hash: {Hash}\nFileName: {FileName}\nComponent: {Component}\nPlatform: {Platform}\n" +
               $"Version: {Version}\nOld Version: {OldVersion}\nNew Version: {NewVersion}\nIs Update: {IsUpdate}";
    }
}
