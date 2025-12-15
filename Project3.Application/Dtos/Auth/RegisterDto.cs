using Project3.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Auth
{
    public class RegisterDto
    {
        public required string MSSV { get; set; }
        public required string FullName { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required UserRole Role { get; set; }
    }
}
