using CorrespondenceCore.Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.AttachmentFeature.request.Queries
{
    public class getAttachmentDetailRequest:IRequest<AttachmentDTO>
    {
        public int id { get; set; }
    }
}
