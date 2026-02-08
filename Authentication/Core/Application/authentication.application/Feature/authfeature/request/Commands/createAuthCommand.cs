using authentication.application.DTO;
using authentication.application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.authfeature.request.Commands
{
    public class createAuthCommand:IRequest<baseCommandResponse>
    {
        public registerDTO registerDTO { get; set; }
    }
}
