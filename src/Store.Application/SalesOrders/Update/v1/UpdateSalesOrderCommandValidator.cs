namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Update.v1;

public class UpdateSalesOrderCommandValidator : AbstractValidator<UpdateSalesOrderCommand>
{
    public UpdateSalesOrderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Sales order ID is required");

        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer is required");

        RuleFor(x => x.Total)
            .GreaterThanOrEqualTo(0m).WithMessage("Total must be non-negative");
    }
}
