using AutoMapper;
using MediatR;
using MongoDB.Bson.Serialization;
using Shared.Infrastructure;
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
    public class getReferralbyTypeSenderidRequestHandler : IRequestHandler<getReferralbyTypeSenderidRequest, PagedList<referralDTO>>
    {
        private readonly IReferralRepository _repository;
        private readonly IMapper _mapper;

        public getReferralbyTypeSenderidRequestHandler(IReferralRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PagedList<referralDTO>> Handle(getReferralbyTypeSenderidRequest request, CancellationToken cancellationToken)
        {
            var newtype = (type)request.mytype;
            var (items, totalCount) = await _repository.getRefferalBytpeandsenderid(request.senderid, newtype, request.userParams);

            // 2. Deserialize and Map
            var myref = items.Select(doc => BsonSerializer.Deserialize<Referral>(doc));
            var dtoItems = _mapper.Map<IEnumerable<referralDTO>>(myref);

            // 3. Instantiate the PagedList exactly ONCE with the mapped items
            var pagedDtoList = new PagedList<referralDTO>(
                dtoItems,
                totalCount,
                request.userParams.PageNumber,
                request.userParams.pageSize
            );

            return pagedDtoList;
        }
    }
}
