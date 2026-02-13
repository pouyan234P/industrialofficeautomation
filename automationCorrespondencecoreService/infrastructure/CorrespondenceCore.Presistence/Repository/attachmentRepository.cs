using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Presistence.Repository
{
    public class attachmentRepository:genericRepository<Attachment>,IattachmentRepository
    {
        private readonly CorrespondenceCoreDB _db;

        public attachmentRepository(CorrespondenceCoreDB db):base(db) 
        {
            _db = db;
        }
    }
}
