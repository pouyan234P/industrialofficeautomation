using AutoMapper;
using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.Application.Feature.AttachmentFeature.request.Commands;
using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.AttachmentFeature.handles.Commands
{
    public class updateAttachmentCommandHandler : IRequestHandler<updateAttachmentCommand, Unit>
    {
        private readonly IattachmentRepository _repository;
        private readonly IMapper _mapper;

        public updateAttachmentCommandHandler(IattachmentRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(updateAttachmentCommand request, CancellationToken cancellationToken)
        {
            var map = _mapper.Map<Attachment>(request.attachmentDTO);
            await _repository.Update(map);
            return Unit.Value;
        }
    }
}
