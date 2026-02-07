using Microsoft.AspNetCore.Http;
using Project3.Application.Interfaces.IAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Services.Authorization
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated =>
           _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public int UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst("userId");

                return userIdClaim != null
                    ? int.Parse(userIdClaim.Value)
                    : throw new UnauthorizedAccessException("UserId claim not found");
            }
        }

        public string Role =>
            _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.Role)?
                .Value
            ?? string.Empty;
    }
}
