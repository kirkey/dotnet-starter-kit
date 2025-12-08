using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CommunicationLogs.Create.v1;

/// <summary>
/// Command to create a new communication log entry.
/// </summary>
public sealed record CreateCommunicationLogCommand(
    string Channel,
    string Recipient,
    string Body,
    DefaultIdType? MemberId = null,
    DefaultIdType? LoanId = null,
    DefaultIdType? TemplateId = null,
    string? Subject = null,
    DefaultIdType? SentByUserId = null) : IRequest<CreateCommunicationLogResponse>;
