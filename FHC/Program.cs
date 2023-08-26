using System.Text;
using System.Net;
using System.Net.Http.Json;

namespace FHC
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Console.Out.WriteLineAsync("Try get public IP...");
            try
            {
                var httpClient = new HttpClient();
                var ip = await httpClient.GetStringAsync("https://api.ipify.org");
                Console.WriteLine($"Your public IP address is: {ip}");
                var register = await httpClient.PostAsJsonAsync("https://localhost:7273/api/ClientRegistration", new DesktopInfo { Name = "PCname", LocalIP = "192.168.100.1", PublicIP = ip });
                Console.WriteLine(register);
            }
            catch
            {
                await Console.Out.WriteLineAsync("Cant get public IP");
            }
            await Console.Out.WriteLineAsync("Try start HTTP server...");
            new HttpServer();


        }
    }

    public class DesktopInfo
    {
        public string Name { get; set; }
        public string LocalIP { get; set; }
        public string PublicIP { get; set; }
        public long LastActivity { get; set; } = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
    }
}