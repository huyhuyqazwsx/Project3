using Project3.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Auth
{
    public class AuthResponseDto
    {
        public int Id { get; set; }
        public required string MSSV { get; set; }
        public string FullName { get; set; } = string.Empty;
        public required DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public required UserRole Role { get; set; }
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
