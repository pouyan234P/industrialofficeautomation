using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.Presistence.helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Presistence.Repository
{
    public class htmlbodyMongoRepository : IhtmlbodyMongoRepository
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public htmlbodyMongoRepository(IOptions<Mongosettings> configuration)
        {
            _mongoClient = new MongoClient(configuration.Value.Connection);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
        }
        public async Task<BsonDocument> create(BsonDocument data)
        {
            var collection = _db.GetCollection<BsonDocument>("myhtmlcollection");
            await collection.InsertOneAsync(data);
            return await collection.Find(data).FirstOrDefaultAsync();
        }

        public async Task<BsonDocument> gethtmlbody(string id)
        {
            var collection = _db.GetCollection<BsonDocument>("myhtmlcollection");
            var filter=new BsonDocument ( "_id",ObjectId.Parse(id));
            var htmlbody=await collection.Find(filter).FirstOrDefaultAsync();
            return htmlbody;
        }
    }
}
