using MediatR;
using searchengine.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchengine.Application.Feature.leatterFeature.request.Queries
{
    public class SearchRequest: IRequest<IEnumerable<LetterSearchDocument>>
    {
        public string? keyword { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }
}
