using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.DTO;
using workflow.Appliction.Responses;

namespace workflow.Appliction.Feature.referralFeature.request.Command
{
    public class createReferralCommand:IRequest<baseCommandResponse>
    {
        public setReferralDTO?  setReferral{ get; set; }
    }
}
