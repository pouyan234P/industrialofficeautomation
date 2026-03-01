using authentication.application.DTO;
using authentication.application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication.application.Feature.positionRepository.request.Commands
{
    public class createPositionCommand:IRequest<baseCommandResponse>
    {
        public setPosition? positionDTO { get; set; }
    }
}
