using Microsoft.EntityFrameworkCore;
using Project3.Application.Dtos.Subject;
using Project3.Application.Interfaces;
using Project3.Domain.Entities;
using Project3.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Services
{
    public class SubjectService : CrudService<Subject>, ISubjectService
    {

        public SubjectService(IRepository<Subject> repository) : base(repository) { }

        public async Task<bool> AddListSubject(CreateSubjectDto[] dto)
        {
            try
            {
                var entities = dto.Select(s => new Subject
                {
                    Name = s.Name,
                    SubjectCode = s.SubjectCode,
                    TotalChapters = s.TotalChapters
                }).ToList();

                await _repository.AddRangeAsync(entities);
                await _repository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public async Task<Subject?> GetByCodeAsync(string code)
        {
            return (await _repository.FindAsync(s => s.SubjectCode == code))
                    .FirstOrDefault();
        }

        public async Task<List<Subject>> SearchAsync(SubjectSearchDto dto)
        {
            var query = _repository.Query();

            if (!string.IsNullOrWhiteSpace(dto.Keyword))
            {
                var key = dto.Keyword.Trim().ToLower();
                query = query.Where(s =>
                    s.Name.ToLower().Contains(key) ||
                    s.SubjectCode.ToLower().Contains(key));
            }

            if (dto.MinChapters.HasValue)
                query = query.Where(s => s.TotalChapters >= dto.MinChapters.Value);

            if (dto.MaxChapters.HasValue)
                query = query.Where(s => s.TotalChapters <= dto.MaxChapters.Value);

            query = dto.SortBy.ToLower() switch
            {
                "code" => dto.Desc ? query.OrderByDescending(s => s.SubjectCode)
                                   : query.OrderBy(s => s.SubjectCode),

                "chapters" => dto.Desc ? query.OrderByDescending(s => s.TotalChapters)
                                       : query.OrderBy(s => s.TotalChapters),

                _ => dto.Desc ? query.OrderByDescending(s => s.Name)
                              : query.OrderBy(s => s.Name)
            };

            return await  query.ToListAsync();
        }
    }
}
