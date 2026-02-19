using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.IRepository;
using workflow.Persistence.helper;

namespace workflow.Persistence.Repository
{
    public class referralRepository:genericRepository<BsonDocument>,IReferralRepository
    {
        private readonly IOptions<Mongosettings> _configuration;

        public referralRepository(IOptions<Mongosettings> configuration):base(configuration) 
        {
            _configuration = configuration;
        }
    }
}
