using authentication.application.DTO;
using MediatR;

namespace authentication.application.Feature.authfeature.request.Queries
{
    public class getRolesRequest : IRequest<IEnumerable<roleDTO>>
    {
    }
}
