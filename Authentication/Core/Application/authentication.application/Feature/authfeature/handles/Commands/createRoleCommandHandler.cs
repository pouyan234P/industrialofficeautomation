using authentication.application.Feature.authfeature.request.Commands;
using authentication.application.IRepository;
using authentication.application.Responses;
using MediatR;

namespace authentication.application.Feature.authfeature.handles.Commands
{
    public class createRoleCommandHandler : IRequestHandler<createRoleCommand, baseCommandResponse>
    {
        private readonly IAuthformRepository _authform;

        public createRoleCommandHandler(IAuthformRepository authform)
        {
            _authform = authform;
        }

        public async Task<baseCommandResponse> Handle(createRoleCommand request, CancellationToken cancellationToken)
        {
            var response = new baseCommandResponse();
            var errors = await _authform.createRole(request.RoleName!.Trim());
            if (errors.Count == 0)
            {
                response.Success = true;
                response.Message = $"Role '{request.RoleName!.Trim()}' created";
            }
            else
            {
                response.Success = false;
                response.Message = "Role creation failed";
                response.Errors = errors;
            }
            return response;
        }
    }
}
