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
    public class getPositionbyUserRequestHandler : IRequestHandler<getPositionbyuserRequest, getPosition>
    {
        private readonly IPositionRepository _repository;
        private readonly IMapper _mapper;

        public getPositionbyUserRequestHandler(IPositionRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<getPosition> Handle(getPositionbyuserRequest request, CancellationToken cancellationToken)
        {
            var pos = await _repository.getPositionbyuser(request.userid);
            var posmap=_mapper.Map<getPosition>(pos);
            return posmap;
        }
    }
}
