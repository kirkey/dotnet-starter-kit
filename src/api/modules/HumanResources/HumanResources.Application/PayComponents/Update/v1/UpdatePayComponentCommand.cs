namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Update.v1;

/// <summary>
/// Command to update pay component details.
/// </summary>
public sealed record UpdatePayComponentCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? ComponentName = null,
    [property: DefaultValue(null)] string? GlAccountCode = null,
    [property: DefaultValue(null)] string? Description = null,
    [property: DefaultValue(null)] bool? IsActive = null
) : IRequest<UpdatePayComponentResponse>;

/// <summary>
/// Response for pay component update.
/// </summary>
public sealed record UpdatePayComponentResponse(
    DefaultIdType Id,
    string ComponentName,
    bool IsActive);

