using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Deactivate.v1;

public sealed class DeactivateBranchHandler(
    ILogger<DeactivateBranchHandler> logger,
    [FromKeyedServices("microfinance:branches")] IRepository<Branch> repository)
    : IRequestHandler<DeactivateBranchCommand, DeactivateBranchResponse>
{
    public async Task<DeactivateBranchResponse> Handle(DeactivateBranchCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var branch = await repository.FirstOrDefaultAsync(
            new BranchByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (branch is null)
            throw new NotFoundException($"Branch with ID {request.Id} not found.");

        branch.Deactivate();

        await repository.UpdateAsync(branch, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Branch deactivated: {BranchId}", branch.Id);

        return new DeactivateBranchResponse(branch.Id, branch.Status);
    }
}
