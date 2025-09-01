using FSH.Framework.Core.Extensions.Dto;
using MediatR;

namespace Accounting.Application.ChartOfAccounts.Create.v1;

public class ChartOfAccountCreateRequest : BaseRequest, IRequest<DefaultIdType>
{
    public string AccountCode { get; set; } = null!;
    public string AccountName { get; set; } = null!;
    public string AccountType { get; set; } = null!;
    public string UsoaCategory { get; set; } = null!;
    public DefaultIdType? SubAccountOf { get; set; }
    public string ParentCode { get; set; } = string.Empty;
    public decimal Balance { get; set; } = 0;
    public bool IsControlAccount { get; set; }
    public string NormalBalance { get; set; } = "Debit";
    public bool IsUsoaCompliant { get; set; } = true;
    public string? RegulatoryClassification { get; set; }
}
