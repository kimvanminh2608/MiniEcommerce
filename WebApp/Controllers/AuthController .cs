using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.JwtService;
using WebApp.Models;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly WebAppContext _context;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        public AuthController(
            WebAppContext context,
            ITokenService tokenService,
            IUserService userService)
        {
            _context = context;
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var user = _context.Users.SingleOrDefault(x => x.Username == loginModel.Username && x.Password == loginModel.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            //Call service user
            var rs = await _userService.RegisterUser(user);
            if (!rs) return BadRequest("something went wrong!");
            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }

    }
}
