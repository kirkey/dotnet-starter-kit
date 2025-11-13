namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Delete.v1;

/// <summary>
/// Command to delete company.
/// </summary>
public sealed record DeleteCompanyCommand(DefaultIdType Id) : IRequest<DeleteCompanyResponse>;

