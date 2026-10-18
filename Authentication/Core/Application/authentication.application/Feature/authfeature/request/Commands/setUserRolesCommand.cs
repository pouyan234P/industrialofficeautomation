using authentication.application.Responses;
using MediatR;

namespace authentication.application.Feature.authfeature.request.Commands
{
    public class setUserRolesCommand : IRequest<baseCommandResponse>
    {
        public int userId { get; set; }
        public List<string>? roles { get; set; }
        /// <summary>Id of the admin making the change (from the JWT), used to stop admins removing their own Admin role.</summary>
        public int? actingUserId { get; set; }
    }
}
