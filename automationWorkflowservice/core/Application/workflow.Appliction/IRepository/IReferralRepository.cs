using MongoDB.Bson;
using Shared.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Application.DTO.Enum;
using workflow.Application.helper;
using workflow.domain;
using workflow.domain.Enum;

namespace workflow.Appliction.IRepository
{
    public interface IReferralRepository:IGenericRepository<Referral>
    {
        Task<(IEnumerable<BsonDocument> Items, int TotalCount)> getRefferalBytpeandreciverid(string reciverid,type mytype, UserParams userParams);
        Task<(IEnumerable<BsonDocument> Items, int TotalCount)> getRefferalBytpeandsenderid(string senderid,type mytype, UserParams userParams);

    }
}
