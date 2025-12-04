using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Create.v1;

/// <summary>
/// Handler for creating a new branch.
/// </summary>
public sealed class CreateBranchHandler(
    ILogger<CreateBranchHandler> logger,
    [FromKeyedServices("microfinance:branches")] IRepository<Branch> repository)
    : IRequestHandler<CreateBranchCommand, CreateBranchResponse>
{
    public async Task<CreateBranchResponse> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var branch = Branch.Create(
            code: request.Code,
            name: request.Name,
            branchType: request.BranchType,
            parentBranchId: request.ParentBranchId,
            address: request.Address,
            city: request.City,
            state: request.State,
            country: request.Country,
            phone: request.Phone,
            email: request.Email,
            openingDate: request.OpeningDate);

        await repository.AddAsync(branch, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Branch created: {Code} - {Name}", branch.Code, branch.Name);

        return new CreateBranchResponse(branch.Id, branch.Code, branch.Name);
    }
}
