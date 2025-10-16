using Accounting.Domain.Entities;

namespace Accounting.Application.Banks.Queries;

/// <summary>
/// Specification for finding a bank by its routing number.
/// </summary>
public class BankByRoutingNumberSpec : Specification<Bank>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankByRoutingNumberSpec"/> class.
    /// </summary>
    /// <param name="routingNumber">The routing number to search for.</param>
    public BankByRoutingNumberSpec(string routingNumber)
    {
        Query.Where(x => x.RoutingNumber == routingNumber);
    }
}

