using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workflow.Appliction.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<BsonDocument> Get(string id);
        Task<IEnumerable<BsonDocument>> GetAllbyposition(string SenderPositionID);
        Task<IEnumerable<BsonDocument>> GetAllbyreciverposition(string reciverPositionID);
        Task<IEnumerable<BsonDocument>> getAllbySenderposition(string SenderPositionID);
        Task<IEnumerable<BsonDocument>> GetAll();
        Task<BsonDocument> Add(BsonDocument entity);
        Task<bool> Exists(int id);
        Task Update(BsonDocument entity);
        Task Delete(string id);
    }
}
