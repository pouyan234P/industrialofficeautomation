using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.Application.DTO.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.LetterFeature.request.Queries
{
    public class getLetterbyTypeRequest:IRequest<IEnumerable<LetterDTO>>
    {
        public TypeDTO type { get; set; }
    }
}
