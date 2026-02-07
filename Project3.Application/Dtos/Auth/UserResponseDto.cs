using Project3.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Auth
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public required string MSSV { get; set; }
        public required string FullName { get; set; } = String.Empty;
        public required DateTime DateOfBirth { get; set; }
        public required string Email { get; set; }
        public required UserRole Role { get; set; }
    }
}
