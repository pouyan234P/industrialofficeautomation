using MediatR;
using Shared.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Application.helper;
using workflow.Appliction.DTO;

namespace workflow.Appliction.Feature.referralFeature.request.Queries
{
    public class getAllbySenderpositionRequest:IRequest<PagedList<referralDTO>>
    {
        public required string id;
        public UserParams userparams { get; set; }
    }
}
