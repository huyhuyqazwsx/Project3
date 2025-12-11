using Project3.Application.Dtos.ExamBlueprint;
using Project3.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Interfaces
{
    public interface IExamBlueprintService : ICrudService<ExamBlueprint>
    {
        Task<ExamBlueprintDto> CreateBlueprintAsync(CreateExamBlueprintDto dto);
    }
}
