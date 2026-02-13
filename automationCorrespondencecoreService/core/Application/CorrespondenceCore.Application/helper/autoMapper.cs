using AutoMapper;
using CorrespondenceCore.Application.DTO;
using CorrespondenceCore.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.helper
{
    public class autoMapper:Profile
    {
        public autoMapper()
        {
            CreateMap<Attachment,AttachmentDTO>();
            CreateMap<Letter,LetterDTO>();
            CreateMap<setAttachmentDTO,Attachment>();
            CreateMap<setLetterDTO, Letter>();
        }
    }
}
