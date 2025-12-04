using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.Branches.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Get.v1;

public sealed class GetBranchHandler(
    [FromKeyedServices("microfinance:branches")] IReadRepository<Branch> repository)
    : IRequestHandler<GetBranchRequest, BranchResponse>
{
    public async Task<BranchResponse> Handle(GetBranchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var branch = await repository.FirstOrDefaultAsync(
            new BranchByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (branch is null)
            throw new NotFoundException($"Branch with ID {request.Id} not found.");

        return new BranchResponse(
            branch.Id,
            branch.Code,
            branch.Name,
            branch.BranchType,
            branch.Status,
            branch.ParentBranchId,
            branch.Address,
            branch.City,
            branch.State,
            branch.Country,
            branch.PostalCode,
            branch.Phone,
            branch.Email,
            branch.ManagerName,
            branch.ManagerPhone,
            branch.ManagerEmail,
            branch.OpeningDate,
            branch.ClosingDate,
            branch.CashHoldingLimit,
            branch.Notes);
    }
}
