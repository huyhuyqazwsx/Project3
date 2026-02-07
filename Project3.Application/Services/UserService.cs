using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Project3.Application.Dtos.Auth;
using Project3.Application.Interfaces;
using Project3.Domain.Entities;
using Project3.Domain.Enums;
using Project3.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Services
{
    public class UserService : CrudService<User>, IUserService
    {
        private readonly IJwtService _jwtService;
        private readonly IRepository<RefreshToken> _repoReToken;

        public UserService(IRepository<User> repository, IJwtService jwtService, IRepository<RefreshToken> repoReToken)
            : base(repository)
        {
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
                MSSV = dto.MSSV,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = dto.Role
            };

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            int accessMinutes = 5;
            var accessToken = _jwtService.GenerateToken(user, accessMinutes);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.Now.AddDays(7),
                IsRevoked = false
            };

            await _repoReToken.AddAsync(refreshToken);
            await _repoReToken.SaveChangesAsync();

            return new AuthResponseDto
            {
                Id = user.Id,
                MSSV = user.MSSV,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                Role = user.Role,
                Token = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _repository.GetByIdAsync(dto.UserId);
            if (user == null)
                return false;

            var oldPasswordHash = HashPassword(dto.OldPassword);

            if (user.PasswordHash == oldPasswordHash)
            {
                var newPasswordHash = HashPassword(dto.NewPassword);
                user.PasswordHash = newPasswordHash;

                _repository.UpdateAsync(user);
                await _repository.SaveChangesAsync();
                return true;
            }

            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserRoleAsync(UpdateUserRoleDto dto)
        {
            var user = await _repository.GetByIdAsync(dto.UserId);
            if (user == null)
                return false;

            var allowedRoles = new[] { "ADMIN", "TEACHER", "STUDENT" };
            if (!allowedRoles.Contains(dto.Role.ToString()))
                throw new Exception("Role không hợp lệ");

            if (user.Role == dto.Role)
                return true;

            user.Role = dto.Role;
            _repository.UpdateAsync(user);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = (await _repository.FindAsync(u => u.Email == dto.Email)).FirstOrDefault();
            if (user == null) throw new Exception("Email không tồn tại");

            var hashedPassword = HashPassword(dto.Password);

            if (hashedPassword != user.PasswordHash) return null;

            int accessMinutes = 10;
            var accessToken = _jwtService.GenerateToken(user, accessMinutes);

            var oldTokens = await _repoReToken.FindAsync(rt => rt.UserId == user.Id);
            foreach (var t in oldTokens)
                _repoReToken.DeleteAsync(t);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString(),
                ExpiresAt = DateTime.Now.AddDays(7),
                IsRevoked = false
            };

            await _repoReToken.AddAsync(refreshToken);
            await _repoReToken.SaveChangesAsync();

            return new AuthResponseDto
            {
                Id = user.Id,
                MSSV = user.MSSV,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                Role = user.Role,
                Token = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var exisToken = _repoReToken.Query()
                .Include(x => x.User)
                .FirstOrDefault(x => x.Token == refreshToken && !x.IsRevoked);

            if (exisToken == null || DateTime.Now > exisToken.ExpiresAt)
            {
                throw new UnauthorizedAccessException("Refresh token invalid or expired.");
            }

            int accessMinutes = 10;

            User user = exisToken.User!;
            var token = _jwtService.GenerateToken(user, accessMinutes);


            exisToken.Token = Guid.NewGuid().ToString();
            exisToken.ExpiresAt = DateTime.Now.AddDays(7);
            exisToken.IsRevoked = false;

            _repoReToken.UpdateAsync(exisToken);
            await _repoReToken.SaveChangesAsync();

            return new AuthResponseDto
            {
                Id = user.Id,
                MSSV = user.MSSV,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                Role = user.Role,
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
            _repoReToken.UpdateAsync(token);
            await _repoReToken.SaveChangesAsync();

            return true;
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }

        public Task<bool> AddListAccount(RegisterDto[] dto)
        {
            throw new NotImplementedException();
        }
    };
}
