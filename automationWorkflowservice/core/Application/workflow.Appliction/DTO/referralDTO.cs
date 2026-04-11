using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Application.DTO.Enum;
using workflow.Appliction.DTO.Common;
using workflow.Appliction.DTO.Enum;

namespace workflow.Appliction.DTO
{
    public class referralDTO:baseDTO
    {
        public int LetterID { get; set; }
        public string? ParentReferralId { get; set; }

        // کپی شده برای سرعت (Denormalized)
        public string LetterSubject { get; set; }
        public string LetterNo { get; set; }
        public priorityDTO priority { get; set; }

        public TypeDTO type { get; set; }

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
        public actionTypeDTO ActionType { get; set; } // جهت اقدام، امضا، استحضار

        [BsonRepresentation(BsonType.String)]
        public referralStatusDTO Status { get; set; } // خوانده نشده (Unread)، مشاهده شد (Viewed)، بایگانی (Archived)

        public string Paraph { get; set; } // متن هامش (دستور مدیر)

        // --- زمان‌بندی ---
        public DateTime Timestamp { get; set; }      // زمان دقیق ارسال
        public DateTime? ViewDate { get; set; }      // زمان دقیق دیده شدن (برای تیک دوم)
        public DateTime? ActionDate { get; set; }    // زمانی که گیرنده کار را تمام کرد
        public DateTime? Deadline { get; set; }
    }
}
