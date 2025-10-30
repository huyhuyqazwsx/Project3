using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Entities;

namespace Wise.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
