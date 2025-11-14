namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;

/// <summary>
/// Request to get an employee document by its identifier.
/// </summary>
public sealed record GetEmployeeDocumentRequest(DefaultIdType Id) : IRequest<EmployeeDocumentResponse>;
