using Project3.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Auth
{
    public class UpdateUserRoleDto
    {
        public int UserId { get; set; }
        public UserRole Role { get; set; } = UserRole.STUDENT;
    }
}
