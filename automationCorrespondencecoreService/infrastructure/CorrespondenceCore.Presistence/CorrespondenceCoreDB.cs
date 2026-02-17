using CorrespondenceCore.domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Presistence
{
    public class CorrespondenceCoreDB:DbContext
    {
        public CorrespondenceCoreDB(DbContextOptions<CorrespondenceCoreDB> options):base(options)
        {
            
        }

        public DbSet<Attachment> attachments { get; set; }
        public DbSet<Letter> letters { get; set; }
        public DbSet<IndicatorBook> indicatorBook { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ... سایر تنظیمات

            // تنظیمات جدول اندیکاتور
            modelBuilder.Entity<IndicatorBook>(entity =>
            {
                // الف) تعریف کلید اصلی
                entity.HasKey(e => e.Id);

                // ب) ایندکس یکتا (Composite Unique Index)
                // این خط حیاتی است: نمی‌گذارد برای (سال + تایپ + دپارتمان) دو ردیف ساخته شود
                entity.HasIndex(e => new { e.FiscalYear, e.type, e.DepartmentID })
                      .IsUnique();

                // ج) مقدار پیش‌فرض برای LastNumber
                entity.Property(e => e.LastNumber)
                      .HasDefaultValue(0);
            });
        }
    }
}
