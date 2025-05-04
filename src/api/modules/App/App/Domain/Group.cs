using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.App.Domain.Events;

namespace FSH.Starter.WebApi.App.Domain;

public sealed class Group : AuditableEntity, IAggregateRoot
{
    private Group() { }

    public string Application { get; private set; }
    public string Parent { get; private set; }
    public string? Tag { get; set; }
    public int Number { get; private set; }
    public string Code { get; private set; }

    public decimal Amount { get; private set; }

    public DefaultIdType? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }

    public Group(DefaultIdType id, string application, string parent, string? tag, int number, string code, string name,
        decimal amount = 0, DefaultIdType? employeeId = null, string? employeeName = null,
        string? description = null, string? notes = null)
    {
        Id = id;

        Application = application.ToUpperInvariant();
        Parent = parent.Trim().ToUpperInvariant();
        if (tag is not null && Tag?.Equals(tag) != true) Tag = tag.Trim().ToUpperInvariant();
        Number = number;
        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim().ToUpperInvariant();
        Amount = amount;

        if (employeeId is not null && (EmployeeId is null || !EmployeeId!.Equals(employeeId))) EmployeeId = employeeId;
        if (employeeName is not null && (EmployeeName is null || !EmployeeName!.Equals(employeeName)))
            EmployeeName = employeeName;

        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(
            new GroupCreated(Id, Application, Parent, Tag, Number, Code, Name, Amount, EmployeeId, EmployeeName,
                Description, Notes));

        AppMetrics.Created.Add(1);
    }

    public static Group Create(string application, string parent, string? tag, int number, string code, string name,
        decimal amount = 0, DefaultIdType? employeeId = null, string? employeeName = null,
        string? description = null, string? notes = null)
    {
        return new Group(DefaultIdType.NewGuid(), application, parent, tag, number, code, name, amount,
            employeeId, employeeName, description, notes);
    }

    public Group Update(string application, string parent, string? tag, int number, string code, string name,
        decimal amount,
        DefaultIdType? employeeId, string? employeeName, string? description, string? notes)
    {
        bool isUpdated = false;

        if (!Application.Equals(application))
        {
            Application = application.ToUpperInvariant();
            isUpdated = true;
        }

        if (!Parent.Equals(parent))
        {
            Parent = parent.Trim().ToUpperInvariant();
            isUpdated = true;
        }

        if (tag is not null && Tag?.Equals(tag) != true)
        {
            Tag = tag.Trim().ToUpperInvariant();
            isUpdated = true;
        }

        if (!Number.Equals(number))
        {
            Number = number;
            isUpdated = true;
        }

        if (!Code.Equals(code))
        {
            Code = code.Trim().ToUpperInvariant();
            isUpdated = true;
        }

        if (!Name.Equals(name))
        {
            Name = name.Trim().ToUpperInvariant();
            isUpdated = true;
        }

        if (!Amount.Equals(amount))
        {
            Amount = amount;
            isUpdated = true;
        }

        if (employeeId is not null && (EmployeeId is null || !EmployeeId!.Equals(employeeId)))
        {
            EmployeeId = employeeId;
            isUpdated = true;
        }

        if (employeeName is not null && (EmployeeName is null || !EmployeeName!.Equals(employeeName)))
        {
            EmployeeName = employeeName;
            isUpdated = true;
        }

        if (description is not null && Description?.Equals(description) != true)
        {
            Description = description.Trim();
            isUpdated = true;
        }

        if (notes is not null && Notes?.Equals(notes) != true)
        {
            Notes = notes.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new GroupUpdated(this));
            AppMetrics.Updated.Add(1);
        }

        return this;
    }
}
