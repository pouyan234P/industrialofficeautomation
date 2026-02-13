using AutoMapper;
using CorrespondenceCore.Application.Feature.AttachmentFeature.request.Commands;
using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.Application.Responses;
using CorrespondenceCore.domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.AttachmentFeature.handles.Commands
{
    public class createAttachmentCommandHandler : IRequestHandler<createAttachmentCommand, baseCommandResponse>
    {
        private readonly IattachmentRepository _repository;
        private readonly IMapper _mapper;

        public createAttachmentCommandHandler(IattachmentRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<baseCommandResponse> Handle(createAttachmentCommand request, CancellationToken cancellationToken)
        {
            var command = new baseCommandResponse();
            var map=_mapper.Map<Attachment>(request.setAttachmentDTO);
            var response = _repository.Add(map);
            if (response != null)
            {
                command.Success = true;
                command.Message = "Successfull Created";
                command.id=response.Id;
            }

            return command;
        }
    }
}
