using authentication.application.DTO;
using authentication.application.Feature.departmentFeature.request.Queries;
using authentication.application.IRepository;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.departmentFeature.handle.Queries
{
    public class GetDepartmentbyidrequestHandler : IRequestHandler<GetDepartmentbyidrequest, DepartmentDTO>
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;

        public GetDepartmentbyidrequestHandler(IDepartmentRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<DepartmentDTO> Handle(GetDepartmentbyidrequest request, CancellationToken cancellationToken)
        {
            var result=await _repository.GetDepartmentbyid(request.Id);
            var resultdto=_mapper.Map<DepartmentDTO>(result);
            return resultdto;
        }
    }
}
