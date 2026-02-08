using authentication.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.IRepository
{
    public interface IDepartmentRepository
    {
        Task<bool> addDepartment(Department department);
        Task<Department> GetDepartmentbyid(int? id);
        Task<IEnumerable<Department>> GetDepartment();
        Task<Department> GetDepartmentbyname(string name);
    }
}
