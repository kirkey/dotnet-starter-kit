namespace FSH.Starter.WebApi.Store.Application.Suppliers.Activate.v1;

/// <summary>
/// Validator for ActivateSupplierCommand.
/// </summary>
public sealed class ActivateSupplierCommandValidator : AbstractValidator<ActivateSupplierCommand>
{
    public ActivateSupplierCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

