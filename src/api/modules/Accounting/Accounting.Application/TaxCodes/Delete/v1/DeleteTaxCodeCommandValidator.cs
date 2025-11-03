namespace Accounting.Application.TaxCodes.Delete.v1;

/// <summary>
/// Validator for the DeleteTaxCodeCommand.
/// Ensures the ID parameter is valid before attempting deletion.
/// </summary>
public class DeleteTaxCodeCommandValidator : AbstractValidator<DeleteTaxCodeCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteTaxCodeCommandValidator"/> class.
    /// </summary>
    public DeleteTaxCodeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Tax code ID is required.");
    }
}

