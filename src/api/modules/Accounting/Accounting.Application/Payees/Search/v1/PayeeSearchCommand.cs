using System.ComponentModel;
using Accounting.Application.Payees.Get.v1;

namespace Accounting.Application.Payees.Search.v1;

/// <summary>
/// Command for searching payees with pagination and filtering capabilities.
/// Follows the CQRS pattern for query operations with comprehensive search functionality.
/// </summary>
public class PayeeSearchCommand : PaginationFilter, IRequest<PagedList<PayeeResponse>>
{
    /// <summary>
    /// Filter payees by specific payee code.
    /// </summary>
    [DefaultValue("")]
    public string? PayeeCode { get; set; }

    /// <summary>
    /// Filter payees by name.
    /// </summary>
    [DefaultValue("")]
    public string? Name { get; set; }

    /// <summary>
    /// Filter payees by expense account code.
    /// </summary>
    [DefaultValue("")]
    public string? ExpenseAccountCode { get; set; }

    /// <summary>
    /// Filter payees by tax identification number.
    /// </summary>
    [DefaultValue("")]
    public string? Tin { get; set; }
}
