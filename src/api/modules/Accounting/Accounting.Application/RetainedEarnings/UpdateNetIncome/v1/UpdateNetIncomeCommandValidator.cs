namespace Accounting.Application.RetainedEarnings.UpdateNetIncome.v1;

public sealed class UpdateNetIncomeCommandValidator : AbstractValidator<UpdateNetIncomeCommand>
{
    public UpdateNetIncomeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Retained earnings ID is required.");
        RuleFor(x => x.NetIncome).LessThanOrEqualTo(999999999.99m).WithMessage("Net income must not exceed 999,999,999.99.")
            .GreaterThanOrEqualTo(-999999999.99m).WithMessage("Net loss must not exceed -999,999,999.99.");
    }
}

