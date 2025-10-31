using Accounting.Application.Customers.Queries;

namespace Accounting.Application.Customers.Search.v1;

/// <summary>
/// Handler for searching customers with filters.
/// </summary>
public sealed class SearchCustomersHandler(
    ILogger<SearchCustomersHandler> logger,
    [FromKeyedServices("accounting")] IReadRepository<Customer> repository)
    : IRequestHandler<SearchCustomersRequest, List<CustomerDto>>
{
    public async Task<List<CustomerDto>> Handle(SearchCustomersRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new CustomerSearchSpec(
            request.CustomerNumber,
            request.CustomerName,
            request.CustomerType,
            request.Status,
            request.IsActive);

        var customers = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Retrieved {Count} customers", customers.Count);

        return customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            CustomerNumber = c.CustomerNumber,
            CustomerName = c.CustomerName,
            CustomerType = c.CustomerType.ToString(),
            Email = c.Email,
            Phone = c.Phone,
            CreditLimit = c.CreditLimit,
            CurrentBalance = c.CurrentBalance,
            Status = c.Status,
            IsActive = c.IsActive
        }).ToList();
    }
}

