using authentication.application.DTO;
using authentication.application.Feature.imagefeature.request.Commands;
using authentication.application.IRepository;
using authentication.domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.imagefeature.handles.Commands
{
    public class CreateImageCommandHandler : IRequestHandler<createimageCommand, getsignitureimageDTO>
    {
        private readonly IPictureRepository _repository;
        private readonly IMapper _mapper;

        public CreateImageCommandHandler(IPictureRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<getsignitureimageDTO> Handle(createimageCommand request, CancellationToken cancellationToken)
        {
            var insertimage = new signitureimage
            {
                FileName = request.signitureimageDTO.FileName,
                ContentType = request.signitureimageDTO.ContentType,
                ImageData = request.signitureimageDTO.ImageData
            };
            var result = await _repository.addimage(insertimage);
            var resultmap = _mapper.Map<getsignitureimageDTO>(result);
            resultmap.ImageData = null;
            return resultmap;
        }
    }
}
