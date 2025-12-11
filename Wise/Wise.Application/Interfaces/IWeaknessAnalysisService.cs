using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Application.DTOs.Analysis;

namespace Wise.Application.Interfaces
{
    public interface IWeaknessAnalysisService
    {
        Task<WeaknessReportDto> AnalyzeAsync(int userId);
    }
}
