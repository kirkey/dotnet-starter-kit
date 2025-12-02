namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Post.v1;

/// <summary>
/// Validator for PostPayrollCommand.
/// </summary>
public sealed class PostPayrollValidator : AbstractValidator<PostPayrollCommand>
{
    public PostPayrollValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Payroll ID is required.");

        RuleFor(x => x.JournalEntryId)
            .NotEmpty().WithMessage("Journal Entry ID is required.")
            .MaximumLength(128).WithMessage("Journal Entry ID cannot exceed 100 characters.");
    }
}

