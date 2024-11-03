namespace FHW.Classes.Game;

public partial class UpdatePackages
    {
        public Core[]? CoreLinux32 { get; set; }
        public Core[]? CoreLinux64 { get; set; }
        public Core[]? CoreLinuxHeadless64 { get; set; }
        public Core[]? CoreMac { get; set; }
        public Core[]? CoreMacArm64 { get; set; }
        public Core[]? CoreMacX64 { get; set; }
        public Core[]? CoreWin32 { get; set; }
        public Core[]? CoreWin64 { get; set; }
        public Core[]? CoreExpansionLinux64 { get; set; }
        public Core[]? CoreExpansionMac { get; set; }
        public Core[]? CoreExpansionWin64 { get; set; }
    }

    public partial class Core
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Stable { get; set; }
    }
