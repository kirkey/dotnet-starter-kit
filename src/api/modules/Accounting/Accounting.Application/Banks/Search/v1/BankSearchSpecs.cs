using Accounting.Application.Banks.Get.v1;

namespace Accounting.Application.Banks.Search.v1;

/// <summary>
/// Specification for searching banks with filtering and pagination.
/// </summary>
public class BankSearchSpecs : EntitiesByPaginationFilterSpec<Bank, BankResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankSearchSpecs"/> class.
    /// </summary>
    /// <param name="request">The search request containing filter criteria.</param>
    public BankSearchSpecs(BankSearchRequest request)
        : base(request)
    {
        // Filter by bank code (partial match)
        Query.Where(x => x.BankCode.Contains(request.BankCode!), !string.IsNullOrWhiteSpace(request.BankCode));

        // Filter by name (partial match)
        Query.Where(x => x.Name.Contains(request.Name!), !string.IsNullOrWhiteSpace(request.Name));

        // Filter by routing number (exact match)
        Query.Where(x => x.RoutingNumber == request.RoutingNumber, !string.IsNullOrWhiteSpace(request.RoutingNumber));

        // Filter by SWIFT code (partial match)
        Query.Where(x => x.SwiftCode!.Contains(request.SwiftCode!), !string.IsNullOrWhiteSpace(request.SwiftCode));

        // Filter by active status
        Query.Where(x => x.IsActive == request.IsActive, request.IsActive.HasValue);

        // Default ordering by bank code
        Query.OrderBy(x => x.BankCode);
    }
}


