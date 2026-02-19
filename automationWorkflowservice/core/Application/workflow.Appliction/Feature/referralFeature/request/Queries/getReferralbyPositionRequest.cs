using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.DTO;

namespace workflow.Appliction.Feature.referralFeature.request.Queries
{
    public class getReferralbyPositionRequest:IRequest<IEnumerable<referralDTO>>
    {
        public required string SenderPositionID { get; set; }
    }
}
