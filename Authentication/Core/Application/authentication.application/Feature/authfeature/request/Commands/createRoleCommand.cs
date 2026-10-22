using authentication.application.Responses;
using MediatR;

namespace authentication.application.Feature.authfeature.request.Commands
{
    public class createRoleCommand : IRequest<baseCommandResponse>
    {
        public string? RoleName { get; set; }
    }
}
