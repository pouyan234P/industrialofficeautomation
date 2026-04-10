using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.DTO;

namespace workflow.Appliction.Feature.referralFeature.request.Command
{
    public class updateReferralCommand:IRequest
    {
        public referralDTO referral { get; set; }
    }
}
