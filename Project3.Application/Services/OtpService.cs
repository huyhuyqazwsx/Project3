using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Project3.Application.Dtos.Otp;
using Project3.Application.Interfaces;
using Project3.Application.Settings;
using Project3.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _cache;
        private readonly SmtpSettings _smtp;
        private readonly Random _random = new();

        public OtpService(IMemoryCache cache, IOptions<SmtpSettings> smtp)
        {
            _cache = cache;
            _smtp = smtp.Value;
        }

        public async Task<string> SendOtpAsync(SendOtpDto dto)
        {
            string code = _random.Next(100000, 999999).ToString();
            string cacheKey = $"otp:{dto.Email}:{dto.Purpose}";
            _cache.Set(cacheKey, code, TimeSpan.FromMinutes(3));

            await SendEmailAsync(dto.Email, code, dto.Purpose);

            return code;

        }

        private async Task SendEmailAsync(string email, string code, OtpPurpose purpose)
        {
            var client = new SmtpClient(_smtp.Host, _smtp.Port)
            {
                Credentials = new NetworkCredential(_smtp.Username, _smtp.Password),
                EnableSsl = _smtp.EnableSsl
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_smtp.Username, "OnlineExam OTP"),
                Subject = $"[OnlineExam] Mã OTP xác thực của bạn cho hình thức : {GetPurposeName(purpose)}",
                Body = $@"
                <html>
                  <body>
                    <h2 style='color:#007bff'>Mã OTP của bạn</h2>
                    <p>Xin chào,</p>
                    <p>Bạn đang thực hiện <b>{GetPurposeName(purpose)}</b> trên hệ thống Wise.</p>
                    <p>Mã OTP của bạn là: <b>{code}</b></p>
                    <p style='color:gray'>Hiệu lực trong 3 phút.</p>
                  </body>
                </html>",
                IsBodyHtml = true
            };
            mail.To.Add(email);

            await client.SendMailAsync(mail);

            Console.WriteLine($"[OTP SENT] {email} => {code}");
        }

        public async Task<bool> VerifyOtpAsync(VerifyOtpDto dto)
        {
            string cacheKey = $"otp:{dto.Email}:{dto.Purpose}";
            if (_cache.TryGetValue(cacheKey, out string? storedCode) && storedCode == dto.Code)
            {
                _cache.Remove(cacheKey);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        private static string GetPurposeName(OtpPurpose purpose)
        {
            return purpose switch
            {
                OtpPurpose.Registration => "Đăng ký tài khoản",
                OtpPurpose.PasswordReset => "Đặt lại mật khẩu",
                OtpPurpose.Login2FA => "Đăng nhập hai bước",
                _ => "Xác thực khác"
            };
        }
    }
}
