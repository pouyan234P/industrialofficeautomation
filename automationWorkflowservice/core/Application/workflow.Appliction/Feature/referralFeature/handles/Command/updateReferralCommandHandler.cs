using MediatR;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using workflow.Appliction.Feature.referralFeature.request.Command;
using workflow.Appliction.IRepository;
using workflow.domain.Enum;

namespace workflow.Appliction.Feature.referralFeature.handles.Command
{
    public class updateReferralCommandHandler : IRequestHandler<updateReferralCommand>
    {
        private readonly IReferralRepository _repository;

        public updateReferralCommandHandler(IReferralRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(updateReferralCommand request, CancellationToken cancellationToken)
        {
            var updateData = new BsonDocument
            {
                { "_id", ObjectId.Parse(request.referral.id) }, // آی‌دی رکورد
                { "ViewDate", DateTime.Now },                // زمان مشاهده
                { "Status", request.referral.Status }  // چون در مدل شما String تنظیم شده است
            };
            await _repository.Update(updateData);

            return Unit.Value; // Ensure a value is returned to fix CS0161
        }
    }
}
