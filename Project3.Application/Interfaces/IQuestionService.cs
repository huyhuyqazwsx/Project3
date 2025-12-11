using Project3.Application.Dtos.Question;
using Project3.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Interfaces
{
    public interface IQuestionService : ICrudService<Question>
    {
        //Phuong thuc rieng cua question
        Task<bool> AddListQuestion(CreateQuestionDto[] questionDtos);
    }
}
