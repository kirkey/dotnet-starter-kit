using Carter;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Create.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Application.Documents.Verify.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Endpoints;

public class DocumentEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("microfinance/documents").WithTags("Documents");

        group.MapPost("/", async (CreateDocumentCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Created($"/microfinance/documents/{result.Id}", result);
        })
        .WithName("CreateDocument")
        .WithSummary("Create a new document record")
        .Produces<CreateDocumentResponse>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetDocumentRequest(id));
            return Results.Ok(result);
        })
        .WithName("GetDocument")
        .WithSummary("Get document by ID")
        .Produces<DocumentResponse>();

        group.MapPost("/{id:guid}/verify", async (Guid id, VerifyDocumentRequest request, ISender sender) =>
        {
            var result = await sender.Send(new VerifyDocumentCommand(id, request.VerifiedById));
            return Results.Ok(result);
        })
        .WithName("VerifyDocument")
        .WithSummary("Verify a document")
        .Produces<VerifyDocumentResponse>();

    }
}

public record VerifyDocumentRequest(Guid VerifiedById);
