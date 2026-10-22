using authentication.application.DTO;
using authentication.application.Responses;
using MediatR;

namespace authentication.application.Feature.authfeature.request.Commands
{
    public class updateUserCommand : IRequest<baseCommandResponse>
    {
        public int id { get; set; }
        public updateUserDTO? userDTO { get; set; }
        /// <summary>Set from the caller's token. Non-admins (editing their own account) can't change PersonID or status.</summary>
        public bool callerIsAdmin { get; set; }
    }
}
