using System.Net.Sockets;
using System.Net;
using System.Text;

namespace FHC
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string baseUrl = "http://localhost:12345/";

            using (var listener = new HttpListener())
            {
                listener.Prefixes.Add(baseUrl);
                listener.Start();

                Console.WriteLine("Server is listening on port 12345...");

                while (true)
                {
                    var context = await listener.GetContextAsync();
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:7273");
                    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST");

                    ProcessRequest(context);
                }
            }
        }

        static void ProcessRequest(HttpListenerContext context)
        {
            string guid = context.Request.QueryString["guid"];
            Console.WriteLine($"Received GUID: {guid}");

            // Здесь вы можете провести проверку авторизации на сервере

            string response = "Authorization successful!";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(response);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }
    }
}