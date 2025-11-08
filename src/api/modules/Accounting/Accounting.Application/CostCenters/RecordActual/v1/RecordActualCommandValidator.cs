namespace Accounting.Application.CostCenters.RecordActual.v1;

public sealed class RecordActualCommandValidator : AbstractValidator<RecordActualCommand>
{
    public RecordActualCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Cost center ID is required.");
        RuleFor(x => x.Amount).LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.")
            .GreaterThanOrEqualTo(-999999999.99m).WithMessage("Amount must not be less than -999,999,999.99.");
    }
}

