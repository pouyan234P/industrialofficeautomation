using authentication.application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.authfeature.request.Commands
{
    public class createRoleCommand:IRequest<baseCommandResponse>
    {
        public string RoleName { get; set; }
    }
}
