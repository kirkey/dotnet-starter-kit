namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Create.v1;

public class CreateSalesOrderCommandValidator : AbstractValidator<CreateSalesOrderCommand>
{
    public CreateSalesOrderCommandValidator([FromKeyedServices("store:customers")] IReadRepository<Customer> customerRepository)
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer is required")
            .MustAsync(async (cid, ct) =>
            {
                var customer = await customerRepository.GetByIdAsync(cid, ct).ConfigureAwait(false);
                return customer is not null;
            }).WithMessage("Customer not found");

        RuleFor(x => x.Total)
            .GreaterThanOrEqualTo(0m).WithMessage("Total must be non-negative");

        // Note: OrderNumber is not part of the current CreateSalesOrderCommand; if you add it
        // later, add uniqueness checks against SalesOrder repository and length validation here.
    }
}
