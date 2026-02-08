using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.authfeature.request.Queries
{
    public class loginRequest:IRequest<string>
    {
        public string? email { get; set; }
        public string? password { get; set; }
    }
}
