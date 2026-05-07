using MediatR;
using Shared.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Application.DTO.Enum;
using workflow.Application.helper;
using workflow.Appliction.DTO;
using workflow.domain.Enum;

namespace workflow.Appliction.Feature.referralFeature.request.Queries
{
    public class getReferralbyTypeRequest:IRequest<PagedList<referralDTO>>
    {
        public TypeDTO mytype { get; set; }
        public string reciverID { get; set; }
        public UserParams UserParams { get; set; }
    }
}
