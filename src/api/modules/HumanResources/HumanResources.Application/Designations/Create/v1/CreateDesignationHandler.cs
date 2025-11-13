using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Specifications;
using FSH.Starter.WebApi.HumanResources.Domain.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Create.v1;

/// <summary>
/// Handler for creating a new designation.
/// </summary>
public sealed class CreateDesignationHandler(
    ILogger<CreateDesignationHandler> logger,
    [FromKeyedServices("hr:designations")] IRepository<Designation> repository,
    [FromKeyedServices("hr:designations")] IReadRepository<Designation> readRepository)
    : IRequestHandler<CreateDesignationCommand, CreateDesignationResponse>
{
    public async Task<CreateDesignationResponse> Handle(CreateDesignationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check if code already exists in this organizational unit
        var existingDesignation = await readRepository
            .FirstOrDefaultAsync(
                new DesignationByCodeAndOrgUnitSpec(request.OrganizationalUnitId, request.Code),
                cancellationToken)
            .ConfigureAwait(false);

        if (existingDesignation is not null)
        {
            throw new DesignationCodeAlreadyExistsException(request.Code);
        }

        // Create designation using domain factory method
        var designation = Designation.Create(
            request.Code,
            request.Title,
            request.OrganizationalUnitId,
            request.Description,
            request.MinSalary,
            request.MaxSalary);

        // Persist to database
        await repository.AddAsync(designation, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Designation created with ID {DesignationId}, Code {Code}, in OrganizationalUnit {OrgUnitId}",
            designation.Id,
            designation.Code,
            designation.OrganizationalUnitId);

        return new CreateDesignationResponse(designation.Id);
    }
}

