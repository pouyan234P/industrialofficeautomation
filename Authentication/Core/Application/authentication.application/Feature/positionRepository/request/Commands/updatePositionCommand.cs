using authentication.application.DTO;
using authentication.application.Responses;
using MediatR;

namespace authentication.application.Feature.positionRepository.request.Commands
{
    public class updatePositionCommand : IRequest<baseCommandResponse>
    {
        public int id { get; set; }
        public setPosition? positionDTO { get; set; }
    }
}
