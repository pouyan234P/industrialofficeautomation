using authentication.application.IRepository;
using authentication.domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.presistence.Repository
{
    public class PositionRepository : IPositionRepository
    {
        private readonly authDb _db;

        public PositionRepository(authDb db)
        {
            _db = db;
        }
        public async Task<bool> deletePosition(int id)
        {
            var pos = await _db.positions.Where(x => x.Id == id).Select(x => x).FirstOrDefaultAsync();
            _db.positions.Remove(pos);
            _db.SaveChanges();
            var postrue=await _db.positions.Where(x=>x.Id==id).Select(x => x).FirstOrDefaultAsync();
            if(postrue==null)
                return true;
            return false;
        }

        public async Task<IEnumerable<Position>> getAll()
        {
            var pos=await _db.positions.Select(x=>x).Include(x=>x.Department).Include(x=>x.User).ToListAsync();
            return pos;
        }

        public async Task<Position> getPosdep(int deptid,int posid)
        {
            var pos = await _db.positions.Where(x => x.Department.Id == deptid && x.Id==posid).Select(x => x).Include(x => x.Department).Include(x => x.User).FirstOrDefaultAsync();
            return pos;
        }

        public async Task<Position> getPosition(int positionId)
        {
            var pos=await _db.positions.Where(x=>x.Id==positionId).Select(x=>x).Include(x=>x.Department).Include(x=>x.User).FirstOrDefaultAsync();
            return pos!;
        }

        public async Task<Position> getPositionbyDept(int deptid)
        {
            var pos = await _db.positions.Where(x => x.Department.Id == deptid).Select(x => x).Include(x => x.Department).Include(x => x.User).FirstOrDefaultAsync();
            return pos;
        }

        public async Task<Position> getPositionbyuser(int userid)
        {
            var pos = await _db.positions.Where(x => x.User.Id == userid).Select(x => x).Include(x => x.Department).Include(x => x.User).FirstOrDefaultAsync();
            return pos!;
        }

        public async Task<bool> insertPosition(Position position)
        {
            _db.positions.Add(position);
            _db.SaveChanges();
            var pos=await _db.positions.Select(x=>x).OrderBy(x=>x).LastOrDefaultAsync();
            if (pos == null)
            {
                return false;
            }
            return true;
        }

        public async Task<Position> updatePosition(int id,Position position)
        {
            // Load without Include(): we change the foreign keys directly, and loaded
            // navigation objects would conflict with the new IDs.
            var pos = await _db.positions.FirstOrDefaultAsync(x => x.Id == id);
            if (pos == null)
            {
                return null!;
            }
            pos.Title = position.Title;
            pos.departmentId = position.departmentId;
            pos.userId = position.userId;
            await _db.SaveChangesAsync();
            return pos;
        }
    }
}
