using authentication.application.DTO;
using authentication.application.Feature.positionRepository.request.Queries;
using authentication.application.IRepository;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.positionRepository.handle.Queries
{
    public class getpositionrequestHandler : IRequestHandler<getpositionrequest, getPosition>
    {
        private readonly IPositionRepository _repository;
        private readonly IMapper _mapper;

        public getpositionrequestHandler(IPositionRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<getPosition> Handle(getpositionrequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.getPosition(request.id);
            var resultreturn=_mapper.Map<getPosition>(result);
            return resultreturn;
        }
    }
}
