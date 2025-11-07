using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<RefreshToken> _repoReToken;

        public UserService(IRepository<User> repository, IJwtService jwtService, IRepository<RefreshToken> repoReToken)
        {
            _repository = repository;
            _jwtService = jwtService;
            _repoReToken = repoReToken;
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

            int accessMinutes = 120;
            var accessToken = _jwtService.GenerateToken(user, accessMinutes);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _repoReToken.AddAsync(refreshToken);
            await _repoReToken.SaveChangesAsync();

            return new AuthResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = (await _repository.FindAsync(u => u.Email == dto.Email)).FirstOrDefault();
            if (user == null) throw new Exception("Email không tồn tại");

            var hashedPassword = HashPassword(dto.Password);

            if (hashedPassword != user.PasswordHash) return null;

            int accessMinutes = 120;
            var accessToken = _jwtService.GenerateToken(user, accessMinutes);

            var oldTokens = await _repoReToken.FindAsync(rt => rt.UserId == user.Id);
            foreach (var t in oldTokens)
                _repoReToken.Delete(t);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _repoReToken.AddAsync(refreshToken);
            await _repoReToken.SaveChangesAsync();

            return new AuthResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var exisToken = _repoReToken.Query()
                .Include(x => x.User)
                .FirstOrDefault(x => x.Token == refreshToken && !x.IsRevoked);

            if(exisToken == null || DateTime.UtcNow > exisToken.ExpiresAt)
            {
                throw new UnauthorizedAccessException("Refresh token invalid or expired.");
            }

            int accessMinutes = 120;

            User user = exisToken.User!;
            var token = _jwtService.GenerateToken(user , accessMinutes);


            exisToken.Token = Guid.NewGuid().ToString();
            exisToken.ExpiresAt = DateTime.UtcNow.AddDays(7);
            exisToken.IsRevoked = false;

            _repoReToken.Update(exisToken);
            await _repoReToken.SaveChangesAsync();

            return new AuthResponseDto  
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token,
                RefreshToken = exisToken.Token
            };
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            var token = await _repoReToken.Query()
                .FirstOrDefaultAsync(x => x.Token == refreshToken && !x.IsRevoked);

            if (token == null)
                return false;

            token.IsRevoked = true;
            _repoReToken.Update(token);
            await _repoReToken.SaveChangesAsync();

            return true;
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    };
}
