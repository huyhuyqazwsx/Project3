using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Wise.Application.DTOs.Auth;
using Wise.Application.Interfaces;
using Wise.Domain.Entities;
using Wise.Domain.Enums;

namespace Wise.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IJwtService _jwtService;

        public UserService(IRepository<User> repository, IJwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var exis = (await _repository.FindAsync(u => u.Email == dto.Email)).FirstOrDefault();
            if (exis != null) throw new Exception("Email đã tồn tại");

            string hashedPassword = HashPassword(dto.Password);

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = UserRole.Student,
                CreatedAt = DateTime.Now,
            };

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = (await _repository.FindAsync(u => u.Email == dto.Email)).FirstOrDefault();
            if (user == null) throw new Exception("Email không tồn tại");

            var hashedPassword = HashPassword(dto.Password);

            if (hashedPassword != user.PasswordHash) return null;

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = dto.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    };
}
