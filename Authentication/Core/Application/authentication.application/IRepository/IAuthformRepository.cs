using authentication.application.DTO;
using authentication.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.IRepository
{
    public interface IAuthformRepository
    {
        Task<User?> regiseter(User user,string password,string role);
        Task<string> login(string email, string password);
        Task<string> createRole(string role);
        Task<IEnumerable<User>> getAll();
        /// <summary>Updates a user. Returns an empty list on success, otherwise the error messages.</summary>
        Task<List<string>> updateUser(int id, updateUserDTO dto);
    }
}
