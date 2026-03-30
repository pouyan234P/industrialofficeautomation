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
    public class getPositionsRequestHandler : IRequestHandler<getPositionsRequest, IEnumerable<getPosition>>
    {
        private readonly IPositionRepository _repository;
        private readonly IMapper _mapper;

        public getPositionsRequestHandler(IPositionRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<getPosition>> Handle(getPositionsRequest request, CancellationToken cancellationToken)
        {
            var pos = await _repository.getAll();
            var posmap=_mapper.Map<IEnumerable<getPosition>>(pos!);
            return posmap;
        }
    }
}
