using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.DTO;
using workflow.domain;

namespace workflow.Appliction.helper
{
    public class autoMapper:Profile
    {
        public autoMapper()
        {
            CreateMap<Referral, referralDTO>().ReverseMap();
            CreateMap<setReferralDTO, Referral>();
        }
    }
}
