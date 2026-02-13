using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.AttachmentFeature.request.Commands
{
    public class createAttachmentCommand: IRequest<baseCommandResponse>
    {
        public setAttachmentDTO? setAttachmentDTO { get; set; }
    }
}
