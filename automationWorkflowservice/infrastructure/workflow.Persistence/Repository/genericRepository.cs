using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.IRepository;
using workflow.Persistence.helper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace workflow.Persistence.Repository
{
    public class genericRepository<T> : IGenericRepository<T> where T : class
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public genericRepository(IOptions<Mongosettings> configuration)
        {
            _mongoClient = new MongoClient(configuration.Value.Connection);
            _db = _mongoClient.GetDatabase(configuration.Value.DatabaseName);
        }

        public async Task<BsonDocument> Get(string id)
        {
            var collection = _db.GetCollection<BsonDocument>("myreferraldocument");
            var filter = new BsonDocument("_id", ObjectId.Parse(id));
            var referral = await collection.Find(filter).FirstOrDefaultAsync();
            return referral;
        }

        public async Task<IEnumerable<BsonDocument>> GetAllbyposition(string SenderPositionID)
        {
            var collection = _db.GetCollection<BsonDocument>("myreferraldocument");
            var filter = new BsonDocument("SenderPositionID", int.Parse(SenderPositionID));
            var referral = await collection.Find(filter).ToListAsync();
            return referral;
        }

        public async Task<IEnumerable<BsonDocument>> GetAll()
        {
            var collection = _db.GetCollection<BsonDocument>("myreferraldocument");
            var referral = await collection.Find(new BsonDocument()).ToListAsync();
            return referral;
        }

        public async Task<BsonDocument> Add(BsonDocument entity)
        {
            var collection = _db.GetCollection<BsonDocument>("myreferraldocument");
            await collection.InsertOneAsync(entity);
            return await collection.Find(entity).FirstOrDefaultAsync();
        }

       

        public Task<bool> Exists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(BsonDocument entity)
        {
            var collection = _db.GetCollection<BsonDocument>("myreferraldocument");
            if (!entity.Contains("_id"))
            {
                throw new ArgumentException("برای بروزرسانی، فیلد _id الزامی است.");
            }

            var id = entity["_id"];

            // ۲. ایجاد فیلتر برای پیدا کردن رکورد مورد نظر
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);

            // ۳. آماده‌سازی فیلدهایی که باید تغییر کنند
            // ما _id را از لیست فیلدهای آپدیت حذف می‌کنیم چون _id در مونگو غیرقابل تغییر است
            var updateDefinition = new BsonDocument(entity);
            updateDefinition.Remove("_id");

            // استفاده از دستور $set برای اینکه فقط فیلدهای ارسالی تغییر کنند و مابقی ثابت بمانند
            var update = new BsonDocument("$set", updateDefinition);

            // ۴. اجرای عملیات در دیتابیس
            await collection.UpdateOneAsync(filter, update);
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BsonDocument>> GetAllbyreciverposition(string reciverPositionID)
        {
            var collection = _db.GetCollection<BsonDocument>("myreferraldocument");
            var filter = new BsonDocument("ReceiverPositionID", int.Parse(reciverPositionID));
            var referral = await collection.Find(filter).ToListAsync();
            return referral;
        }
    }
}
