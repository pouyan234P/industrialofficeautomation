using AutoMapper;
using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.Application.Feature.LetterFeature.request.Command;
using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.Application.Responses;
using CorrespondenceCore.domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.LetterFeature.handles.Command
{
    public class createLetterCommandHandler : IRequestHandler<createLetterCommand, baseCommandResponse>
    {
        private readonly ILetterRepository _repository;
        private readonly IMapper _mapper;

        public createLetterCommandHandler(ILetterRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<baseCommandResponse> Handle(createLetterCommand request, CancellationToken cancellationToken)
        {
            var response = new baseCommandResponse();
            var map = _mapper.Map<Letter>(request.setLetterDTO);
            map.CreatedDate=DateTime.Now;
            map.BodyHTMLID=request.setLetterDTO!.BodyHTML!.ToString();
            var result=await _repository.Add(map);
            var resultmap = _mapper.Map<LetterDTO>(result);
            resultmap.BodyHTML = result.BodyHTMLID;
            if(resultmap!=null)
            {
                response.Success= true;
                response.Message = resultmap!;
                response.id = result.ID;
            }
            return response;
        }
    }
}
