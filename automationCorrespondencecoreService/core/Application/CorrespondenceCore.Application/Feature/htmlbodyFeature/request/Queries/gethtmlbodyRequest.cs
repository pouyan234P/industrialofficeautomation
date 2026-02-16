using MediatR;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.htmlbodyFeature.request.Queries
{
    public class gethtmlbodyRequest:IRequest<BsonDocument>
    {
        public string id { get; set; }
    }
}
