using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CreditScores.Create.v1;

/// <summary>
/// Handler for creating a new credit score record.
/// </summary>
public sealed class CreateCreditScoreHandler(
    [FromKeyedServices("microfinance:creditscores")] IRepository<CreditScore> repository,
    ILogger<CreateCreditScoreHandler> logger)
    : IRequestHandler<CreateCreditScoreCommand, CreateCreditScoreResponse>
{
    public async Task<CreateCreditScoreResponse> Handle(CreateCreditScoreCommand request, CancellationToken cancellationToken)
    {
        var creditScore = CreditScore.Create(
            request.MemberId,
            request.ScoreType,
            request.Score,
            request.ScoreMin,
            request.ScoreMax,
            request.ScoreModel,
            request.LoanId,
            request.Source,
            request.CreditBureauReportId,
            request.ProbabilityOfDefault,
            request.ScoreFactors,
            request.ValidUntil);

        await repository.AddAsync(creditScore, cancellationToken);

        logger.LogInformation("Credit score {CreditScoreId} created for member {MemberId} - Score: {Score}, Grade: {Grade}",
            creditScore.Id, request.MemberId, creditScore.Score, creditScore.Grade);

        return new CreateCreditScoreResponse(
            creditScore.Id,
            creditScore.MemberId,
            creditScore.ScoreType,
            creditScore.Score,
            creditScore.Grade,
            creditScore.ScorePercentile);
    }
}
