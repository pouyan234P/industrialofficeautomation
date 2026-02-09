using authentication.application.IRepository;
using authentication.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.presistence.Repository
{
    public class AuthformRepository : IAuthformRepository
    {
        public Task createRole(string role)
        {
            throw new NotImplementedException();
        }

        public Task<string> login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<User> regiseter(User user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
