using authentication.application.DTO;
using authentication.application.Feature.imagefeature.request.Queries;
using authentication.application.IRepository;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.imagefeature.handles.Queries
{
    public class getimageRequestHandler : IRequestHandler<getimageRequest, getsignitureimageDTO>
    {
        private readonly IPictureRepository _repository;
        private readonly IMapper _mapper;

        public getimageRequestHandler(IPictureRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<getsignitureimageDTO> Handle(getimageRequest request, CancellationToken cancellationToken)
        {
            var picture=await _repository.getimage(request.id);
            var picturemap=_mapper.Map<getsignitureimageDTO>(picture);
            return picturemap;
        }
    }
}
