using Microsoft.AspNetCore.Mvc;

namespace MS.AFORO255.Security.Controllers
{
    [Route("")] //QUITAMOS ESTO PARA QUE LA PETICION SEA A LA RAIZ DE LA URL
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}