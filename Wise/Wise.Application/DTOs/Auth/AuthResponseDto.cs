using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Entities;

namespace Wise.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
