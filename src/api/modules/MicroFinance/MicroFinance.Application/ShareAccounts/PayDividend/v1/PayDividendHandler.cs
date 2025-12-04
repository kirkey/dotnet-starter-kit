using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PayDividend.v1;

/// <summary>
/// Handler for paying dividend.
/// </summary>
public sealed class PayDividendHandler(
    IRepository<ShareAccount> repository,
    ILogger<PayDividendHandler> logger)
    : IRequestHandler<PayDividendCommand, PayDividendResponse>
{
    public async Task<PayDividendResponse> Handle(PayDividendCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.ShareAccountId, cancellationToken)
            ?? throw new Exception($"Share account with ID {request.ShareAccountId} not found.");

        account.PayDividend(request.Amount);

        await repository.UpdateAsync(account, cancellationToken);
        logger.LogInformation("Paid dividend {Amount} from share account {AccountId}", request.Amount, request.ShareAccountId);

        return new PayDividendResponse(
            account.Id,
            request.Amount,
            account.TotalDividendsPaid,
            "Dividend paid successfully.");
    }
}
