using FSH.Starter.WebApi.HumanResources.Application.Designations.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Create.v1;

/// <summary>
/// Handler for creating a new designation with area-specific salary configuration.
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

        // Check if code already exists
        var existingDesignation = await readRepository
            .FirstOrDefaultAsync(
                new DesignationByCodeSpec(request.Code),
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
            request.Area,
            request.Description,
            request.SalaryGrade,
            request.MinimumSalary,
            request.MaximumSalary,
            request.IsManagerial);

        // Persist to database
        await repository.AddAsync(designation, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Designation created with ID {DesignationId}, Code {Code}, Title {Title}, Area {Area}, Grade {SalaryGrade}",
            designation.Id,
            designation.Code,
            designation.Title,
            designation.Area,
            designation.SalaryGrade);

        return new CreateDesignationResponse(designation.Id);
    }
}

