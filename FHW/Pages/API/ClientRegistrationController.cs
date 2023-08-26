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
        public IActionResult Post(Client.DesktopInfo desktop)
        {
            if (Temp.Clients.ContainsKey(desktop.PublicIP))
            {
                if (Temp.Clients[desktop.PublicIP].ContainsKey(desktop.LocalIP))
                {
                    Temp.Clients[desktop.PublicIP][desktop.LocalIP].Name = desktop.Name;
                    Temp.Clients[desktop.PublicIP][desktop.LocalIP].LastActivity = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                }
                else
                {
                    Temp.Clients[desktop.PublicIP].Add(desktop.LocalIP, new Client.DesktopInfo { LocalIP = desktop.LocalIP, PublicIP = desktop.PublicIP, Name = desktop.Name });
                }
            }
            else
            {
                Temp.Clients.Add(desktop.PublicIP, new Dictionary<string, Client.DesktopInfo>());
                Temp.Clients[desktop.PublicIP].Add(desktop.LocalIP, new Client.DesktopInfo { LocalIP = desktop.LocalIP, PublicIP = desktop.PublicIP, Name = desktop.Name });
            }
            return Ok();
        }

        //Remove client
        [HttpDelete]
        public IActionResult Delete(Client.DesktopInfo desktop)
        {
            if (Temp.Clients.ContainsKey(desktop.PublicIP))
            {
                if (Temp.Clients[desktop.PublicIP].ContainsKey(desktop.LocalIP))
                {
                    Temp.Clients[desktop.PublicIP].Remove(desktop.LocalIP);
                }
            }
            return Ok("Remove Client Test");
        }
    }
}
