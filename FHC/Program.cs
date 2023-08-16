using System.Text;
using System.Net;

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
            }
            catch
            {
                await Console.Out.WriteLineAsync("Cant get public IP");
            }
            await Console.Out.WriteLineAsync("Try start HTTP server...");
            new HttpServer();


        }
    }
}