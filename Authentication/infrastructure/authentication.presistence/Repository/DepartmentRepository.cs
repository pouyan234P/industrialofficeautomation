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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly authDb _db;

        public DepartmentRepository(authDb db)
        {
            _db = db;
        }
        public async Task<bool> addDepartment(Department department)
        {
            _db.departments.Add(department);
            _db.SaveChanges();
            var t =await _db.departments.Select(t => t).OrderBy(x=>x).LastOrDefaultAsync();
            if (t == null)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<Department>> GetDepartment()
        {
            var dep=await _db.departments.Select(t=>t).ToListAsync();
            return dep;
        }

        public async Task<Department> GetDepartmentbyid(int? id)
        {
            var dep=await _db.departments.Where(x=>x.Id==id).Select(x=>x).FirstOrDefaultAsync();
            return dep!;
        }

        public async Task<Department> GetDepartmentbyname(string name)
        {
            var dep=await _db.departments.Where(x=>x.Name==name).Select(x=>x).FirstOrDefaultAsync();
            return dep!;
        }
    }
}
