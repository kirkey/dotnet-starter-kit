namespace FSH.Starter.WebApi.Store.Application.Bins.Create.v1;

public class CreateBinCommandValidator : AbstractValidator<CreateBinCommand>
{
    public CreateBinCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(c => c.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters.");

        RuleFor(c => c.WarehouseLocationId)
            .NotEmpty().WithMessage("WarehouseLocationId is required.");

        RuleFor(c => c.BinType)
            .NotEmpty().WithMessage("BinType is required.")
            .MaximumLength(50).WithMessage("BinType must not exceed 50 characters.");

        RuleFor(c => c.Capacity)
            .GreaterThanOrEqualTo(0).When(c => c.Capacity.HasValue)
            .WithMessage("Capacity cannot be negative.");

        RuleFor(c => c.Priority)
            .GreaterThanOrEqualTo(0).WithMessage("Priority cannot be negative.");
    }
}
