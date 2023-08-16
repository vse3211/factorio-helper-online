using System.Net.Sockets;
using System.Net;
using System.Text;

namespace FHC
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var ip = await httpClient.GetStringAsync("https://api.ipify.org");
            Console.WriteLine($"My public IP address is: {ip}");


        }
    }
}