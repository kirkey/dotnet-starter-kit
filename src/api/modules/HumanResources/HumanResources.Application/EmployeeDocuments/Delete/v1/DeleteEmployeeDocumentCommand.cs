namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Delete.v1;

public sealed record DeleteEmployeeDocumentCommand(DefaultIdType Id) : IRequest<DeleteEmployeeDocumentResponse>;

