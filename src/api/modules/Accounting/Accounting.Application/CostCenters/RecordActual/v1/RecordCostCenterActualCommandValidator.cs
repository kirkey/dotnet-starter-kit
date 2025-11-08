namespace Accounting.Application.CostCenters.RecordActual.v1;

/// <summary>
/// Validator for RecordCostCenterActualCommand.
/// </summary>
public sealed class RecordCostCenterActualCommandValidator : AbstractValidator<RecordCostCenterActualCommand>
{
    public RecordCostCenterActualCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Cost center ID is required.");
        RuleFor(x => x.Amount).LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.")
            .GreaterThanOrEqualTo(-999999999.99m).WithMessage("Amount must not be less than -999,999,999.99.");
    }
}

