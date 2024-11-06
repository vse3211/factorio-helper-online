namespace FHW.Core.Game.Classes;

public class UpdatePackages
{
    public partial class Packages
    {
        public Destination[]? CoreLinux32 { get; set; }
        public Destination[]? CoreLinux64 { get; set; }
        public Destination[]? CoreLinuxHeadless64 { get; set; }
        public Destination[]? CoreMac { get; set; }
        public Destination[]? CoreMacArm64 { get; set; }
        public Destination[]? CoreMacX64 { get; set; }
        public Destination[]? CoreWin32 { get; set; }
        public Destination[]? CoreWin64 { get; set; }
        public Destination[]? CoreExpansionLinux64 { get; set; }
        public Destination[]? CoreExpansionMac { get; set; }
        public Destination[]? CoreExpansionWin64 { get; set; }
    }

    public partial class Destination
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Stable { get; set; }
    }
}

