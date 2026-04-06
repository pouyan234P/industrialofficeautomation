using AutoMapper;
using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.Application.Feature.LetterFeature.request.Command;
using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.LetterFeature.handles.Command
{
    public class updateLetterCommandHandler : IRequestHandler<updateLetterCommand, Unit>
    {
        private readonly ILetterRepository _repository;
        private readonly IMapper _mapper;

        public updateLetterCommandHandler(ILetterRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(updateLetterCommand request, CancellationToken cancellationToken)
        {
            var map=_mapper.Map<Letter>(request.letterDTO);
            await _repository.Updatelettr(map);
            return Unit.Value;
        }
    }
}
