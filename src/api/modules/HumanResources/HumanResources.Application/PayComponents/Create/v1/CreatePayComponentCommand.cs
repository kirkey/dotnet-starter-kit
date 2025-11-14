namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Create.v1;

/// <summary>
/// Command to create a new pay component for payroll.
/// Components are: Earnings, Taxes, Deductions per Philippines requirements.
/// </summary>
public sealed record CreatePayComponentCommand(
    [property: DefaultValue("Basic Salary")] string ComponentName,
    [property: DefaultValue("Earnings")] string ComponentType,
    [property: DefaultValue("")] string GlAccountCode = "",
    [property: DefaultValue(null)] string? Description = null
) : IRequest<CreatePayComponentResponse>;

/// <summary>
/// Response for pay component creation.
/// </summary>
public sealed record CreatePayComponentResponse(
    DefaultIdType Id,
    string ComponentName,
    string ComponentType,
    bool IsActive);

