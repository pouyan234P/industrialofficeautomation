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
        /// <summary>Creates the user and puts them in <paramref name="role"/>. Returns an empty list on success, otherwise the error messages.</summary>
        Task<List<string>> regiseter(User user, string password, string role);
        Task<loginResultDTO> login(string email, string password);
        /// <summary>Creates a role. Returns an empty list on success, otherwise the error messages.</summary>
        Task<List<string>> createRole(string role);
        Task<IEnumerable<roleDTO>> getRoles();
        /// <summary>Users with their role names filled in.</summary>
        Task<IEnumerable<getuserDTO>> getAll();
        /// <summary>Updates a user. Returns an empty list on success, otherwise the error messages.
        /// When <paramref name="callerIsAdmin"/> is false, Admin accounts cannot be edited.</summary>
        Task<List<string>> updateUser(int id, updateUserDTO dto, bool callerIsAdmin);
        /// <summary>Replaces the user's roles with <paramref name="roles"/>. Returns an empty list on success, otherwise the error messages.</summary>
        Task<List<string>> setUserRoles(int userId, IEnumerable<string> roles, int? actingUserId);
    }
}
