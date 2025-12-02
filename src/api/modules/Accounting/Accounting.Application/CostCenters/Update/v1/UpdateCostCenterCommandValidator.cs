namespace Accounting.Application.CostCenters.Update.v1;

public sealed class UpdateCostCenterCommandValidator : AbstractValidator<UpdateCostCenterCommand>
{
    public UpdateCostCenterCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Cost center ID is required.");
        RuleFor(x => x.Name).MaximumLength(256).WithMessage("Name must not exceed 200 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));
        RuleFor(x => x.ManagerName).MaximumLength(256).WithMessage("Manager name must not exceed 256 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ManagerName));
        RuleFor(x => x.Location).MaximumLength(512).WithMessage("Location must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Location));
        RuleFor(x => x.Description).MaximumLength(2048).WithMessage("Description must not exceed 2048 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
        RuleFor(x => x.Notes).MaximumLength(2048).WithMessage("Notes must not exceed 2048 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
        RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Name) || x.ManagerId.HasValue || 
                                  !string.IsNullOrWhiteSpace(x.Location) || x.EndDate.HasValue || 
                                  !string.IsNullOrWhiteSpace(x.Description) || !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("At least one field must be provided for update.");
    }
}

