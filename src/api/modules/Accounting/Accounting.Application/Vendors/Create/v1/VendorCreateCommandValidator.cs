using FluentValidation;

namespace Accounting.Application.Vendors.Create.v1;

public class VendorCreateCommandValidator : AbstractValidator<VendorCreateCommand>
{
    public VendorCreateCommandValidator()
    {
        RuleFor(x => x.VendorCode).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Phone).MaximumLength(32);
    }
}
