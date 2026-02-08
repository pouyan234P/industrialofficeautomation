using authentication.application.DTO;
using authentication.application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.departmentFeature.request.Commands
{
    public class createDepartmentCommand:IRequest<baseCommandResponse>
    {
        public DepartmentDTO? departmentDTO { get; set; }
    }
}
