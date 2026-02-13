using AutoMapper;
using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.Application.Feature.LetterFeature.request.Queries;
using CorrespondenceCore.Application.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.LetterFeature.handles.Queries
{
    public class getLetterDetailRequestHandler : IRequestHandler<getLetterDetailRequest, LetterDTO>
    {
        private readonly ILetterRepository _repository;
        private readonly IMapper _mapper;

        public getLetterDetailRequestHandler(ILetterRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<LetterDTO> Handle(getLetterDetailRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.Get(request.id);
            var resultmap=_mapper.Map<LetterDTO>(result);
            return resultmap;
        }
    }
}
