namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Update.v1;

/// <summary>
/// Validator for updating a tax bracket.
/// </summary>
public class UpdateTaxValidator : AbstractValidator<UpdateTaxCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTaxValidator"/> class.
    /// </summary>
    public UpdateTaxValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Tax bracket ID is required");

        RuleFor(x => x.FilingStatus)
            .MaximumLength(50)
            .WithMessage("Filing status cannot exceed 50 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.FilingStatus));

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

