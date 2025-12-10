using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Update.v1;

public sealed class UpdateBranchHandler(
    ILogger<UpdateBranchHandler> logger,
    [FromKeyedServices("microfinance:branches")] IRepository<Branch> repository)
    : IRequestHandler<UpdateBranchCommand, UpdateBranchResponse>
{
    public async Task<UpdateBranchResponse> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var branch = await repository.FirstOrDefaultAsync(
            new BranchByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (branch is null)
            throw new NotFoundException($"Branch with ID {request.Id} not found.");

        branch.Update(
            request.Name,
            request.Address,
            request.Phone,
            request.Email,
            request.ManagerName,
            request.ManagerPhone,
            request.ManagerEmail,
            request.Latitude,
            request.Longitude,
            request.OperatingHours,
            request.Timezone,
            request.CashHoldingLimit,
            request.Notes);

        await repository.UpdateAsync(branch, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Branch updated: {BranchId}", branch.Id);

        return new UpdateBranchResponse(branch.Id);
    }
}
