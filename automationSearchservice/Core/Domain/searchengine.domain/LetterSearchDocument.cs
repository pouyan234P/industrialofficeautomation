using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchengine.domain
{
    public class LetterSearchDocument
    {
        public int LetterId { get; set; }          // شناسه اصلی نامه در SQL
        public string LetterNo { get; set; }       // شماره نامه (مثلا 1402/5001)
        public string? Subject { get; set; }        // موضوع نامه
        public string? Abstract { get; set; }       // خلاصه
        public string? PlainTextBody { get; set; }  // متن نامه (تگ‌های HTML باید قبل از ذخیره حذف شوند)
        public string? OcrContent { get; set; }     // متن استخراج شده از عکس‌ها/PDF پیوست

        // متادیتا برای فیلتر کردن سریع
        public DateTime? CreatedDate { get; set; }
        public int SenderDepartmentId { get; set; }
        public int CreatorPositionId { get; set; }
        public string? LetterType { get; set; }     // Internal, Incoming, Outgoing
    }
}
