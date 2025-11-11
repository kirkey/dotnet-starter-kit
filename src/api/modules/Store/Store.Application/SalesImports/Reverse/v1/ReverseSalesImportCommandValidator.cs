namespace FSH.Starter.WebApi.Store.Application.SalesImports.Reverse.v1;

/// <summary>
/// Validator for ReverseSalesImportCommand.
/// </summary>
public class ReverseSalesImportCommandValidator : AbstractValidator<ReverseSalesImportCommand>
{
    public ReverseSalesImportCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Sales import ID is required");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reversal reason is required")
            .MaximumLength(500).WithMessage("Reversal reason must not exceed 500 characters");
    }
}

