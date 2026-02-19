using AutoMapper;
using MediatR;
using MongoDB.Bson;
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
    public class getReferralbyPositionRequestHandler : IRequestHandler<getReferralbyPositionRequest, IEnumerable<referralDTO>>
    {
        private readonly IReferralRepository _repository;
        private readonly IMapper _mapper;

        public getReferralbyPositionRequestHandler(IReferralRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<referralDTO>> Handle(getReferralbyPositionRequest request, CancellationToken cancellationToken)
        {
            var result =await _repository.GetAllbyposition(request.SenderPositionID);
            var myref =result.Select(doc => BsonSerializer.Deserialize<Referral>(doc)).ToList();
            var map = _mapper.Map<IEnumerable<referralDTO>>(myref);
            return map;
        }
    }
}
