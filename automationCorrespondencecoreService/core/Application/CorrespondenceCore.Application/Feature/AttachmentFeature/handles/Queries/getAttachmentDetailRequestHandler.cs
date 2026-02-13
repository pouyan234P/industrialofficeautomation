using AutoMapper;
using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.Application.Feature.AttachmentFeature.request.Queries;
using CorrespondenceCore.Application.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.AttachmentFeature.handles.Queries
{
    public class getAttachmentDetailRequestHandler : IRequestHandler<getAttachmentDetailRequest, AttachmentDTO>
    {
        private readonly IattachmentRepository _repository;
        private readonly IMapper _mapper;

        public getAttachmentDetailRequestHandler(IattachmentRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<AttachmentDTO> Handle(getAttachmentDetailRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.Get(request.id);
            var resultmap=_mapper.Map<AttachmentDTO>(result);
            return resultmap;
        }
    }
}
