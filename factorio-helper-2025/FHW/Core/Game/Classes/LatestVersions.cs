using System;

namespace FHW.Core.Game.Classes;

public class LatestVersions
{
    public partial class Branch
    {
        public Type Experimental { get; set; }
        public Type Stable { get; set; }
    }

    public partial class Type
    {
        public string Alpha { get; set; }
        public string Demo { get; set; }
        public string Expansion { get; set; }
        public string Headless { get; set; }
    }
}
