using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.AssignManager.v1;

public sealed class AssignBranchManagerHandler(
    ILogger<AssignBranchManagerHandler> logger,
    [FromKeyedServices("microfinance:branches")] IRepository<Branch> repository)
    : IRequestHandler<AssignBranchManagerCommand, AssignBranchManagerResponse>
{
    public async Task<AssignBranchManagerResponse> Handle(AssignBranchManagerCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var branch = await repository.FirstOrDefaultAsync(
            new BranchByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (branch is null)
            throw new NotFoundException($"Branch with ID {request.Id} not found.");

        branch.AssignManager(request.ManagerName, request.ManagerPhone, request.ManagerEmail);

        await repository.UpdateAsync(branch, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Branch manager assigned: {BranchId} - {ManagerName}", branch.Id, request.ManagerName);

        return new AssignBranchManagerResponse(branch.Id, request.ManagerName);
    }
}
