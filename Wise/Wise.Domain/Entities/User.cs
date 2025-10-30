using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Domain.Enums;

namespace Wise.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public int Level { get; set; } = 1;
        public UserRole Role { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ICollection<LearningResult> LearningResults { get; set; } = new List<LearningResult>();
    }
}
