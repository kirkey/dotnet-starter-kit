namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Get.v1;

/// <summary>
/// Request to get pay component details.
/// </summary>
public sealed record GetPayComponentRequest(DefaultIdType Id) : IRequest<PayComponentResponse>;

/// <summary>
/// Response with pay component details.
/// </summary>
public sealed record PayComponentResponse(
    DefaultIdType Id,
    string ComponentName,
    string ComponentType,
    string GlAccountCode,
    bool IsActive,
    bool IsCalculated,
    string? Description);

