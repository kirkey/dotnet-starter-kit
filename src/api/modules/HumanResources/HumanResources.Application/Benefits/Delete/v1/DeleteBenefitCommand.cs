namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Delete.v1;

/// <summary>
/// Command to delete benefit.
/// </summary>
public sealed record DeleteBenefitCommand(
    DefaultIdType Id
) : IRequest<DeleteBenefitResponse>;

/// <summary>
/// Response for benefit deletion.
/// </summary>
public sealed record DeleteBenefitResponse(
    DefaultIdType Id,
    bool Success);

