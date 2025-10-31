using Accounting.Application.Customers.Queries;

namespace Accounting.Application.Customers.Search.v1;

/// <summary>
/// Request to search for customers with optional filters.
/// </summary>
public record SearchCustomersRequest(
    string? CustomerNumber = null,
    string? CustomerName = null,
    string? CustomerType = null,
    string? Status = null,
    bool? IsActive = null) : IRequest<List<CustomerDto>>;

