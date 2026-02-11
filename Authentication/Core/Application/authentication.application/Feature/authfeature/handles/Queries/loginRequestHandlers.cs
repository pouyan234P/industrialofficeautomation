using authentication.application.Feature.authfeature.request.Queries;
using authentication.application.IRepository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.authfeature.handles.Queries
{
    public class loginRequestHandlers : IRequestHandler<loginRequest, string>
    {
        private readonly IAuthformRepository _authform;

        public loginRequestHandlers(IAuthformRepository authform)
        {
            _authform = authform;
        }
        public async Task<string> Handle(loginRequest request, CancellationToken cancellationToken)
        {
            var login=await _authform.login(request.myslogindto!.Email!,request.myslogindto.Password!);
            return login ?? "it not valid";
        }
    }
}
