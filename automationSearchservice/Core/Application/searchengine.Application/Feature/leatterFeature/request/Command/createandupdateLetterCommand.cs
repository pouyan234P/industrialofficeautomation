using MediatR;
using searchengine.Application.Responses;
using searchengine.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchengine.Application.Feature.leatterFeature.request.Command
{
    public class createandupdateLetterCommand:IRequest<baseCommandResponse>
    {
        public LetterSearchDocument? document{ get; set; }
    }
}
