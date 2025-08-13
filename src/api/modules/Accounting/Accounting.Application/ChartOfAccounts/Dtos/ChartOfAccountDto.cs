using FSH.Framework.Core.Extensions.Dto;

namespace Accounting.Application.ChartOfAccounts.Dtos;

public class ChartOfAccountDto : BaseDto
{
    public string AccountCategory { get; set; } = null!;
    public string AccountType { get; set; } = null!;
    public string ParentCode { get; set; } = null!;
    public string AccountCode { get; set; } = null!;
    // Name comes from AuditableEntity.Name
    public string NormalBalance { get; set; } = null!;
    public int AccountLevel { get; set; }
    public bool IsControlAccount { get; set; }
    public bool AllowDirectPosting { get; set; }
    public bool IsUsoaCompliant { get; set; }
    public string? RegulatoryClassification { get; set; }
    public decimal Balance { get; set; }
}
