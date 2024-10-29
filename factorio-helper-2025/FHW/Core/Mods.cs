namespace FHW.Core
{
    public class Mods
    {
        public class Build
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; }
            public string Description { get; set; }
            public string ModThumbnail { get; set; }
            public Dictionary<string, string> Mods { get; set; } = new Dictionary<string, string>();
        }
        public class StoreMod
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Version { get; set; }
            public DateTime LastUse { get; set; }
        }
        
    }
}
