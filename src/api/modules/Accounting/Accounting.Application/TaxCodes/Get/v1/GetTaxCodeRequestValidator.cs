namespace Accounting.Application.TaxCodes.Get.v1;

/// <summary>
/// Validator for the GetTaxCodeRequest.
/// Ensures the ID parameter is valid before attempting retrieval.
/// </summary>
public class GetTaxCodeRequestValidator : AbstractValidator<GetTaxCodeRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetTaxCodeRequestValidator"/> class.
    /// </summary>
    public GetTaxCodeRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Tax code ID is required.");
    }
}

