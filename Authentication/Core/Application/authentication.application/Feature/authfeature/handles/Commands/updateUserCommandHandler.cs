using authentication.application.Feature.authfeature.request.Commands;
using authentication.application.IRepository;
using authentication.application.Responses;
using MediatR;

namespace authentication.application.Feature.authfeature.handles.Commands
{
    public class updateUserCommandHandler : IRequestHandler<updateUserCommand, baseCommandResponse>
    {
        private readonly IAuthformRepository _authformRepository;

        public updateUserCommandHandler(IAuthformRepository authformRepository)
        {
            _authformRepository = authformRepository;
        }

        public async Task<baseCommandResponse> Handle(updateUserCommand request, CancellationToken cancellationToken)
        {
            var response = new baseCommandResponse();
            var errors = await _authformRepository.updateUser(request.id, request.userDTO!);

            if (errors.Count == 0)
            {
                response.Success = true;
                response.Message = "User updated successfully";
                response.id = request.id;
            }
            else
            {
                response.Success = false;
                response.Message = "User update failed";
                response.Errors = errors;
            }
            return response;
        }
    }
}
