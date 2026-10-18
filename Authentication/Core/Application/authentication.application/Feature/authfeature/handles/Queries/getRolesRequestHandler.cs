using authentication.application.DTO;
using authentication.application.Feature.authfeature.request.Queries;
using authentication.application.IRepository;
using MediatR;

namespace authentication.application.Feature.authfeature.handles.Queries
{
    public class getRolesRequestHandler : IRequestHandler<getRolesRequest, IEnumerable<roleDTO>>
    {
        private readonly IAuthformRepository _repository;

        public getRolesRequestHandler(IAuthformRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<roleDTO>> Handle(getRolesRequest request, CancellationToken cancellationToken)
            => _repository.getRoles();
    }
}
