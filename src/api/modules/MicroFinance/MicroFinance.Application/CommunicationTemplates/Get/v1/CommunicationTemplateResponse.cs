namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationTemplates.Get.v1;

public sealed record CommunicationTemplateResponse(
    Guid Id,
    string Code,
    string Name,
    string Channel,
    string Category,
    string? Subject,
    string Body,
    string? Placeholders,
    string Language,
    bool RequiresApproval,
    string Status);
