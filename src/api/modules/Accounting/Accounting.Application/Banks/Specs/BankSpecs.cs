namespace Accounting.Application.Banks.Specs;

/// <summary>
/// Specification for finding a bank by its unique code.
/// </summary>
public class BankByCodeSpec : Specification<Bank>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankByCodeSpec"/> class.
    /// </summary>
    /// <param name="bankCode">The bank code to search for.</param>
    public BankByCodeSpec(string bankCode)
    {
        Query.Where(x => x.BankCode == bankCode);
    }
}

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

/// <summary>
/// Specification for finding a bank by its ID.
/// </summary>
public class BankByIdSpec : Specification<Bank>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankByIdSpec"/> class.
    /// </summary>
    /// <param name="id">The bank ID to search for.</param>
    public BankByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

/// <summary>
/// Specification for finding banks by name (partial match).
/// </summary>
public class BankByNameSpec : Specification<Bank>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BankByNameSpec"/> class.
    /// </summary>
    /// <param name="name">The name to search for (partial match).</param>
    public BankByNameSpec(string name)
    {
        Query.Where(x => x.Name.Contains(name));
    }
}

/// <summary>
/// Specification for exporting banks with selected fields.
/// </summary>
public class ExportBanksSpec : Specification<Bank>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExportBanksSpec"/> class.
    /// </summary>
    public ExportBanksSpec()
    {
        Query.OrderBy(x => x.BankCode);
    }
}

