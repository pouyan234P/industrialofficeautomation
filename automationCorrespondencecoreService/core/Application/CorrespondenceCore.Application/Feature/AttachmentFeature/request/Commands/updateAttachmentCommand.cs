using CorrespondenceCore.Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.AttachmentFeature.request.Commands
{
    public class updateAttachmentCommand:IRequest<Unit>
    {
        public AttachmentDTO? attachmentDTO { get; set; }
    }
}
