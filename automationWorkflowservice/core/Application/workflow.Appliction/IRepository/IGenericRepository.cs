using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Shared.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Application.helper;

namespace workflow.Appliction.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<BsonDocument> Get(string id);
        Task<IEnumerable<BsonDocument>> GetAllbyposition(string SenderPositionID);
        Task<(IEnumerable<BsonDocument> Items, int TotalCount)> GetAllbyreciverposition(string reciverPositionID,UserParams userParams);
        Task<(IEnumerable<BsonDocument> Items, int TotalCount)> getAllbySenderposition(string SenderPositionID, UserParams userParams);
        Task<IEnumerable<BsonDocument>> GetAll();
        Task<BsonDocument> Add(BsonDocument entity);
        Task<bool> Exists(int id);
        Task Update(BsonDocument entity);
        Task Delete(string id);
    }
}
