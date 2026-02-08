using authentication.application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.departmentFeature.request.Queries
{
    public class GetDepartmentbyidrequest:IRequest<DepartmentDTO>
    {
        public int Id { get; set; }
    }
}
