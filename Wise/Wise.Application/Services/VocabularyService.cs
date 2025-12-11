using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wise.Application.Interfaces;
using Wise.Domain.Entities;

namespace Wise.Application.Services
{
    public class VocabularyService : IVocabularyService
    {
        private readonly IRepository<Vocabulary> _repoVoca;
        public VocabularyService(IRepository<Vocabulary> repoVoca)
        {
            _repoVoca = repoVoca;
        }

        public async Task<IEnumerable<Vocabulary>> GetAllAsync()
        {
            return await _repoVoca.Query()
                .AsNoTracking()
                .OrderBy(v => v.Word)
                .ToListAsync();
        }

        public async Task<Vocabulary?> GetByIdAsync(int id)
        {
            return await _repoVoca.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Vocabulary>> GetByLessonIdAsync(int lessonId)
        {
            return await _repoVoca.Query()
                .Where(v => v.LessonId == lessonId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Vocabulary> CreateAsync(Vocabulary model)
        {
            await _repoVoca.AddAsync(model);
            await _repoVoca.SaveChangesAsync();
            return model;
        }

        public async Task<Vocabulary?> UpdateAsync(int id, Vocabulary model)
        {
            var existing = await _repoVoca.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Word = model.Word;
            existing.Synonym = model.Synonym;
            existing.PartOfSpeech = model.PartOfSpeech;
            existing.Transcription = model.Transcription;
            existing.AudioUrl = model.AudioUrl;
            existing.ImageUrl = model.ImageUrl;
            existing.Meaning = model.Meaning;
            existing.Example = model.Example;
            existing.LessonId = model.LessonId;
            existing.Topic = model.Topic;

            _repoVoca.Update(existing);
            await _repoVoca.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repoVoca.GetByIdAsync(id);
            if (existing == null) return false;

            _repoVoca.Delete(existing);
            await _repoVoca.SaveChangesAsync();
            return true;
        }
    }
}
