namespace Accounting.Application.FiscalPeriodCloses.Create.v1;

/// <summary>
/// Validator for fiscal period close creation command.
/// </summary>
public class CreateFiscalPeriodCloseCommandValidator : AbstractValidator<CreateFiscalPeriodCloseCommand>
{
    public CreateFiscalPeriodCloseCommandValidator()
    {
        RuleFor(x => x.CloseNumber)
            .NotEmpty().WithMessage("Close number is required")
            .MaximumLength(64).WithMessage("Close number cannot exceed 50 characters");

        RuleFor(x => x.PeriodId)
            .NotEmpty().WithMessage("Period ID is required");

        RuleFor(x => x.CloseType)
            .NotEmpty().WithMessage("Close type is required")
            .Must(type => new[] { "MonthEnd", "QuarterEnd", "YearEnd" }.Contains(type))
            .WithMessage("Close type must be one of: MonthEnd, QuarterEnd, YearEnd");

        RuleFor(x => x.PeriodStartDate)
            .NotEmpty().WithMessage("Period start date is required")
            .LessThan(x => x.PeriodEndDate).WithMessage("Period start date must be before end date");

        RuleFor(x => x.PeriodEndDate)
            .NotEmpty().WithMessage("Period end date is required")
            .GreaterThan(x => x.PeriodStartDate).WithMessage("Period end date must be after start date");

        RuleFor(x => x.InitiatedBy)
            .NotEmpty().WithMessage("Initiator is required")
            .MaximumLength(256).WithMessage("Initiator cannot exceed 256 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2048).WithMessage("Description cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

