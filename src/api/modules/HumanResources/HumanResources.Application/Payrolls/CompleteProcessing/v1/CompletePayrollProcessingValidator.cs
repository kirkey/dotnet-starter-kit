namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.CompleteProcessing.v1;

/// <summary>
/// Validator for CompletePayrollProcessingCommand.
/// </summary>
public sealed class CompletePayrollProcessingValidator : AbstractValidator<CompletePayrollProcessingCommand>
{
    public CompletePayrollProcessingValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Payroll ID is required.");
    }
}

