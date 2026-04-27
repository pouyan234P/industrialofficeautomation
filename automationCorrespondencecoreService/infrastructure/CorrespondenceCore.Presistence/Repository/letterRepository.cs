using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.domain;
using CorrespondenceCore.domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Presistence.Repository
{
    public class letterRepository:genericRepository<Letter>,ILetterRepository
    {
        private readonly CorrespondenceCoreDB _db;

        public letterRepository(CorrespondenceCoreDB db):base(db) 
        {
            _db = db;
        }

        public async Task<IEnumerable<Letter>> GetLetterbyTypeAsync(Typecorrespondence type)
        {
            var letters=await _db.letters.Where(x=>x.type == type).ToListAsync();
            return letters;
        }
    }
}
