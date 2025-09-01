using FSH.Framework.Core.Extensions.Dto;
using MediatR;

namespace Accounting.Application.AccountReconciliation.Commands.ReconcileAccount.v1;

public class ReconcileAccountCommand : BaseRequest, IRequest<DefaultIdType>
{
    public DefaultIdType ChartOfAccountId { get; set; }
    public DateTime ReconciliationDate { get; set; }
    public decimal StatementBalance { get; set; }
    public string? ReconciliationReference { get; set; }
    public List<ReconciliationLineDto> ReconciliationLines { get; set; } = new();
    public string? Notes { get; set; }
}

public class ReconciliationLineDto
{
    public DefaultIdType? TransactionId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = null!;
    public decimal Amount { get; set; }
    public bool IsCleared { get; set; }
}
