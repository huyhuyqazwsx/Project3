using Project3.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Otp
{
    public class SendOtpDto
    {
        public string Email { get; set; } = string.Empty;
        public OtpPurpose Purpose { get; set; } = OtpPurpose.Login2FA;
    }
}
