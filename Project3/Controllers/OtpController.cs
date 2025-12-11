using Microsoft.AspNetCore.Mvc;
using Project3.Application.Dtos.Otp;
using Project3.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Controllers
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
            if (res == false) return BadRequest(new { Message = "OTP sai hoặc hết hạn" });

            return Ok(new { Message = "Xác thực OTP thành công" });

        }
    }
}
