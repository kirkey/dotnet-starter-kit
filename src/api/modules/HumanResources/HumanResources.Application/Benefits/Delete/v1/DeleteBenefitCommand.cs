namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Delete.v1;

/// <summary>
/// Command to delete a benefit.
/// </summary>
public sealed record DeleteBenefitCommand(DefaultIdType Id) : IRequest<DeleteBenefitResponse>;

