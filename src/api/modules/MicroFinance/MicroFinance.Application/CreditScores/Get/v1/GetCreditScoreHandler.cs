using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Get.v1;

/// <summary>
/// Handler for getting a credit score by ID.
/// </summary>
public sealed class GetCreditScoreHandler(
    [FromKeyedServices("microfinance:creditscores")] IReadRepository<CreditScore> repository,
    ILogger<GetCreditScoreHandler> logger)
    : IRequestHandler<GetCreditScoreRequest, CreditScoreResponse>
{
    public async Task<CreditScoreResponse> Handle(GetCreditScoreRequest request, CancellationToken cancellationToken)
    {
        var spec = new CreditScoreByIdSpec(request.Id);
        var creditScore = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (creditScore is null)
        {
            throw new NotFoundException($"Credit score with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved credit score {CreditScoreId}", request.Id);

        return creditScore;
    }
}
