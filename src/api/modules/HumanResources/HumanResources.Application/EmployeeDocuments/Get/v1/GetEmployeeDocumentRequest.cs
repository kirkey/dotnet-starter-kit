namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;

public sealed record GetEmployeeDocumentRequest(DefaultIdType Id) : IRequest<EmployeeDocumentResponse>;

