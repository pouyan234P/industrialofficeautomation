using CorrespondenceCore.Application.DTO.Enum;
using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.domain;
using CorrespondenceCore.domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Presistence.Repository
{
    public class GenerateNextNumberAsyncRepository : IGenerateNextNumberAsyncRepository
    {
        private readonly CorrespondenceCoreDB _db;

        public GenerateNextNumberAsyncRepository(CorrespondenceCoreDB db)
        {
            _db = db;
        }
        public async Task<string> mynextnumber(TypeDTO type, int year, int? deptId = 0)
        {
            using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            
            try
            {
                IndicatorBook? indicator;

                // ---------------------------------------------------------
                // اصلاح منطق NULL: جدا کردن شرط‌ها برای تولید SQL صحیح
                // ---------------------------------------------------------
                if (deptId!=0)
                {
                    // سناریوی ۱: مربوط به یک دپارتمان خاص
                    // SQL Generated: WHERE DeptID = @deptId
                    indicator = await _db.indicatorBook
                        .FirstOrDefaultAsync(x => x.FiscalYear == year
                                               && x.type ==(Typecorrespondence)type
                                               && x.DepartmentID == deptId);
                }
                else
                {
                    // سناریوی ۲: مربوط به دبیرخانه مرکزی (بدون دپارتمان)
                    // SQL Generated: WHERE DeptID IS NULL
                    // نکته: اینجا صریحاً با null مقایسه می‌کنیم تا EF تبدیل به IS NULL کند
                    indicator = await _db.indicatorBook
                        .FirstOrDefaultAsync(x => x.FiscalYear == year
                                               && x.type == (Typecorrespondence)type
                                               && x.DepartmentID == 0);
                }

                // ---------------------------------------------------------
                // ادامه منطق یکسان است
                // ---------------------------------------------------------

                // اگر وجود نداشت، بساز
                if (indicator == null)
                {
                    indicator = new IndicatorBook
                    {
                        FiscalYear = year,
                        type =(Typecorrespondence) type,
                        DepartmentID =(int) deptId!, // اینجا اگر null باشد، در دیتابیس NULL ذخیره می‌شود که درست است
                        LastNumber = 0
                    };

                    await _db.indicatorBook.AddAsync(indicator);
                    await _db.SaveChangesAsync();
                }

                // افزایش شمارنده
                indicator.LastNumber += 1;

                // ذخیره نهایی
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                // فرمت دهی خروجی
                var serial = indicator.LastNumber.ToString("D4");

                if (deptId.HasValue)
                {
                    return $"{year}/{deptId}/{serial}";
                }
                else
                {
                    return $"{year}/{serial}";
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
