using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Presistence.Repository
{
    public class genericRepository<T> :IGenericRepository<T> where T : class
    {
        private readonly CorrespondenceCoreDB _db;

        public genericRepository(CorrespondenceCoreDB db)
        {
            _db = db;
        }

        public async Task<T> Add(T entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = await _db.Set<T>().OrderBy(x=>x).LastOrDefaultAsync();
            return result!;
        }

        public async Task Delete(int id)
        {
            var any=await _db.Set<T>().FindAsync(id);
            _db.Set<T>().Remove(any);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity=await Get(id);
            return entity!=null;
        }

        public async Task<T> Get(int id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task Update(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Updatelettr(Letter entity)
        {
            var myletter = await _db.letters.Where(x => x.ID == entity.ID).Select(x => x).FirstOrDefaultAsync();
            myletter.LetterNo = entity.LetterNo;
            myletter.SentDate = entity.SentDate;
            _db.Update(myletter);
            _db.SaveChanges();
        }
    }
}
