using CorrespondenceCore.Application.DTO.Coomon;
using CorrespondenceCore.Application.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.DTO
{
    public class LetterDTO:baseDTO
    {
        public string? Subject { get; set; }
        public string? Abstract { get; set; }
        public string? LetterNo { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public PriorityDTO priority { get; set; }
        public ConfidentialityDTO confidentiality { get; set; }
        public TypeDTO type { get; set; }
        public string? BodyHTML { get; set; }
        public AttachmentDTO? attachmentID { get; set; }
        // چه کسی این نامه را ایجاد کرده است؟ (مالک پیش‌نویس)
        public int CreatorPositionID { get; set; }

        // آیا این نامه هنوز پیش‌نویس است؟ 
        // (می‌توانید از این فیلد استفاده کنید یا فقط چک کنید که SentDate مساوی Null باشد)
        public bool IsDraft { get; set; }
        public int? ReplyToLetterID { get; set; }
    }
}
