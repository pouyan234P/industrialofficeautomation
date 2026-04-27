using AutoMapper;
using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.Application.Feature.LetterFeature.request.Queries;
using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.LetterFeature.handles.Queries
{
    public class getLetterbyTypeRequestHandler : IRequestHandler<getLetterbyTypeRequest, IEnumerable<LetterDTO>>
    {
        private readonly ILetterRepository _repository;
        private readonly IMapper _mapper;

        public getLetterbyTypeRequestHandler(ILetterRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<LetterDTO>> Handle(getLetterbyTypeRequest request, CancellationToken cancellationToken)
        {
            
           var mytype=(Typecorrespondence)request.type;
            var letters=await _repository.GetLetterbyTypeAsync(mytype);
            var lettersmap=_mapper.Map<IEnumerable<LetterDTO>>(letters);
            return lettersmap;
        }
    }
}
