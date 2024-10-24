﻿using Newtonsoft.Json.Linq;
using System.Text;
/// <summary>
/// Sysytem classes
/// </summary>
namespace LMC
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    public class Blueprints
    {
        //public ImageSource Thumbnail { get; set; }
        public string Title { get; set; }
        public string FactorioVersion { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
    }
    public class Saves
    {
        //public ImageSource Thumbnail { get; set; }
        public string Title { get; set; }
        public string FactorioVersion { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
    }
    public class Builds
    {
        //public ImageSource Thumbnail { get; set; }
        public string Title { get; set; }
        public string FactorioVersion { get; set; }
        public string Mods { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
    }
    public class Pages
    {
        public string Page { get; set; }
        public int Pos { get; set; }
    }

    public class Web
    {
        public static string GetString(string uri)
        {
            WebRequest request = WebRequest.Create(uri);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            return reader.ReadToEnd();
        }
    }
}

namespace Mod
{
    public class Info
    {
        public ModObj.Mod Mod { get; set; }
        public long LastUpdate { get; set; }
    }

    public class Dependency
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Requirements { get; set; }
    }
}

namespace Client
{
    public class DesktopInfo
    {
        public string Name { get; set; }
        public string LocalIP { get; set; }
        public string PublicIP { get; set; }
        public long LastActivity { get; set; } = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
    }

    public class DesktopTask
    {
        public string Id { get; set; }
        public TaskType Type { get; set; }
        public List<TaskInfo> Info { get; set; } = new List<TaskInfo>();
        public long Created { get; set; } = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        public long LastActivity { get; set; } = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
    }

    public enum TaskType
    {
        DownloadMod = 10,
        RemoveMod = 11,
        EnableMod = 12,
        DisableMod = 13,
        DownloadGame = 21,
        RemoveGame = 22,
        SelectGameVersion = 23
    }

    public class TaskInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Link { get; set; }
    }
}


namespace Factorio.Tools
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using ComponentAce.Compression.Libs.zlib;

    public class Blueprint
    {
        private Blueprint() { }

        public static string Decode(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64)) return "";
            var enc = new UTF8Encoding();
            return enc.GetString(Decompress(Convert.FromBase64String(base64.Substring(1))));
        }

        public static string Encode(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return "";
            var enc = new UTF8Encoding();
            return "0" + Convert.ToBase64String(Compress(enc.GetBytes(json)));
        }

        private static byte[] Compress(byte[] inData)
        {
            using (MemoryStream outMemoryStream = new MemoryStream())
            using (ZOutputStream outZStream = new ZOutputStream(outMemoryStream, zlibConst.Z_DEFAULT_COMPRESSION))
            using (Stream inMemoryStream = new MemoryStream(inData))
            {
                CopyStream(inMemoryStream, outZStream);
                outZStream.finish();
                return outMemoryStream.ToArray();
            }
        }

        private static byte[] Decompress(byte[] inData)
        {
            using (MemoryStream outMemoryStream = new MemoryStream())
            using (ZOutputStream outZStream = new ZOutputStream(outMemoryStream))
            using (Stream inMemoryStream = new MemoryStream(inData))
            {
                CopyStream(inMemoryStream, outZStream);
                outZStream.finish();
                return outMemoryStream.ToArray();
            }
        }

        private static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        public static string Mirror(string json, bool vertical)
        {
            var o = JObject.Parse(json);
            var entities = o["blueprint"]["entities"];
            foreach (var entity in entities)
            {
                if (entity["direction"] == null)
                    entity["direction"] = 0;
                int d = entity["direction"].Value<int>();
                switch (d)
                {
                    case 0: d = vertical ? 4 : 0; break;
                    case 4: d = vertical ? 0 : 4; break;
                    case 2: d = vertical ? 2 : 6; break;
                    case 6: d = vertical ? 6 : 2; break;
                }
                entity["direction"] = d;

                if (vertical)
                {
                    entity["position"]["y"] = entity["position"]["y"].Value<double>() * -1;
                }
                else
                {
                    entity["position"]["x"] = entity["position"]["x"].Value<double>() * -1;
                }
            }
            return o.ToString(Formatting.None);
        }

        public static string ReplaceItems(string json, Dictionary<string, string> itemsToReplace, out string results)
        {
            var o = JObject.Parse(json);
            var entities = o["blueprint"]["entities"];
            var nReplaced = new Dictionary<string, int>();
            var nNotReplaced = new Dictionary<string, int>();
            foreach (var entity in entities)
            {
                foreach (var key in itemsToReplace.Keys)
                {
                    if (entity["name"].Value<string>() == key)
                    {
                        if (itemsToReplace[key] == "stack-filter-inserter" && entity["filters"] != null && (entity["filters"] as JArray).Count > 1)
                        {
                            if (!nNotReplaced.ContainsKey(key)) nNotReplaced.Add(key, 0);
                            nNotReplaced[key]++;
                            continue;
                        }
                        entity["name"] = itemsToReplace[key];
                        if (!nReplaced.ContainsKey(key)) nReplaced.Add(key, 0);
                        nReplaced[key]++;
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("The following items were replaced:");
            foreach (var key in nReplaced.Keys)
            {
                sb.AppendLine(key + ": " + nReplaced[key].ToString());
            }
            sb.AppendLine();
            sb.AppendLine("The following items could not be replaced:");
            foreach (var key in nNotReplaced.Keys)
            {
                sb.AppendLine(key + ": " + nNotReplaced[key].ToString());
            }
            results = sb.ToString();

            return o.ToString(Formatting.None);
        }

    }
}

/// <summary>
/// Mods JSON (Mod)
/// </summary>
namespace ModObj
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Mod
    {
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }

        [JsonProperty("changelog", NullValueHandling = NullValueHandling.Ignore)]
        public string Changelog { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("downloads_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? DownloadsCount { get; set; }

        [JsonProperty("github_path", NullValueHandling = NullValueHandling.Ignore)]
        public string GithubPath { get; set; }

        [JsonProperty("homepage", NullValueHandling = NullValueHandling.Ignore)]
        public string Homepage { get; set; }

        [JsonProperty("license", NullValueHandling = NullValueHandling.Ignore)]
        public License License { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("owner", NullValueHandling = NullValueHandling.Ignore)]
        public string Owner { get; set; }

        [JsonProperty("releases", NullValueHandling = NullValueHandling.Ignore)]
        public ObservableCollection<Release> Releases { get; set; }

        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public double? Score { get; set; }

        [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
        public string Summary { get; set; }

        [JsonProperty("tag", NullValueHandling = NullValueHandling.Ignore)]
        public Tag Tag { get; set; }

        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public string Thumbnail { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedAt { get; set; }
    }

    public partial class License
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }
    }

    public partial class Release
    {
        [JsonProperty("download_url", NullValueHandling = NullValueHandling.Ignore)]
        public string DownloadUrl { get; set; }

        [JsonProperty("file_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FileName { get; set; }

        [JsonProperty("info_json", NullValueHandling = NullValueHandling.Ignore)]
        public InfoJson InfoJson { get; set; }

        [JsonProperty("released_at", NullValueHandling = NullValueHandling.Ignore)]
        public string ReleasedAt { get; set; }

        [JsonProperty("sha1", NullValueHandling = NullValueHandling.Ignore)]
        public string Sha1 { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }

        [JsonProperty("Installed", NullValueHandling = NullValueHandling.Ignore)]
        public bool Installed { get; set; }
    }

    public partial class InfoJson
    {
        [JsonProperty("dependencies", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Dependencies { get; set; }

        [JsonProperty("factorio_version", NullValueHandling = NullValueHandling.Ignore)]
        public string FactorioVersion { get; set; }
    }

    public partial class Tag
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public partial class Mod
    {
        public static Mod FromJson(string json) => JsonConvert.DeserializeObject<Mod>(json, ModObj.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Mod self) => JsonConvert.SerializeObject(self, ModObj.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}

/// <summary>
/// Mods list JSON (LocalMod)
/// </summary>
namespace ModsList
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class LocalMod
    {
        [JsonProperty("pagination", NullValueHandling = NullValueHandling.Ignore)]
        public Pagination Pagination { get; set; }

        [JsonProperty("results")]
        public Result[] Results { get; set; }
    }

    public partial class Pagination
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("page_count")]
        public long PageCount { get; set; }

        [JsonProperty("page_size")]
        public long PageSize { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }
    }

    public partial class Links
    {
        [JsonProperty("first")]
        public object First { get; set; }

        [JsonProperty("next")]
        public object Next { get; set; }

        [JsonProperty("prev")]
        public object Prev { get; set; }

        [JsonProperty("last")]
        public object Last { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("downloads_count")]
        public long DownloadsCount { get; set; }

        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }

        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public double? Score { get; set; }

        [JsonProperty("latest_release")]
        public LatestRelease LatestRelease { get; set; }

        [JsonProperty("releases", NullValueHandling = NullValueHandling.Ignore)]
        public ObservableCollection<string> Releases { get; set; }

        [JsonProperty("releasesList", NullValueHandling = NullValueHandling.Ignore)]
        public ObservableCollection<ModObj.Release> ReleasesList { get; set; }

    }

    public partial class LatestRelease
    {
        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("info_json")]
        public InfoJson InfoJson { get; set; }

        [JsonProperty("released_at")]
        public string ReleasedAt { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }
    }

    public partial class InfoJson
    {
        [JsonProperty("factorio_version")]
        public string FactorioVersion { get; set; }
    }

    public partial class LocalMod
    {
        public static LocalMod FromJson(string json) => JsonConvert.DeserializeObject<LocalMod>(json, ModsList.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this LocalMod self) => JsonConvert.SerializeObject(self, ModsList.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}