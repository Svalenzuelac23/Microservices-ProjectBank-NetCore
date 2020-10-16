using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MS.AFORO255.Cross.Jwt.Src;
using MS.AFORO255.Security.DTO;
using MS.AFORO255.Security.Service;

namespace MS.AFORO255.Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccessService _accessService;
        private readonly JwtOptions _jwtOption;

        public AuthController(IAccessService accessService,
            IOptionsSnapshot<JwtOptions> jwtOption)
        {
            _accessService = accessService;
            _jwtOption = jwtOption.Value;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_accessService.GetAll());
        }

        [HttpPost]
        public IActionResult Post([FromBody] AuthRequest request) 
        {
            if (!_accessService.Validate(request.UserName, request.Password))
            {
                return Unauthorized();
            }

            //ESTAMOS PASANDO EL TOKEN POR EL HEADER PARA MAYOR SEGURIDAD Y NO MANDARLO EN EL BODY
            Response.Headers.Add("access-control-expose-headers", "Authorization");
            Response.Headers.Add("Authorization", JwtToken.Create(_jwtOption));

            return Ok();
        }
    }
}