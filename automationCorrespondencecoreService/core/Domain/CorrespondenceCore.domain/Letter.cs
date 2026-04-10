using CorrespondenceCore.domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.domain
{
    public class Letter
    {
        public int ID { get; set; }
        public string? Subject { get; set; }
        public string? Abstract { get; set; }
        public string? LetterNo { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Priority priority { get; set; }
        public Confidentiality confidentiality { get; set; }
        public Typecorrespondence type { get; set; }
        public string? BodyHTMLID { get; set; }
        public Attachment? attachmentID { get; set; }
        // چه کسی این نامه را ایجاد کرده است؟ (مالک پیش‌نویس)
        public int CreatorPositionID { get; set; }

        // آیا این نامه هنوز پیش‌نویس است؟ 
        // (می‌توانید از این فیلد استفاده کنید یا فقط چک کنید که SentDate مساوی Null باشد)
        public bool IsDraft { get; set; } = true;

        public int? ReplyToLetterID { get; set; }

        // Navigation Property (اختیاری در EF Core برای گرفتن اطلاعات نامه والد)
        public virtual Letter? ParentLetter { get; set; }
    }
}
