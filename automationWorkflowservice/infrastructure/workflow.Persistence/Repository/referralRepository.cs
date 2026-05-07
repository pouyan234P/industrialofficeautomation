using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Application.DTO.Enum;
using workflow.Application.helper;
using workflow.Appliction.IRepository;
using workflow.domain;
using workflow.domain.Enum;
using workflow.Persistence.helper;

namespace workflow.Persistence.Repository
{
    public class referralRepository:genericRepository<BsonDocument>,IReferralRepository
    {
        private readonly IOptions<Mongosettings> _configuration;
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public referralRepository(IOptions<Mongosettings> configuration):base(configuration) 
        {
            _configuration = configuration;
            _mongoClient = new MongoClient(configuration.Value.Connection);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
        }

        public async Task<(IEnumerable<BsonDocument> Items, int TotalCount)> getRefferalBytpeandreciverid(string reciverid, type mytype, UserParams userParams)
        {
            var collection = _db.GetCollection<BsonDocument>("myreferraldocument");
            var filter = Builders<BsonDocument>.Filter.Eq("ReceiverPositionID", int.Parse(reciverid));
            var filter2 = Builders<BsonDocument>.Filter.Eq("type", mytype);
            var combinedFilter = Builders<BsonDocument>.Filter.And(filter, filter2);
            // 1. Get the total count asynchronously
            var count = await collection.CountDocumentsAsync(combinedFilter);

            // 2. Fetch the specific page of items
            var items = await collection.Find(filter)
                                        .Skip((userParams.PageNumber - 1) * userParams.pageSize)
                                        .Limit(userParams.pageSize)
                                        .ToListAsync();

            // 3. Return a Tuple containing the raw data and the count. NO PagedList here!
            return (items, (int)count);
        }

        public async Task<(IEnumerable<BsonDocument> Items, int TotalCount)> getRefferalBytpeandsenderid(string senderid, type mytype, UserParams userParams)
        {
            var collection = _db.GetCollection<BsonDocument>("myreferraldocument");
            var filter = Builders<BsonDocument>.Filter.Eq("SenderPositionID", int.Parse(senderid));
            var filter2 = Builders<BsonDocument>.Filter.Eq("type", mytype);
            var combinedFilter = Builders<BsonDocument>.Filter.And(filter, filter2);
            // 1. Get the total count asynchronously
            var count = await collection.CountDocumentsAsync(combinedFilter);

            // 2. Fetch the specific page of items
            var items = await collection.Find(filter)
                                        .Skip((userParams.PageNumber - 1) * userParams.pageSize)
                                        .Limit(userParams.pageSize)
                                        .ToListAsync();

            // 3. Return a Tuple containing the raw data and the count. NO PagedList here!
            return (items, (int)count);
        }
    }
}
