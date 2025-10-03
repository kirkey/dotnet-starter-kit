namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Delete.v1;

/// <summary>
/// Validator for DeleteItemSupplierCommand.
/// </summary>
public sealed class DeleteItemSupplierCommandValidator : AbstractValidator<DeleteItemSupplierCommand>
{
    public DeleteItemSupplierCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
    }
}
