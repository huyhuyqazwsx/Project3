using Microsoft.AspNetCore.Mvc;
using Project3.Application.Dtos.Auth;
using Project3.Application.Interfaces;

namespace Project3.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();

                var res = users.Select(user => new UserResponseDto
                {
                    Id = user.Id,
                    MSSV = user.MSSV,
                    FullName = user.FullName,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    Role = user.Role
                });
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null) return NotFound();
                var res = new UserResponseDto {
                    Id = user.Id,
                    MSSV = user.MSSV,
                    FullName = user.FullName,
                    DateOfBirth = user.DateOfBirth,
                    Email = user.Email,
                    Role = user.Role
                };

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var success = await _userService.ChangePasswordAsync(dto);
            if (!success)
                return BadRequest(new { message = "Mật khẩu cũ không đúng" });

            return Ok(new { message = "Đổi mật khẩu thành công" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteAsync(id);
            return Ok(new { message = "Xóa thành công"});
        }

        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleDto dto)
        {
            var success = await _userService.UpdateUserRoleAsync(dto);

            if (!success)
                return NotFound(new { message = "Không tìm thấy user hoặc role không hợp lệ" });

            return Ok(new { message = "Cập nhật role thành công" });
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
                if (result) return Ok(new { message = "Đăng xuất thành công" });
                else return NotFound(new { message = "Refresh token không tồn tại hoặc đã bị thu hồi." });
            }
            catch (Exception e)
            {
                return BadRequest($"Unable to logout: {e}");
            }
        }
    }
}
