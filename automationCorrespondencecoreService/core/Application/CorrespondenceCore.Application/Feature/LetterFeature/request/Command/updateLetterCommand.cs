using CorrespondenceCore.Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.LetterFeature.request.Command
{
    public class updateLetterCommand:IRequest<Unit>
    {
        public LetterDTO letterDTO { get; set; }
    }
}
