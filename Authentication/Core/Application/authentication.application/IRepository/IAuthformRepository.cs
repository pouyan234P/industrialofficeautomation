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
        Task<User> regiseter(User user,string password);
        Task<string> login(string email, string password);
        Task createRole(string role);
    }
}
