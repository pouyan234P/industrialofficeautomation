using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.domain.Enum;

namespace workflow.domain
{
    public class Referral
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        // --- زنجیره ارجاعات ---
        // کلید طلایی برای ردیابیِ اینکه این ارجاع در نتیجه‌یِ یک Forward یا Reject ساخته شده است
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentReferralId { get; set; }

        // --- ارتباط با نامه (از SQL) ---
        public int LetterID { get; set; }

        // کپی شده برای سرعت (Denormalized)
        public string LetterSubject { get; set; }
        public string LetterNo { get; set; }
        public Priority Priority { get; set; }    // آنی، فوری، عادی

        public type type { get; set; }

        // --- ارتباط با فرستنده (از SQL - Position) ---
        public int SenderPositionID { get; set; }
        public string SenderName { get; set; }    // نام شخص در لحظه ارسال (مثلاً: علی رضایی)
        public string SenderTitle { get; set; }   // سمت در لحظه ارسال (مثلاً: مدیر فنی)

        // --- ارتباط با گیرنده (از SQL - Position) ---
        public int ReceiverPositionID { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverTitle { get; set; }

        // --- جزئیات عملیاتی ---
        [BsonRepresentation(BsonType.String)]
        public ActionType ActionType { get; set; } // جهت اقدام، امضا، استحضار

        [BsonRepresentation(BsonType.String)]
        public ReferralStatus Status { get; set; } // خوانده نشده (Unread)، مشاهده شد (Viewed)، بایگانی (Archived)

        public string Paraph { get; set; } // متن هامش (دستور مدیر)

        // --- زمان‌بندی ---
        public DateTime Timestamp { get; set; }      // زمان دقیق ارسال
        public DateTime? ViewDate { get; set; }      // زمان دقیق دیده شدن (برای تیک دوم)
        public DateTime? ActionDate { get; set; }    // // لحظه‌ای که Status به 2 یا 4 تغییر کرد
        public DateTime? Deadline { get; set; }      // مهلت اقدام
    }
}
