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
        public IActionResult Post()
        {
            return Ok("Client registration test");
        }

        //Remove client
        [HttpDelete]
        public IActionResult Delete()
        {
            return Ok("Remove Client Test");
        }
    }
}
