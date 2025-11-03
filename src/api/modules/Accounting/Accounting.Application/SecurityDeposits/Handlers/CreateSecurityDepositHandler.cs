using Accounting.Application.SecurityDeposits.Commands;

namespace Accounting.Application.SecurityDeposits.Handlers;

/// <summary>
/// Handler for creating a new security deposit.
/// </summary>
public sealed class CreateSecurityDepositHandler(
    ILogger<CreateSecurityDepositHandler> logger,
    IRepository<SecurityDeposit> repository)
    : IRequestHandler<CreateSecurityDepositCommand, CreateSecurityDepositResponse>
{
    /// <summary>
    /// Handles the creation of a new security deposit.
    /// </summary>
    /// <param name="request">The create security deposit command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response containing the created security deposit ID.</returns>
    public async Task<CreateSecurityDepositResponse> Handle(
        CreateSecurityDepositCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var deposit = SecurityDeposit.Create(
            request.MemberId,
            request.Amount,
            request.DepositDate,
            notes: request.Notes);

        await repository.AddAsync(deposit, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Security deposit created for member {MemberId}: {DepositId}", 
            request.MemberId, deposit.Id);
        
        return new CreateSecurityDepositResponse(deposit.Id);
    }
}
