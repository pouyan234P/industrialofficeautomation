using CorrespondenceCore.Application.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.DTO
{
    public class setAttachmentDTO
    {
        public string? FilePath { get; set; }
        public FileType MyProperty { get; set; }
    }
}
