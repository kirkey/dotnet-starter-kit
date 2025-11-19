namespace FSH.Starter.WebApi.HumanResources.Application.DocumentTemplates.Get.v1;

public sealed record GetDocumentTemplateRequest(DefaultIdType Id) : IRequest<DocumentTemplateResponse>;

