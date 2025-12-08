using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Approve.v1;

/// <summary>
/// Handler for approving a share account.
/// </summary>
public sealed class ApproveShareAccountHandler(
    [FromKeyedServices("microfinance:shareaccounts")] IRepository<ShareAccount> repository,
    ILogger<ApproveShareAccountHandler> logger)
    : IRequestHandler<ApproveShareAccountCommand, ApproveShareAccountResponse>
{
    public async Task<ApproveShareAccountResponse> Handle(ApproveShareAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.ShareAccountId, cancellationToken)
            ?? throw new NotFoundException($"Share account with ID {request.ShareAccountId} not found.");

        account.Approve(request.Notes);

        await repository.UpdateAsync(account, cancellationToken);
        logger.LogInformation("Approved share account {ShareAccountId}", request.ShareAccountId);

        return new ApproveShareAccountResponse(account.Id, account.Status, "Share account approved successfully.");
    }
}
