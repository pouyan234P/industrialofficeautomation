using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Application.DTO.Enum;
using workflow.domain;
using workflow.domain.Enum;

namespace workflow.Appliction.IRepository
{
    public interface IReferralRepository:IGenericRepository<Referral>
    {
        Task<IEnumerable<BsonDocument>> getRefferalBytpeandreciverid(string reciverid,type mytype);
    }
}
