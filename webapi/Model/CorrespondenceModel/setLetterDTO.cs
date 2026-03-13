
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Model.CorrespondenceModel.Enum;

namespace webapi.Model.CorrespondenceModel
{
    public class setLetterDTO
    {
        public string? Subject { get; set; }
        public string? Abstract { get; set; }
        public string? LetterNo { get; set; }
        public DateTime SentDate { get; set; }
        public PriorityDTO priority { get; set; }
        public ConfidentialityDTO confidentiality { get; set; }
        public TypeDTO type { get; set; }
        public object? BodyHTML { get; set; }
        public AttachmentDTO? attachmentID { get; set; }
        public int CreatorPositionID { get; set; }

        // آیا این نامه هنوز پیش‌نویس است؟ 
        // (می‌توانید از این فیلد استفاده کنید یا فقط چک کنید که SentDate مساوی Null باشد)
        public bool? IsDraft { get; set; } = true;
    }
}
