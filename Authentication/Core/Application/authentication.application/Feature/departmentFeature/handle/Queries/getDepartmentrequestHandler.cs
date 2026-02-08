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
    public class getDepartmentrequestHandler : IRequestHandler<GetDepartmentrequest, IEnumerable<DepartmentDTO>>
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;

        public getDepartmentrequestHandler(IDepartmentRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DepartmentDTO>> Handle(GetDepartmentrequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetDepartment();
            var resultreturn=_mapper.Map<IEnumerable<DepartmentDTO>>(result);
            return resultreturn;
        }
    }
}
