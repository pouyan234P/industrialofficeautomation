using authentication.application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.authfeature.request.Queries
{
    public class getAllRequest:IRequest<IEnumerable<getuserDTO>>
    {
    }
}
