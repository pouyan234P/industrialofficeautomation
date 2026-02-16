using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.IRepository
{
    public interface IhtmlbodyMongoRepository
    {
        public Task<BsonDocument> create(BsonDocument data);
        public Task<BsonDocument> gethtmlbody(string id);
    }
}
