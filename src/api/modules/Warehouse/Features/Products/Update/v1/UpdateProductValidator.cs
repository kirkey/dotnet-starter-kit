using FluentValidation;

namespace FSH.Starter.WebApi.Warehouse.Features.Products.Update.v1;

public sealed class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(p => p.Id).NotEmpty();
    }
}
