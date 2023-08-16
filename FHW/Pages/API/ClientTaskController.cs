using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FHW.Pages.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientTaskController : ControllerBase
    {
        public ClientTaskController() { }

        [HttpGet]
        public IActionResult Get(string Id)
        {
            return Ok(new { Id });
        }
    }
}
