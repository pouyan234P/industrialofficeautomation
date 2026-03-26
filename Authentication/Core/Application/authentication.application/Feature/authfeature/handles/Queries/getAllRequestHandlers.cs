using authentication.application.DTO;
using authentication.application.Feature.authfeature.request.Queries;
using authentication.application.IRepository;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.authfeature.handles.Queries
{
    public class getAllRequestHandlers : IRequestHandler<getAllRequest,IEnumerable<getuserDTO>>
    {
        private readonly IAuthformRepository _repository;
        private readonly IMapper _mapper;

        public getAllRequestHandlers(IAuthformRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<getuserDTO>> Handle(getAllRequest request, CancellationToken cancellationToken)
        {
            var user = await _repository.getAll();
            var usermap=_mapper.Map<IEnumerable<getuserDTO>>(user);
            return usermap;
        }
    }
}
