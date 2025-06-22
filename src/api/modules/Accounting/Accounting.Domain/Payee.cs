using Accounting.Domain.Events.Payee;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class Payee : AuditableEntity, IAggregateRoot
{
    public string PayeeCode { get; private set; }
    public string? Address { get; private set; }
    public string? ExpenseAccountCode { get; private set; }
    public string? ExpenseAccountName { get; private set; }
    public string? Tin { get; private set; }

    private Payee(DefaultIdType id, string payeeCode, string name, string? address, string? expenseAccountCode, string? expenseAccountName, string? tin, string? description, string? notes)
    {
        Id = id;
        PayeeCode = payeeCode.Trim();
        Name = name.Trim();
        Address = address?.Trim();
        ExpenseAccountCode = expenseAccountCode?.Trim();
        ExpenseAccountName = expenseAccountName?.Trim();
        Tin = tin?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();
    }

    public static Payee Create(string payeeCode, string name, string? address, string? expenseAccountCode, string? expenseAccountName, string? tin, string? description, string? notes)
    {
        return new Payee(DefaultIdType.NewGuid(), payeeCode, name, address, expenseAccountCode, expenseAccountName, tin, description, notes);
    }

    public Payee Update(string? payeeCode, string? name, string? address, string? expenseAccountCode, string? expenseAccountName, string? tin, string? description, string? notes)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(payeeCode) && !string.Equals(PayeeCode, payeeCode, StringComparison.OrdinalIgnoreCase))
        {
            PayeeCode = payeeCode.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(address) && !string.Equals(Address, address, StringComparison.OrdinalIgnoreCase))
        {
            Address = address.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(expenseAccountCode) && !string.Equals(ExpenseAccountCode, expenseAccountCode, StringComparison.OrdinalIgnoreCase))
        {
            ExpenseAccountCode = expenseAccountCode.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(expenseAccountName) && !string.Equals(ExpenseAccountName, expenseAccountName, StringComparison.OrdinalIgnoreCase))
        {
            ExpenseAccountName = expenseAccountName.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(tin) && !string.Equals(Tin, tin, StringComparison.OrdinalIgnoreCase))
        {
            Tin = tin.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(description) && !string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description.Trim();
            isUpdated = true;
        }
        if (!string.IsNullOrWhiteSpace(notes) && !string.Equals(Notes, notes, StringComparison.OrdinalIgnoreCase))
        {
            Notes = notes.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            // Optionally queue a domain event here, e.g.:
            QueueDomainEvent(new PayeeUpdated(this));
        }

        return this;
    }
}
