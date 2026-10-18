using authentication.application.DTO;
using authentication.application.Feature.authfeature.request.Queries;
using authentication.application.IRepository;
using MediatR;

namespace authentication.application.Feature.authfeature.handles.Queries
{
    public class loginRequestHandlers : IRequestHandler<loginRequest, loginResultDTO>
    {
        private readonly IAuthformRepository _authform;

        public loginRequestHandlers(IAuthformRepository authform)
        {
            _authform = authform;
        }

        public Task<loginResultDTO> Handle(loginRequest request, CancellationToken cancellationToken)
            => _authform.login(request.myslogindto!.Email!.Trim(), request.myslogindto.Password!);
    }
}
