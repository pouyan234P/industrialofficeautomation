using CorrespondenceCore.Application.Feature.htmlbodyFeature.request.Commands;
using CorrespondenceCore.Application.IRepository;
using CorrespondenceCore.Application.Responses;
using MediatR;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorrespondenceCore.Application.Feature.htmlbodyFeature.handles.Commands
{
    public class createhtmlbodyCommandHandler : IRequestHandler<createhtmlbodyCommand, BsonDocument>
    {
        private readonly IhtmlbodyMongoRepository _repository;

        public createhtmlbodyCommandHandler(IhtmlbodyMongoRepository repository)
        {
            _repository = repository;
        }
        public async Task<BsonDocument> Handle(createhtmlbodyCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.create(request.elements!);
            return result;
        }
    }
}
