using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Update.v1;

public sealed record UpdateCommunicationTemplateCommand(
    DefaultIdType Id,
    string? Name,
    string? Subject,
    string? Body,
    string? Placeholders,
    bool? RequiresApproval,
    string? Notes) : IRequest<UpdateCommunicationTemplateResponse>;
