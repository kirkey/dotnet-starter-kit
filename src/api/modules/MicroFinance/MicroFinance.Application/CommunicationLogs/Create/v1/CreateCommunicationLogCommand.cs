using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Create.v1;

/// <summary>
/// Command to create a new communication log entry.
/// </summary>
public sealed record CreateCommunicationLogCommand(
    string Channel,
    string Recipient,
    string Body,
    Guid? MemberId = null,
    Guid? LoanId = null,
    Guid? TemplateId = null,
    string? Subject = null,
    Guid? SentByUserId = null) : IRequest<CreateCommunicationLogResponse>;
