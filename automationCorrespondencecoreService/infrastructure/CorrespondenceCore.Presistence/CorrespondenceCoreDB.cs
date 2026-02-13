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
    }
}
