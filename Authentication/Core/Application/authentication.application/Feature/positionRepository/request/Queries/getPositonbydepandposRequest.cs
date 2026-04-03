using authentication.application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.positionRepository.request.Queries
{
    public class getPositonbydepandposRequest:IRequest<getPosition>
    {
        public int depid { get; set; }
        public int posid { get; set; }
    }
}
