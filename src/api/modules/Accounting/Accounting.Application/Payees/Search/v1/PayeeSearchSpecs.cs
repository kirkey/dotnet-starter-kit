using Accounting.Application.Payees.Get.v1;
using Accounting.Domain.Entities;

namespace Accounting.Application.Payees.Search.v1;

/// <summary>
/// Specification for searching payees with comprehensive filtering capabilities.
/// Implements pagination and multiple search criteria following the CQRS pattern.
/// </summary>
public class PayeeSearchSpecs : EntitiesByPaginationFilterSpec<Payee, PayeeResponse>
{
    /// <summary>
    /// Initializes a new instance of the PayeeSearchSpecs class with search criteria.
    /// </summary>
    /// <param name="command">The search command containing filter criteria and pagination settings.</param>
    public PayeeSearchSpecs(PayeeSearchCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(p => p.Name.Contains(command.Keyword!) || 
                       p.PayeeCode.Contains(command.Keyword!) || 
                       p.Description!.Contains(command.Keyword!), 
                   !string.IsNullOrEmpty(command.Keyword))
            .Where(p => p.PayeeCode.Contains(command.PayeeCode!), 
                   !string.IsNullOrEmpty(command.PayeeCode))
            .Where(p => p.Name.Contains(command.Name!), 
                   !string.IsNullOrEmpty(command.Name))
            .Where(p => p.ExpenseAccountCode != null && p.ExpenseAccountCode.Contains(command.ExpenseAccountCode!), 
                   !string.IsNullOrEmpty(command.ExpenseAccountCode))
            .Where(p => p.Tin != null && p.Tin.Contains(command.Tin!), 
                   !string.IsNullOrEmpty(command.Tin));
}
