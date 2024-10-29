namespace FHW.Data.Temp;

public class Mods
{
    public static bool ModsLoad { get; private set; } = false;
        public static Exception? ModsLoadError { get; private set; } = null;

        public static List<ModsList.Result> MainModsList { get; private set; } = new List<ModsList.Result>();
        public static Dictionary<string, Mod.Info> LastUpdateMods { get; set; } = new Dictionary<string, Mod.Info>();
        
        public static DateTimeOffset LastUpdated { get; private set; } = DateTimeOffset.MinValue;
        public static bool isUpdating { get; private set; } = false;
        private static Timer WatchDog = new(new TimerCallback(_ =>
        {
            if (!isUpdating && !ModsLoad)
            {
                isUpdating = true;
                if (DateTimeOffset.Now - LastUpdated > TimeSpan.FromHours(12))
                {
                    LoadModsList();
                    LastUpdated = DateTime.Now;
                }

                isUpdating = false;
            }
        }), null, 0, 100);

        public static void LoadModsList()
        {
            Guid guid = Guid.NewGuid();
            try
            {
                Console.WriteLine($"[{guid}] {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss zzz")}: Loading mods...");
                ModsLoad = true;
                ModsList.LocalMod lm =
                    ModsList.LocalMod.FromJson(LMC.Web.GetString(@"https://mods.factorio.com/api/mods?page_size=max"));
                MainModsList.Clear();
                lm.Results.ToList().ForEach(item =>
                {
                    if (item.LatestRelease != null) MainModsList.Add(item);
                });
                ModsLoad = false;
            }
            catch (Exception ex)
            {
                ModsLoadError = ex;
                Console.WriteLine(
                    $"[{guid}] {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss zzz")}: Mods loading error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"[{guid}] {DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss zzz")}: Mods loaded!");
            }
        }
}