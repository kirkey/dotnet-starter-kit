namespace FSH.Starter.WebApi.HumanResources.Application.Companies.Get.v1;

/// <summary>
/// Request to get company by ID.
/// </summary>
public sealed record GetCompanyRequest(DefaultIdType Id) : IRequest<CompanyResponse>;

