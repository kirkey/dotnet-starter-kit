namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.MarkAsPaid.v1;

/// <summary>
/// Validator for MarkPayrollAsPaidCommand.
/// </summary>
public sealed class MarkPayrollAsPaidValidator : AbstractValidator<MarkPayrollAsPaidCommand>
{
    public MarkPayrollAsPaidValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Payroll ID is required.");
    }
}

