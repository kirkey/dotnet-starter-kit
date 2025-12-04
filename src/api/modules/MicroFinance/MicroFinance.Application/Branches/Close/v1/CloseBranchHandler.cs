using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Close.v1;

public sealed class CloseBranchHandler(
    ILogger<CloseBranchHandler> logger,
    [FromKeyedServices("microfinance:branches")] IRepository<Branch> repository)
    : IRequestHandler<CloseBranchCommand, CloseBranchResponse>
{
    public async Task<CloseBranchResponse> Handle(CloseBranchCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var branch = await repository.FirstOrDefaultAsync(
            new BranchByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (branch is null)
            throw new NotFoundException($"Branch with ID {request.Id} not found.");

        branch.Close(request.ClosingDate);

        await repository.UpdateAsync(branch, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Branch closed: {BranchId}", branch.Id);

        return new CloseBranchResponse(branch.Id, branch.Status, branch.ClosingDate!.Value);
    }
}
