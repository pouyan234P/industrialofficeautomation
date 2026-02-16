using CorrespondenceCore.Application.Responses;
using MediatR;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.htmlbodyFeature.request.Commands
{
    public class createhtmlbodyCommand:IRequest<BsonDocument>
    {
        public BsonDocument? elements { get; set; }
    }
}
