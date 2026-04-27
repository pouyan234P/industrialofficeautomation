using CorrespondenceCore.Application.DTO.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.GenerateNextNumberAsyncFeature.request.Queries
{
    public class getlastNumberbyTypeRequest:IRequest<long>
    {
        public TypeDTO type { get; set; }
        public int depid { get; set; }
        public int year { get; set; }
    }
}
