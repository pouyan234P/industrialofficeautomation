using authentication.application.Feature.departmentFeature.request.Commands;
using authentication.application.IRepository;
using authentication.application.Responses;
using authentication.domain;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.departmentFeature.handle.Commands
{
    public class createDepartmentCommandHandler : IRequestHandler<createDepartmentCommand, baseCommandResponse>
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public createDepartmentCommandHandler(IDepartmentRepository departmentRepository,IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        public async Task<baseCommandResponse> Handle(createDepartmentCommand request, CancellationToken cancellationToken)
        {
            var response = new baseCommandResponse();
            var dtoreturn=_mapper.Map<Department>(request.departmentDTO);
            var result=await _departmentRepository.addDepartment(dtoreturn);
            if(result==true)
            {
                response.Success = true;
                response.Message = "created successfully";
                response.id = 1;
            }
            return response;
        }
    }
}
