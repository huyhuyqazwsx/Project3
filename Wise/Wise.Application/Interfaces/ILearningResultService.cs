using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Application.DTOs.Learning;
using Wise.Domain.Entities;

namespace Wise.Application.Interfaces
{
    public interface ILearningResultService
    {
        Task<LearningResult> SubmitAsync(LearningSubmitDto dto);
        Task<IEnumerable<LearningResult>> GetByUserAsync(int userId);
    }
}
