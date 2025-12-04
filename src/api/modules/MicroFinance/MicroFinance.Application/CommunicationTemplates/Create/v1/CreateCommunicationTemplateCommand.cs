using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Create.v1;

public sealed record CreateCommunicationTemplateCommand(
    string Code,
    string Name,
    string Channel,
    string Category,
    string Body,
    string? Subject = null,
    string? Placeholders = null,
    string Language = "en",
    bool RequiresApproval = false) : IRequest<CreateCommunicationTemplateResponse>;
