using authentication.application.DTO;
using authentication.application.Feature.authfeature.request.Queries;
using authentication.application.IRepository;
using MediatR;

namespace authentication.application.Feature.authfeature.handles.Queries
{
    public class getAllRequestHandlers : IRequestHandler<getAllRequest, IEnumerable<getuserDTO>>
    {
        private readonly IAuthformRepository _repository;

        public getAllRequestHandlers(IAuthformRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<getuserDTO>> Handle(getAllRequest request, CancellationToken cancellationToken)
            => _repository.getAll();
    }
}
