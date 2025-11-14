using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Search.v1;

/// <summary>
/// Request to search bank accounts with filtering and pagination.
/// </summary>
public class SearchBankAccountsRequest : PaginationFilter, IRequest<PagedList<BankAccountResponse>>
{
    /// <summary>
    /// Gets or sets the employee ID filter.
    /// </summary>
    public DefaultIdType? EmployeeId { get; set; }

    /// <summary>
    /// Gets or sets the account type filter.
    /// </summary>
    public string? AccountType { get; set; }

    /// <summary>
    /// Gets or sets the bank name search string.
    /// </summary>
    public string? BankName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by primary accounts only.
    /// </summary>
    public bool? IsPrimary { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by active accounts only.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to filter by verified accounts only.
    /// </summary>
    public bool? IsVerified { get; set; }
}

