namespace FSH.Starter.WebApi.HumanResources.Application.Designations.Delete.v1;

/// <summary>
/// Command to delete designation.
/// </summary>
public sealed record DeleteDesignationCommand(DefaultIdType Id) : IRequest<DeleteDesignationResponse>;

