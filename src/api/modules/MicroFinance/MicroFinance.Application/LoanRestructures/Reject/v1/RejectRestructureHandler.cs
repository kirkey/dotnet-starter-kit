using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Reject.v1;

/// <summary>
/// Handles the reject loan restructure command.
/// </summary>
public sealed class RejectRestructureHandler(
    [FromKeyedServices("microfinance:loanrestructures")] IRepository<LoanRestructure> repository,
    ILogger<RejectRestructureHandler> logger)
    : IRequestHandler<RejectRestructureCommand, RejectRestructureResponse>
{
    /// <summary>
    /// Handles the command to reject a loan restructure.
    /// </summary>
    public async Task<RejectRestructureResponse> Handle(
        RejectRestructureCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var restructure = await repository.FirstOrDefaultAsync(
            new LoanRestructureByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new NotFoundException($"Loan restructure {request.Id} not found");

        restructure.Reject(request.UserId, request.Reason);
        await repository.UpdateAsync(restructure, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Loan restructure rejected: {RestructureId}, Reason: {Reason}", 
            restructure.Id, request.Reason);

        return new RejectRestructureResponse(restructure.Id, restructure.Status, request.Reason);
    }
}
