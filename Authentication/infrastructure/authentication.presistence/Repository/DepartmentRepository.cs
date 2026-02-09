using authentication.application.IRepository;
using authentication.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.presistence.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        public Task<bool> addDepartment(Department department)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Department>> GetDepartment()
        {
            throw new NotImplementedException();
        }

        public Task<Department> GetDepartmentbyid(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<Department> GetDepartmentbyname(string name)
        {
            throw new NotImplementedException();
        }
    }
}
