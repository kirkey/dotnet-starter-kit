namespace FSH.Starter.WebApi.HumanResources.Application.PayComponents.Delete.v1;

/// <summary>
/// Command to delete pay component.
/// </summary>
public sealed record DeletePayComponentCommand(
    DefaultIdType Id
) : IRequest<DeletePayComponentResponse>;

/// <summary>
/// Response for pay component deletion.
/// </summary>
public sealed record DeletePayComponentResponse(
    DefaultIdType Id,
    bool Success);

