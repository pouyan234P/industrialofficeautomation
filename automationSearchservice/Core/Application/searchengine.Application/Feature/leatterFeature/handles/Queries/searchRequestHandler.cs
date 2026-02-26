using MediatR;
using searchengine.Application.Feature.leatterFeature.request.Queries;
using searchengine.Application.IRepository;
using searchengine.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchengine.Application.Feature.leatterFeature.handles.Queries
{
    public class searchRequestHandler : IRequestHandler<SearchRequest, IEnumerable<LetterSearchDocument>>
    {
        private readonly ILetterSearchRepository _repository;

        public searchRequestHandler(ILetterSearchRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<LetterSearchDocument>> Handle(SearchRequest request, CancellationToken cancellationToken)
        {
            var response= await _repository.SearchAsync(request.keyword!, request.fromDate, request.toDate);
            return response;
        }
    }
}
