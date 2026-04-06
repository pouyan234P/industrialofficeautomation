using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task<bool> Exists(int id);
        Task Updatelettr(Letter entity);
        Task Delete(int id);
        Task Update(T entity);
    }
}
