namespace FHW.Data
{
    public static class Temp
    {
        public static bool ModsLoad { get; private set; } = false;
        public static Exception? ModsLoadError { get; private set; } = null;

        public static List<ModsList.Result> MainModsList { get; private set; } = new List<ModsList.Result>();

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
    }
}
