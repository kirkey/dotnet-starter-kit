using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.SetLossParameters.v1;

/// <summary>
/// Handler for setting loss parameters on a credit score.
/// </summary>
public sealed class SetLossParametersHandler(
    [FromKeyedServices("microfinance:creditscores")] IRepository<CreditScore> repository,
    ILogger<SetLossParametersHandler> logger)
    : IRequestHandler<SetLossParametersCommand, SetLossParametersResponse>
{
    public async Task<SetLossParametersResponse> Handle(SetLossParametersCommand request, CancellationToken cancellationToken)
    {
        var creditScore = await repository.GetByIdAsync(request.CreditScoreId, cancellationToken);

        if (creditScore is null)
        {
            throw new NotFoundException($"Credit score with ID {request.CreditScoreId} not found.");
        }

        creditScore.SetLossParameters(
            request.ProbabilityOfDefault,
            request.LossGivenDefault,
            request.ExposureAtDefault);

        await repository.UpdateAsync(creditScore, cancellationToken);

        logger.LogInformation("Loss parameters set for credit score {CreditScoreId} - Expected Loss: {ExpectedLoss}",
            request.CreditScoreId, creditScore.ExpectedLoss);

        return new SetLossParametersResponse(creditScore.Id, creditScore.ExpectedLoss);
    }
}
