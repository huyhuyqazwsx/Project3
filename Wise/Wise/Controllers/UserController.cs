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
            try
            {
                var user = await _userService.LoginAsync(dto);
                if (user == null)
                    return Unauthorized(new { Message = "Sai email hoặc mật khẩu" });

                return Ok(new { Message = "Đăng nhập thành công", User = user });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Làm mới access token bằng refresh token
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequestDto dto)
        {
            try
            {
                var result = await _userService.RefreshTokenAsync(dto.RefreshToken);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] RefreshRequestDto dto)
        {
            try
            {
                var result = await _userService.LogoutAsync(dto.RefreshToken);
                if (result) return Ok(new {message = "Đăng xuất thành công"});
                else return NotFound(new { message = "Refresh token không tồn tại hoặc đã bị thu hồi." });
            }
            catch (Exception e)
            {
                return BadRequest($"Unable to logout: {e}");
            }
        }
    }
    
}
