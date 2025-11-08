namespace Accounting.Application.FixedAssets.UpdateMaintenance.v1;

public sealed class UpdateMaintenanceCommandValidator : AbstractValidator<UpdateMaintenanceCommand>
{
    public UpdateMaintenanceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Fixed asset ID is required.");
        RuleFor(x => x).Must(x => x.LastMaintenanceDate.HasValue || x.NextMaintenanceDate.HasValue)
            .WithMessage("At least one maintenance date must be provided.");
    }
}

