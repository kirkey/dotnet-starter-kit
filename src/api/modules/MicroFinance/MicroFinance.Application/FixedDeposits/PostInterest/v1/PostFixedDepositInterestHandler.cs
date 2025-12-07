using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.PostInterest.v1;

/// <summary>
/// Handler for posting interest to fixed deposit.
/// </summary>
public sealed class PostFixedDepositInterestHandler(
    [FromKeyedServices("microfinance:fixeddeposits")] IRepository<FixedDeposit> repository,
    ILogger<PostFixedDepositInterestHandler> logger)
    : IRequestHandler<PostFixedDepositInterestCommand, PostFixedDepositInterestResponse>
{
    public async Task<PostFixedDepositInterestResponse> Handle(PostFixedDepositInterestCommand request, CancellationToken cancellationToken)
    {
        var deposit = await repository.GetByIdAsync(request.DepositId, cancellationToken)
            ?? throw new NotFoundException($"Fixed deposit with ID {request.DepositId} not found.");

        deposit.PostInterest(request.InterestAmount);

        await repository.UpdateAsync(deposit, cancellationToken);
        logger.LogInformation("Posted interest {Amount} to fixed deposit {DepositId}", request.InterestAmount, request.DepositId);

        return new PostFixedDepositInterestResponse(
            deposit.Id,
            request.InterestAmount,
            deposit.InterestEarned,
            "Interest posted successfully.");
    }
}
