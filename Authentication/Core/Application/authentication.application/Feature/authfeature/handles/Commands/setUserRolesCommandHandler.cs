using authentication.application.Feature.authfeature.request.Commands;
using authentication.application.IRepository;
using authentication.application.Responses;
using MediatR;

namespace authentication.application.Feature.authfeature.handles.Commands
{
    public class setUserRolesCommandHandler : IRequestHandler<setUserRolesCommand, baseCommandResponse>
    {
        private readonly IAuthformRepository _authform;

        public setUserRolesCommandHandler(IAuthformRepository authform)
        {
            _authform = authform;
        }

        public async Task<baseCommandResponse> Handle(setUserRolesCommand request, CancellationToken cancellationToken)
        {
            var response = new baseCommandResponse();
            var errors = await _authform.setUserRoles(request.userId, request.roles ?? new List<string>(), request.actingUserId);
            if (errors.Count == 0)
            {
                response.Success = true;
                response.Message = "Roles updated";
                response.id = request.userId;
            }
            else
            {
                response.Success = false;
                response.Message = "Updating roles failed";
                response.Errors = errors;
            }
            return response;
        }
    }
}
