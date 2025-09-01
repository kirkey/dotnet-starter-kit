using Accounting.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.DeferredRevenue.Queries;
using Accounting.Application.DeferredRevenue.Dtos;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;

namespace Accounting.Application.DeferredRevenue.Handlers
{
    public class GetDeferredRevenueByIdHandler(IReadRepository<Accounting.Domain.DeferredRevenue> repository)
        : IRequestHandler<GetDeferredRevenueByIdQuery, DeferredRevenueDto>
    {
        public async Task<DeferredRevenueDto> Handle(GetDeferredRevenueByIdQuery request, CancellationToken cancellationToken)
        {
            var deferred = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (deferred == null)
                throw new NotFoundException($"DeferredRevenue with Id {request.Id} not found");
            return new DeferredRevenueDto
            {
                Id = deferred.Id,
                DeferredRevenueNumber = deferred.DeferredRevenueNumber,
                RecognitionDate = deferred.RecognitionDate,
                Amount = deferred.Amount,
                Description = deferred.Description,
                IsRecognized = deferred.IsRecognized,
                RecognizedDate = deferred.RecognizedDate
            };
        }
    }
}
