namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Process.v1;

/// <summary>
/// Validator for ProcessPayrollCommand.
/// </summary>
public sealed class ProcessPayrollValidator : AbstractValidator<ProcessPayrollCommand>
{
    public ProcessPayrollValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Payroll ID is required.");
    }
}

