using CorrespondenceCore.Application.Feature.htmlbodyFeature.request.Queries;
using CorrespondenceCore.Application.IRepository;
using MediatR;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.htmlbodyFeature.handles.Queries
{
    public class gethtmlbodyRequestHandler : IRequestHandler<gethtmlbodyRequest, BsonDocument>
    {
        private readonly IhtmlbodyMongoRepository _repository;

        public gethtmlbodyRequestHandler(IhtmlbodyMongoRepository repository)
        {
            _repository = repository;
        }
        public async Task<BsonDocument> Handle(gethtmlbodyRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.gethtmlbody(request.id);
            return result;
        }
    }
}
