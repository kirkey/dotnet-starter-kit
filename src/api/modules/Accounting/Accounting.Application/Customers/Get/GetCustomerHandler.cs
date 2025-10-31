using Accounting.Application.Customers.Queries;

namespace Accounting.Application.Customers.Get;

/// <summary>
/// Request to get a customer by ID.
/// </summary>
public record GetCustomerRequest(DefaultIdType Id) : IRequest<CustomerDetailsDto>;

/// <summary>
/// Handler for retrieving a customer by ID.
/// </summary>
public class GetCustomerHandler(
    [FromKeyedServices("accounting")] IReadRepository<Customer> repository)
    : IRequestHandler<GetCustomerRequest, CustomerDetailsDto>
{
    public async Task<CustomerDetailsDto> Handle(
        GetCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customer = await repository.FirstOrDefaultAsync(
            new CustomerByIdSpec(request.Id),
            cancellationToken).ConfigureAwait(false);

        if (customer is null)
        {
            throw new NotFoundException(
                $"{nameof(Customer)} with ID {request.Id} was not found.");
        }

        return new CustomerDetailsDto
        {
            Id = customer.Id,
            CustomerNumber = customer.CustomerNumber,
            CustomerName = customer.CustomerName,
            CustomerType = customer.CustomerType,
            Email = customer.Email,
            Phone = customer.Phone,
            BillingAddress = customer.BillingAddress,
            ShippingAddress = customer.ShippingAddress,
            Fax = customer.Fax,
            ContactName = customer.ContactName,
            ContactEmail = customer.ContactEmail,
            ContactPhone = customer.ContactPhone,
            PaymentTerms = customer.PaymentTerms,
            CreditLimit = customer.CreditLimit,
            CurrentBalance = customer.CurrentBalance,
            AvailableCredit = customer.CreditLimit - customer.CurrentBalance,
            Status = customer.Status,
            IsActive = customer.IsActive,
            TaxExempt = customer.TaxExempt,
            TaxId = customer.TaxId,
            DiscountPercentage = customer.DiscountPercentage,
            IsOnCreditHold = customer.IsOnCreditHold,
            Description = customer.Description,
            Notes = customer.Notes,
            AccountOpenDate = customer.CreatedOn.DateTime
        };
    }
}

