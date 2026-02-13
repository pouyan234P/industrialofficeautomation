using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.LetterFeature.request.Command
{
    public class createLetterCommand:IRequest<baseCommandResponse>
    {
        public setLetterDTO? setLetterDTO { get; set; }
    }
}
