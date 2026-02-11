using authentication.application.Feature.authfeature.request.Commands;
using authentication.application.IRepository;
using authentication.application.Responses;
using authentication.domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.authfeature.handles.Commands
{
    public class createRoleCommandHandler : IRequestHandler<createRoleCommand, baseCommandResponse>
    {
        private readonly IAuthformRepository _authform;

        public createRoleCommandHandler(IAuthformRepository authform) 
        {
            _authform = authform;
        }
        public async Task<baseCommandResponse> Handle(createRoleCommand request, CancellationToken cancellationToken)
        {
            var response= new baseCommandResponse();
            var returntype = await _authform.createRole(request.RoleName);
            if (returntype != null)
            {
                response.Success = true;
                response.Message = "Creation was Successful";
                
            }
            return response;
        }
    }
}
