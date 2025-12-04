using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Documents.Get.v1;

/// <summary>
/// Request to get a document by ID.
/// </summary>
public sealed record GetDocumentRequest(Guid Id) : IRequest<DocumentResponse>;
