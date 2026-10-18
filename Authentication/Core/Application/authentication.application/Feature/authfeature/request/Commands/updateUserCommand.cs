using authentication.application.DTO;
using authentication.application.Responses;
using MediatR;

namespace authentication.application.Feature.authfeature.request.Commands
{
    public class updateUserCommand : IRequest<baseCommandResponse>
    {
        public int id { get; set; }
        public updateUserDTO? userDTO { get; set; }
        /// <summary>Set from the caller's token. Non-admins may not edit Admin accounts.</summary>
        public bool callerIsAdmin { get; set; }
    }
}
