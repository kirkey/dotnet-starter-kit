using FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.OrganizationalUnits.Create.v1;

/// <summary>
/// Handler for creating a new organizational unit.
/// </summary>
public sealed class CreateOrganizationalUnitHandler(
    ILogger<CreateOrganizationalUnitHandler> logger,
    [FromKeyedServices("hr:organizationalunits")] IRepository<OrganizationalUnit> repository,
    [FromKeyedServices("hr:organizationalunits")] IReadRepository<OrganizationalUnit> readRepository)
    : IRequestHandler<CreateOrganizationalUnitCommand, CreateOrganizationalUnitResponse>
{
    public async Task<CreateOrganizationalUnitResponse> Handle(CreateOrganizationalUnitCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check if code already exists
        var existingUnit = await readRepository
            .FirstOrDefaultAsync(
                new OrganizationalUnitByCodeSpec(request.Code), 
                cancellationToken)
            .ConfigureAwait(false);

        if (existingUnit is not null)
        {
            throw new OrganizationalUnitCodeAlreadyExistsException(request.Code);
        }

        // Validate parent type if parent is specified
        if (request.ParentId.HasValue)
        {
            var parent = await readRepository
                .GetByIdAsync(request.ParentId.Value, cancellationToken)
                .ConfigureAwait(false);

            if (parent is null)
            {
                throw new OrganizationalUnitNotFoundException(request.ParentId.Value);
            }

            // Section must have Division parent
            if (request.Type == OrganizationalUnitType.Section && parent.Type != OrganizationalUnitType.Division)
            {
                throw new InvalidOrganizationalHierarchyException("Section parent must be a Division.");
            }

            // Division must have Department parent
            if (request.Type == OrganizationalUnitType.Division && parent.Type != OrganizationalUnitType.Department)
            {
                throw new InvalidOrganizationalHierarchyException("Division parent must be a Department.");
            }
        }

        // Create organizational unit using domain factory method
        var organizationalUnit = OrganizationalUnit.Create(
            request.Code,
            request.Name,
            request.Type,
            request.ParentId,
            request.ManagerId,
            request.CostCenter,
            request.Location);

        // Build hierarchy path
        if (request.ParentId.HasValue)
        {
            var parent = await readRepository.GetByIdAsync(request.ParentId.Value, cancellationToken).ConfigureAwait(false);
            var hierarchyPath = $"{parent!.HierarchyPath}{request.Code}/";
            organizationalUnit.UpdateHierarchyPath(hierarchyPath);
        }
        else
        {
            organizationalUnit.UpdateHierarchyPath($"/{request.Code}/");
        }

        // Persist to database
        await repository.AddAsync(organizationalUnit, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Organizational unit created with ID {UnitId}, Code {Code}, Type {Type}", 
            organizationalUnit.Id, 
            organizationalUnit.Code,
            organizationalUnit.Type);

        return new CreateOrganizationalUnitResponse(organizationalUnit.Id);
    }
}

