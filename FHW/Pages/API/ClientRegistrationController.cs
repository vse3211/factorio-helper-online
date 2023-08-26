using FHW.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHW.Pages.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientRegistrationController : ControllerBase
    {
        public ClientRegistrationController() { }

        //Register client
        [HttpPost]
        public IActionResult Post(string Name, string PublicIP, string LocalIP)
        {
            if (Temp.Clients.ContainsKey(PublicIP))
            {
                if (Temp.Clients[PublicIP].ContainsKey(LocalIP))
                {
                    Temp.Clients[PublicIP][LocalIP].Name = Name;
                    Temp.Clients[PublicIP][LocalIP].LastActivity = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                }
                else
                {
                    Temp.Clients[PublicIP].Add(LocalIP, new Client.DesktopInfo { LocalIP = LocalIP, PublicIP = PublicIP, Name = Name });
                }
            }
            else
            {
                Temp.Clients.Add(PublicIP, new Dictionary<string, Client.DesktopInfo>());
                Temp.Clients[PublicIP].Add(LocalIP, new Client.DesktopInfo { LocalIP = LocalIP, PublicIP = PublicIP, Name = Name });
            }
            return Ok("done");
        }

        //Remove client
        [HttpDelete]
        public IActionResult Delete(string PublicIP, string LocalIP)
        {
            if (Temp.Clients.ContainsKey(PublicIP))
            {
                if (Temp.Clients[PublicIP].ContainsKey(LocalIP))
                {
                    Temp.Clients[PublicIP].Remove(LocalIP);
                }
            }
            return Ok("Remove Client Test");
        }
    }
}
