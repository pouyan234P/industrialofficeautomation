
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Model.CorrespondenceModel.Enum;

namespace webapi.Model.CorrespondenceModel
{
    public class setAttachmentDTO
    {
        public string? FilePath { get; set; }
        public FileType MyProperty { get; set; }
    }
}
