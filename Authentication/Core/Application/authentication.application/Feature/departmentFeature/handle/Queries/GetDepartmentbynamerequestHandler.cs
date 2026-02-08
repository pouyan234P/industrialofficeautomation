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
    public class GetDepartmentbynamerequestHandler : IRequestHandler<GetDepartmentbynamerequest, DepartmentDTO>
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;

        public GetDepartmentbynamerequestHandler(IDepartmentRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<DepartmentDTO> Handle(GetDepartmentbynamerequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetDepartmentbyname(request.name!);
            var resultreturn=_mapper.Map<DepartmentDTO>(result);
            return resultreturn;
        }
    }
}
