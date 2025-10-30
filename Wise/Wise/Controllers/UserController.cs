using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wise.Application.DTOs.Auth;
using Wise.Application.Interfaces;

namespace Wise.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var user = await _userService.RegisterAsync(dto);
                return Ok(new { Message = "Đăng ký thành công", User = user });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userService.LoginAsync(dto);
            if (user == null)
                return Unauthorized(new { Message = "Sai email hoặc mật khẩu" });

            return Ok(new { Message = "Đăng nhập thành công", User = user });
        }
    }
}
