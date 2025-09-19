namespace Accounting.Application.Budgets.Create;

public class CreateBudgetCommand(
    string name,
    DefaultIdType? projectId = null,
    decimal totalAmount = 0,
    string? notes = null) : IRequest<DefaultIdType>
{
    public string Name { get; set; } = name;
    public DefaultIdType? ProjectId { get; set; } = projectId;
    public decimal TotalAmount { get; set; } = totalAmount;
    public string? Notes { get; set; } = notes;
}
