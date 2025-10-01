namespace FSH.Starter.WebApi.Store.Application.Suppliers.Deactivate.v1;

/// <summary>
/// Validator for DeactivateSupplierCommand.
/// </summary>
public sealed class DeactivateSupplierCommandValidator : AbstractValidator<DeactivateSupplierCommand>
{
    public DeactivateSupplierCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

