using Accounting.Application.Accruals.Dtos;

namespace Accounting.Application.Accruals.Queries
{
    public class SearchAccrualsQuery : IRequest<List<AccrualDto>>
    {
        public string? AccrualNumber { get; set; }
        public string? Description { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}

