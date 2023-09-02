
namespace FHW.Data
{
    public static class Temp
    {
        #region Mods
        public static bool ModsLoad { get; private set; } = false;
        public static Exception? ModsLoadError { get; private set; } = null;

        public static List<ModsList.Result> MainModsList { get; private set; } = new List<ModsList.Result>();
        public static Dictionary<string, Mod.Info> LastUpdateMods { get; set; } = new Dictionary<string, Mod.Info>();

        public static void LoadModsList()
        {
            try
            {
                ModsLoad = true;
                ModsList.LocalMod lm = ModsList.LocalMod.FromJson(LMC.Web.GetString(@"https://mods.factorio.com/api/mods?page_size=max"));
                MainModsList.Clear();
                lm.Results.ToList().ForEach(item => {
                    MainModsList.Add(item);
                });
                ModsLoad = false;
            }
            catch (Exception ex)
            {
                ModsLoadError = ex;
            }
        }
        #endregion

        #region Clients
        public static Dictionary<string, Dictionary<string, Client.DesktopInfo>> Clients { get; set; } = new Dictionary<string, Dictionary<string, Client.DesktopInfo>>();
        #endregion
    }
}
