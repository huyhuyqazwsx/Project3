using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Application.DTOs.Otp;

namespace Wise.Application.Interfaces
{
    public interface IOtpService
    {
        Task<string> SendOtpAsync(SendOtpDto dto);
        Task<bool> VerifyOtpAsync(VerifyOtpDto dto);
    }
}
