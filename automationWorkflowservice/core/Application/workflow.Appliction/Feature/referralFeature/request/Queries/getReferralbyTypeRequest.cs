using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Application.DTO.Enum;
using workflow.Appliction.DTO;
using workflow.domain.Enum;

namespace workflow.Appliction.Feature.referralFeature.request.Queries
{
    public class getReferralbyTypeRequest:IRequest<IEnumerable<referralDTO>>
    {
        public TypeDTO mytype { get; set; }
        public string reciverID { get; set; }
    }
}
