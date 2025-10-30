using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Enums;

namespace Wise.Application.DTOs.Otp
{
    public class VerifyOtpDto
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public OtpPurpose Purpose { get; set; } = OtpPurpose.Registration;
    }
}
