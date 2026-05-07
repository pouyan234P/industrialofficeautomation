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
    public class getReferralbyTypeRequestHandler : IRequestHandler<getReferralbyTypeRequest, PagedList<referralDTO>>
    {
        private readonly IReferralRepository _repository;
        private readonly IMapper _mapper;

        public getReferralbyTypeRequestHandler(IReferralRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PagedList<referralDTO>> Handle(getReferralbyTypeRequest request, CancellationToken cancellationToken)
        {
            var newtype = (type)request.mytype;
            var (items, totalCount) = await _repository.getRefferalBytpeandreciverid(request.reciverID,newtype, request.UserParams);

            // 2. Deserialize and Map
            var myref = items.Select(doc => BsonSerializer.Deserialize<Referral>(doc));
            var dtoItems = _mapper.Map<IEnumerable<referralDTO>>(myref);

            // 3. Instantiate the PagedList exactly ONCE with the mapped items
            var pagedDtoList = new PagedList<referralDTO>(
                dtoItems,
                totalCount,
                request.UserParams.PageNumber,
                request.UserParams.pageSize
            );

            return pagedDtoList;
        }
    }
}
