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
    public class getPositionbydepandposRequestHandler : IRequestHandler<getPositonbydepandposRequest, getPosition>
    {
        private readonly IPositionRepository _repository;
        private readonly IMapper _mapper;

        public getPositionbydepandposRequestHandler(IPositionRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<getPosition> Handle(getPositonbydepandposRequest request, CancellationToken cancellationToken)
        {
            var pos = await _repository.getPosdep(request.depid,request.posid);
            var posmap=_mapper.Map<getPosition>(pos);
            return posmap;
        }
    }
}
