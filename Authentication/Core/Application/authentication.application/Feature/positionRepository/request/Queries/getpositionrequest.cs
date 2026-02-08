using authentication.application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.positionRepository.request.Queries
{
    public class getpositionrequest: IRequest<getPosition>
    {
        public int id { get; set; }
    }
}
