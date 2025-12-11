using Project3.Application.Dtos.Otp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Interfaces
{
    public interface IOtpService
    {
        Task<string> SendOtpAsync(SendOtpDto dto);
        Task<bool> VerifyOtpAsync(VerifyOtpDto dto);
    }
}
