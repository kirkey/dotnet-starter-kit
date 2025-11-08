namespace Accounting.Application.PrepaidExpenses.Update.v1;

public sealed class UpdatePrepaidExpenseCommandValidator : AbstractValidator<UpdatePrepaidExpenseCommand>
{
    public UpdatePrepaidExpenseCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Prepaid expense ID is required.");
        RuleFor(x => x.Description).MaximumLength(2048).WithMessage("Description must not exceed 2048 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
        RuleFor(x => x.Notes).MaximumLength(2048).WithMessage("Notes must not exceed 2048 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
        RuleFor(x => x).Must(x => !string.IsNullOrWhiteSpace(x.Description) || x.EndDate.HasValue || 
                                  x.CostCenterId.HasValue || !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("At least one field must be provided for update.");
    }
}

