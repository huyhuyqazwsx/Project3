using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Dtos.Background
{
    public sealed record ExamSubmitJob(
        int ExamId,
        int StudentId,
        DateTime Deadline
    );
}
