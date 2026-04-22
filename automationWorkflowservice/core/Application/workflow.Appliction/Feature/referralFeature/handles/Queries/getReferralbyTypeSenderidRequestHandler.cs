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
using workflow.domain.Enum;

namespace workflow.Appliction.Feature.referralFeature.handles.Queries
{
    public class getReferralbyTypeSenderidRequestHandler : IRequestHandler<getReferralbyTypeSenderidRequest, IEnumerable<referralDTO>>
    {
        private readonly IReferralRepository _repository;
        private readonly IMapper _mapper;

        public getReferralbyTypeSenderidRequestHandler(IReferralRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<referralDTO>> Handle(getReferralbyTypeSenderidRequest request, CancellationToken cancellationToken)
        {
            var newtype = (type)request.mytype;
            var referral = await _repository.getRefferalBytpeandsenderid(request.senderid, newtype);
            var myref = referral.Select(doc => BsonSerializer.Deserialize<Referral>(doc));
            var referralmap = _mapper.Map<IEnumerable<referralDTO>>(myref.ToList());
            return referralmap;
        }
    }
}
