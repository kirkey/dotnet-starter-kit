using FluentValidation;

namespace Accounting.Application.Vendors.Update.v1;

public class VendorUpdateCommandValidator : AbstractValidator<VendorUpdateCommand>
{
    public VendorUpdateCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.VendorCode).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Phone).MaximumLength(32);
    }
}
