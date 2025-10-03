namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Get.v1;

/// <summary>
/// Validator for GetItemSupplierRequest.
/// </summary>
public sealed class GetItemSupplierCommandValidator : AbstractValidator<GetItemSupplierCommand>
{
    public GetItemSupplierCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
    }
}
