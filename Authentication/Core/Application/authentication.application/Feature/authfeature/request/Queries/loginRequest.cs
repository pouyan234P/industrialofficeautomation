using authentication.application.DTO;
using MediatR;

namespace authentication.application.Feature.authfeature.request.Queries
{
    public class loginRequest : IRequest<loginResultDTO>
    {
        public loginDTO? myslogindto { get; set; }
    }
}
