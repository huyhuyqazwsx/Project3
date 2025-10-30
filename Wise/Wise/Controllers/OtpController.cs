using Microsoft.AspNetCore.Mvc;
using Wise.Application.DTOs.Otp;
using Wise.Application.Interfaces;

namespace Wise.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;

        public OtpController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpDto dto)
        {
            await _otpService.SendOtpAsync(dto);
            return Ok(new { Message = "OTP đã gửi đến email." });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            bool res = await _otpService.VerifyOtpAsync(dto);
            if(res == false) return BadRequest(new { Message = "OTP sai hoặc hết hạn" });

            return Ok(new { Message = "Xác thực OTP thành công" });

        }
    }
}
