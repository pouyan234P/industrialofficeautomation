using AutoMapper;
using MediatR;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.DTO;
using workflow.Appliction.Feature.referralFeature.request.Queries;
using workflow.Appliction.IRepository;
using workflow.domain;

namespace workflow.Appliction.Feature.referralFeature.handles.Queries
{
    public class getAllbySenderpositionRequestHandler : IRequestHandler<getAllbySenderpositionRequest, IEnumerable<referralDTO>>
    {
        private readonly IReferralRepository _repository;
        private readonly IMapper _mapper;

        public getAllbySenderpositionRequestHandler(IReferralRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<referralDTO>> Handle(getAllbySenderpositionRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.getAllbySenderposition(request.id);
            var myref = result.Select(doc => BsonSerializer.Deserialize<Referral>(doc));
            var map = _mapper.Map<IEnumerable<referralDTO>>(myref.ToList());
            return map;
        }
    }
}
